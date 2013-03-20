using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Refactoring;
using ICSharpCode.NRefactory.Editor;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Polaris.MSBuild.Common
{
    public class AssemblyInfoWriterTask : Task
    {
        public string AssemblyInfoFilePath
        {
            get { return _assemblyInfoFilePath; }
            set { _assemblyInfoFilePath = value; }
        }
        private string _assemblyInfoFilePath = @"Properties\AssemblyInfo.cs";

        protected readonly Regex AssemblyVersionRegEx = new Regex(@"""(?<Major>\d+)\.(?<Minor>\d+)(?:\.*)(?<Build>\*|(\d*))(?:\.*)(?<Revision>\*|(\d*))""", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        //[assembly: AssemblyTitle("FooLibrary.MsBuild")]
        public string AssemblyTitle { get; set; }
        private const string AssemblyTitleAttributeName = @"AssemblyTitle";

        //[assembly: AssemblyDescription("")]
        public string AssemblyDescription { get; set; }
        private const string AssemblyDescriptionAttributeName = @"AssemblyDescription";

        //[assembly: AssemblyConfiguration("")]
        public string AssemblyConfiguration { get; set; }
        private const string AssemblyConfigurationAttributeName = @"AssemblyConfiguration";

        //[assembly: AssemblyCompany("")]
        public string AssemblyCompany { get; set; }
        private const string AssemblyCompanyAttributeName = @"AssemblyCompany";

        //[assembly: AssemblyProduct("FooLibrary.MsBuild")]
        public string AssemblyProduct { get; set; }
        private const string AssemblyProductAttributeName = @"AssemblyProduct";

        //[assembly: AssemblyCopyright("Copyright ©  2013")]
        public string AssemblyCopyright { get; set; }
        private const string AssemblyCopyrightAttributeName = @"AssemblyCopyright";

        //[assembly: AssemblyTrademark("")]
        public string AssemblyTrademark { get; set; }
        private const string AssemblyTrademarkAttributeName = @"AssemblyTrademark";

        //[assembly: AssemblyCulture("")]
        public string AssemblyCulture { get; set; }
        private const string AssemblyCultureAttributeName = @"AssemblyCulture";

        //[assembly: AssemblyInformationalVersion("")]
        public string AssemblyInformationalVersion { get; set; }
        private const string AssemblyInformationalVersionAttributeName = @"AssemblyInformationalVersion";

        //[assembly: AssemblyVersion("1.0.0.0")]
        private const string AssemblyVersionAttributeName = @"AssemblyVersion";

        //[assembly: AssemblyFileVersion("1.0.0.0")]
        public string AssemblyFileVersion { get; set; }
        private const string AssemblyFileVersionAttributeName = @"AssemblyFileVersion";

        public override bool Execute()
        {
            if (!System.IO.File.Exists(AssemblyInfoFilePath))
            {
                Log.LogError(@"File '{0}' not found ", AssemblyInfoFilePath);
                return false;
            }

            var programCode = System.IO.File.ReadAllText(AssemblyInfoFilePath);
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(programCode);

            var fileOriginalText = programCode;
            var fileIndexOfInvocations = syntaxTree.Descendants.OfType<ICSharpCode.NRefactory.CSharp.Attribute>().Where(att => att.HasArgumentList).ToList();
            var fileFileName = AssemblyInfoFilePath;
            var document = new StringBuilderDocument(fileOriginalText);
            var formattingOptions = FormattingOptionsFactory.CreateAllman();
            var options = new TextEditorOptions();
            using (var script = new DocumentScript(document, formattingOptions, options))
            {
                foreach (var attributeToken in fileIndexOfInvocations)
                {

                    var oldPrimitiveExpression = attributeToken.Arguments.FirstOrDefault();
                    var newPrimitiveExpression = (PrimitiveExpression)oldPrimitiveExpression.Clone();

                    switch (attributeToken.Type.GetText())
                    {
                        case AssemblyTitleAttributeName:
                            newPrimitiveExpression.LiteralValue = AssemblyTitle ?? newPrimitiveExpression.LiteralValue;
                            break;
                        case AssemblyDescriptionAttributeName:
                            newPrimitiveExpression.LiteralValue = AssemblyDescription ?? newPrimitiveExpression.LiteralValue;
                            break;
                        case AssemblyConfigurationAttributeName:
                            newPrimitiveExpression.LiteralValue = AssemblyConfiguration ?? newPrimitiveExpression.LiteralValue;
                            break;
                        case AssemblyCompanyAttributeName:
                            newPrimitiveExpression.LiteralValue = AssemblyCompany ?? newPrimitiveExpression.LiteralValue;
                            break;
                        case AssemblyProductAttributeName:
                            newPrimitiveExpression.LiteralValue = AssemblyProduct ?? newPrimitiveExpression.LiteralValue;
                            break;
                        case AssemblyCopyrightAttributeName:
                            newPrimitiveExpression.LiteralValue = AssemblyCopyright ?? newPrimitiveExpression.LiteralValue;
                            break;
                        case AssemblyTrademarkAttributeName:
                            newPrimitiveExpression.LiteralValue = AssemblyTrademark ?? newPrimitiveExpression.LiteralValue;
                            break;
                        case AssemblyCultureAttributeName:
                            newPrimitiveExpression.LiteralValue = AssemblyCulture ?? newPrimitiveExpression.LiteralValue;
                            break;
                        case AssemblyInformationalVersionAttributeName:
                            newPrimitiveExpression.LiteralValue = AssemblyInformationalVersion ?? newPrimitiveExpression.LiteralValue;
                            break;
                        case AssemblyFileVersionAttributeName:
                            newPrimitiveExpression.LiteralValue = AssemblyFileVersion ?? newPrimitiveExpression.LiteralValue;
                            break;
                        case AssemblyVersionAttributeName:
                            // tries to identify the parts of the old version
                            var versionMatches = AssemblyVersionRegEx.Matches(newPrimitiveExpression.LiteralValue);
                            if (versionMatches.Count == 0)
                            {
                                Log.LogMessage(MessageImportance.High, "Task could not analyze version '{0}'.", newPrimitiveExpression.LiteralValue);
                                continue;
                            }
                            var major = versionMatches[0].Groups["Major"].Value;
                            var minor = versionMatches[0].Groups["Minor"].Value;
                            var build = versionMatches[0].Groups["Build"].Value;
                            var revision = versionMatches[0].Groups["Revision"].Value;

                            //TODO: Provide mechanism for customizing build/revision computation. This is only one of the tons of possible computations
                            // Computing the build number based on how much of the year has lapsed
                            #region Computing new Build and Revision
                            var utcNow = DateTime.UtcNow;
                            var endOfThisYear = new DateTime(utcNow.Year, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc);
                            var beginOfThisYear = new DateTime(utcNow.Year, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                            var totalOfMillisecondOfThisYear = (endOfThisYear - beginOfThisYear).TotalMilliseconds;
                            var totalOfMillisecondUntilNow = (utcNow - beginOfThisYear).TotalMilliseconds;
                            ushort newBuildNumber = Convert.ToUInt16((totalOfMillisecondUntilNow / totalOfMillisecondOfThisYear) * ushort.MaxValue);
                            // Computing the revision number based on how much of the day has lapsed
                            double totalOfMillisecondsOfADay = 86400000L;
                            double totalOfMillisecondsOnlyToday = (utcNow - utcNow.Date).TotalMilliseconds;
                            ushort newRevisionNumber = Convert.ToUInt16((totalOfMillisecondsOnlyToday / totalOfMillisecondsOfADay) * ushort.MaxValue);
                            #endregion

                            // Generates the new version based on the old version and the computed version
                            var newVersion = string.Format("\"{0}.{1}{2}{3}\"",
                                major,
                                minor,
                                string.IsNullOrEmpty(build) ? build : string.Concat(".", "*" == build ? build : newBuildNumber.ToString()),
                                string.IsNullOrEmpty(revision) ? revision : string.Concat(".", "*" == revision ? revision : newRevisionNumber.ToString())
                                );
                            newPrimitiveExpression.LiteralValue = newVersion;
                            break;

                        default:
                            continue;
                    }
                    script.Replace(oldPrimitiveExpression, newPrimitiveExpression);

                }
            }
            System.IO.File.WriteAllText(fileFileName, document.Text);

            return true;
        }
    }
}
