using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.CSharp.Refactoring;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Polaris.MSBuild.Common
{
    public class CopyrightWriterTask : Task
    {
        // http://stackoverflow.com/questions/153602/create-msbuild-custom-task-to-modify-c-sharp-code-before-compile
        // http://msdn.microsoft.com/en-us/magazine/dd483291.aspx#id0100020
        // http://blogs.msdn.com/b/msbuild/archive/2006/02/02/523714.aspx
        // http://en.wikipedia.org/wiki/String_metric
        // http://blogs.msdn.com/b/toub/archive/2006/05/05/590814.aspx

        [Required]
        public ITaskItem[] InputFiles { get; set; }

        [Required]
        public string Copyrights { get; set; }

        public bool SupportRegion
        {
            get { return _supportRegion; }
            set { _supportRegion = value; }
        }
        private bool _supportRegion = true;

        public string RegionCaption
        {
            get { return _regionCaption; }
            set { _regionCaption = value; }
        }
        private string _regionCaption = string.Empty;

        public int CopyrightReplacementDistance
        {
            get { return _copyrightReplacementDistance; }
            set { _copyrightReplacementDistance = value; }
        }
        private int _copyrightReplacementDistance = 25;

        public int HeaderReplacementDistance
        {
            get { return _headerReplacementDistance; }
            set { _headerReplacementDistance = value; }
        }
        private int _headerReplacementDistance = 5;

        public CopyrightWriterFileType ForceCopyrightWriterFileType
        {
            get { return _forceCopyrightWriterFileType; }
            set { _forceCopyrightWriterFileType = value; }
        }
        private CopyrightWriterFileType _forceCopyrightWriterFileType = CopyrightWriterFileType.Default;

        public override bool Execute()
        {
            // TODO: Define a mechanism for accepting parameters for text replacement.
            var finalCopyrights = Copyrights;

            foreach (ITaskItem item in InputFiles.Where(i => i.ItemSpec.Length > 0))
            {
                CopyrightWriterFileType copyrightWriterFileType = ForceCopyrightWriterFileType;
                var fileFileName = item.ItemSpec;

                if (copyrightWriterFileType == CopyrightWriterFileType.Default)
                {
                    switch (System.IO.Path.GetExtension(fileFileName.ToLowerInvariant()))
                    {
                        case ".cs":
                            copyrightWriterFileType = CopyrightWriterFileType.CSharp;
                            break;
                        case ".xml":
                        case ".xaml":
                        case ".xsd":
                        case ".config":
                        case ".xslt":
                            copyrightWriterFileType = CopyrightWriterFileType.Xml;
                            break;
                    }
                }

                switch (copyrightWriterFileType)
                {
                    case CopyrightWriterFileType.CSharp:
                        WriteCSharpCopyright(finalCopyrights, fileFileName);
                        break;
                    case CopyrightWriterFileType.Xml:
                        WriteXmlCopyright(finalCopyrights, fileFileName);
                        break;
                }

            }
            return true;
        }

        private void WriteXmlCopyright(string finalCopyrights, string fileFileName)
        {
            System.Xml.XmlReaderSettings readerSettings = new System.Xml.XmlReaderSettings() { IgnoreWhitespace = false };
            readerSettings.IgnoreComments = false;
            var newFileContent = string.Empty;
            var oldFileContent = string.Empty;
            int distance;
            System.Text.StringBuilder sbOutput;
            var hasXmlDeclaration = false;
            using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(fileFileName, readerSettings))
            {
                System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                xmlDocument.Load(xmlReader);
                oldFileContent = xmlDocument.InnerXml;
                System.Xml.XmlComment xmlComment;

                hasXmlDeclaration = xmlDocument.ChildNodes.OfType<System.Xml.XmlNode>().Any(node => node.NodeType == System.Xml.XmlNodeType.XmlDeclaration);
                xmlComment = xmlDocument.ChildNodes.Cast<System.Xml.XmlNode>().Where(node => node.NodeType == System.Xml.XmlNodeType.Comment).Cast<System.Xml.XmlComment>().FirstOrDefault();
                if (xmlComment == null)
                {
                    xmlComment = CreateXmlComment(finalCopyrights, xmlDocument, xmlComment, hasXmlDeclaration);
                }
                else
                {
                    var xmlCommentIndex = xmlDocument.ChildNodes.OfType<System.Xml.XmlNode>().ToList().IndexOf(xmlComment);
                    if (xmlCommentIndex < 2 )
                    {
                        distance = EditDistance(xmlComment.Value, finalCopyrights);
                        if (distance < CopyrightReplacementDistance)
                        {
                            return;
                        }
                        xmlComment.Value = finalCopyrights;
                    }
                    else
                    {
                        xmlComment = CreateXmlComment(finalCopyrights, xmlDocument, xmlComment, hasXmlDeclaration);
                    }
                }

                sbOutput = new System.Text.StringBuilder();
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.OmitXmlDeclaration = !hasXmlDeclaration;
                settings.Indent = true;
                settings.IndentChars = ("\t");
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sbOutput, settings))
                {
                    xmlDocument.WriteContentTo(writer);
                }

                newFileContent = xmlDocument.InnerXml;
            }

            distance = EditDistance(oldFileContent, newFileContent);
            if (distance > HeaderReplacementDistance)
            {
                System.IO.File.WriteAllText(fileFileName, sbOutput.ToString());
            }
        }

        private static System.Xml.XmlComment CreateXmlComment(string finalCopyrights, System.Xml.XmlDocument xmlDocument, System.Xml.XmlComment xmlComment, bool hasXmlDeclaration)
        {
            xmlComment = xmlDocument.CreateComment(finalCopyrights);
            if (xmlDocument.HasChildNodes)
            {
                if (hasXmlDeclaration)
                {
                    xmlDocument.InsertAfter(xmlComment, xmlDocument.FirstChild);
                }
                else
                {
                    xmlDocument.InsertBefore(xmlComment, xmlDocument.FirstChild);
                }
            }
            else
            {
                xmlDocument.AppendChild(xmlComment);
            }
            return xmlComment;
        }

        private void WriteCSharpCopyright(string finalCopyrights, string fileFileName)
        {

            var programCode = System.IO.File.ReadAllText(fileFileName);
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(programCode);
            AstNode[] descendants;
            PreProcessorDirective region, endRegion;
            Comment comment;
            int distance;
            string newFileContent;
            string oldFileContent;

            if (SupportRegion)
            {
                descendants = syntaxTree.Descendants.Take(3).ToArray();
                region = syntaxTree.Descendants.FirstOrDefault() as PreProcessorDirective;
                comment = syntaxTree.Descendants.Skip(1).FirstOrDefault() as Comment;
                endRegion = syntaxTree.Descendants.Skip(2).FirstOrDefault() as PreProcessorDirective;

                if (region == null || region.Type != PreProcessorDirectiveType.Region
                    || comment == null
                    || endRegion == null || endRegion.Type != PreProcessorDirectiveType.Endregion)
                {

                    if (syntaxTree.FirstChild is Comment)
                    {
                        comment = syntaxTree.FirstChild as Comment;
                        distance = EditDistance(comment.Content, finalCopyrights);
                        if (distance < CopyrightReplacementDistance) // 1000> is a complete different text
                        {
                            comment.Remove();
                        }
                    }
                    syntaxTree.InsertChildBefore<PreProcessorDirective>(
                        syntaxTree.FirstChild,
                        region = new PreProcessorDirective(PreProcessorDirectiveType.Region, RegionCaption),
                        Roles.PreProcessorDirective
                    );

                    syntaxTree.InsertChildAfter<Comment>(
                        region,
                        comment = new Comment(finalCopyrights, CommentType.MultiLine),
                        Roles.Comment
                    );
                    syntaxTree.InsertChildAfter<PreProcessorDirective>(
                        comment,
                        endRegion = new PreProcessorDirective(PreProcessorDirectiveType.Endregion, RegionCaption),
                        Roles.PreProcessorDirective
                    );
                }
                else
                {
                    region.Argument = RegionCaption;
                    comment.Content = finalCopyrights;
                    endRegion.Argument = RegionCaption;
                }
                syntaxTree.InsertChildAfter<NewLineNode>(
                        endRegion,
                        new WindowsNewLine(),
                        Roles.NewLine
                        );
                newFileContent = syntaxTree.GetText();
                oldFileContent = System.IO.File.ReadAllText(fileFileName);
                distance = EditDistance(oldFileContent, newFileContent);
                if (distance > HeaderReplacementDistance)
                {
                    System.IO.File.WriteAllText(fileFileName, syntaxTree.GetText());
                }

                return;
            }

            comment = InsertOnlyComment(finalCopyrights, syntaxTree);
            syntaxTree.InsertChildAfter<NewLineNode>(
                        comment,
                        new WindowsNewLine(),
                        Roles.NewLine
                        );

            newFileContent = syntaxTree.GetText();
            oldFileContent = System.IO.File.ReadAllText(fileFileName);
            distance = EditDistance(oldFileContent, newFileContent);
            if (distance > HeaderReplacementDistance)
            {
                System.IO.File.WriteAllText(fileFileName, syntaxTree.GetText());
            }
        }

        private static Comment InsertOnlyComment(string finalCopyrights, SyntaxTree syntaxTree)
        {
            Comment comment;
            comment = syntaxTree.FirstChild as Comment;
            if (comment == null)
            {
                syntaxTree.InsertChildBefore<Comment>(
                    syntaxTree.FirstChild,
                    comment = new Comment(finalCopyrights, CommentType.MultiLine),
                    Roles.Comment
                );
            }
            else
            {
                comment.Content = finalCopyrights;
            }
            syntaxTree.InsertChildAfter<NewLineNode>(
                        comment,
                        new WindowsNewLine(),
                        Roles.NewLine
                        );
            return comment;
        }

        public static int EditDistance<T>(IEnumerable<T> x, IEnumerable<T> y)
            where T : IEquatable<T>
        {
            // Validate parameters
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");

            // Convert the parameters into IList instances
            // in order to obtain indexing capabilities
            IList<T> first = x as IList<T> ?? new List<T>(x);
            IList<T> second = y as IList<T> ?? new List<T>(y);

            // Get the length of both.  If either is 0, return
            // the length of the other, since that number of insertions
            // would be required.
            int n = first.Count, m = second.Count;
            if (n == 0) return m;
            if (m == 0) return n;

            // Rather than maintain an entire matrix (which would require O(n*m) space),
            // just store the current row and the next row, each of which has a length m+1,
            // so just O(m) space. Initialize the current row.
            int curRow = 0, nextRow = 1;
            int[][] rows = new int[][] { new int[m + 1], new int[m + 1] };
            for (int j = 0; j <= m; ++j) rows[curRow][j] = j;

            // For each virtual row (since we only have physical storage for two)
            for (int i = 1; i <= n; ++i)
            {
                // Fill in the values in the row
                rows[nextRow][0] = i;
                for (int j = 1; j <= m; ++j)
                {
                    int dist1 = rows[curRow][j] + 1;
                    int dist2 = rows[nextRow][j - 1] + 1;
                    int dist3 = rows[curRow][j - 1] +
                        (first[i - 1].Equals(second[j - 1]) ? 0 : 1);

                    rows[nextRow][j] = Math.Min(dist1, Math.Min(dist2, dist3));
                }

                // Swap the current and next rows
                if (curRow == 0)
                {
                    curRow = 1;
                    nextRow = 0;
                }
                else
                {
                    curRow = 0;
                    nextRow = 1;
                }
            }

            // Return the computed edit distance
            return rows[curRow][m];
        }
    }
}
