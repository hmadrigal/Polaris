//-----------------------------------------------------------------------
// <copyright file="FormattedTextBlock.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    [Localizability(LocalizationCategory.Text)]
    [DisplayName("Formatted Text Block")]
    [Description("Displays a text allowing to change the font")]
    public class FormattedTextBlock : Control
    {
        protected char[] UNAVAILABLE_GLYPHS = new char[] { '\n', '\r' };

        private FontFormat DefaultFontFormat { get; set; }

        ///private TagCollection resourcedTags;

        ///<summary>
        /// Summary:
        ///     Gets or sets the height of each line of content.
        ///
        /// Returns:
        ///     The height of line, in device independent pixels, in the range of 0.0034
        ///     to 160000. A value of System.Double.NaN (equivalent to an attribute value
        ///     of "Auto") indicates that the line height is determined automatically from
        ///     the current font characteristics. The default is System.Double.NaN.
        ///
        /// Exceptions:
        ///   System.ArgumentException:
        ///     System.Windows.Controls.TextBlock.LineHeight is set to a non-positive value.
        ///</summary>
        [TypeConverter(typeof(LengthConverter))]
        [Localizability(LocalizationCategory.None)]
        [Bindable(true)]
        [Description("Defines in pixels the space between lines.")]
        [Category("Formatted Text Block")]
        [DisplayName("Line Height")]

        #region LineHeight

        public double LineHeight
        {
            get { return (double)GetValue(LineHeightProperty); }
            set { SetValue(LineHeightProperty, value); }
        }

        /// <summary>
        /// LineHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty LineHeightProperty =
            DependencyProperty.Register("LineHeight", typeof(double), typeof(FormattedTextBlock),
                new FrameworkPropertyMetadata(System.Windows.SystemFonts.MessageFontFamily.LineSpacing,
                    new PropertyChangedCallback(OnLineHeightChanged)));

        /// <summary>
        /// Handles changes to the LineHeight property.
        /// </summary>
        private static void OnLineHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FormattedTextBlock)d).OnLineHeightChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the LineHeight property.
        /// </summary>
        protected virtual void OnLineHeightChanged(DependencyPropertyChangedEventArgs e)
        {
            TryInvalidateDisplay();
        }

        #endregion LineHeight

        /// <summary>
        /// Collection of fonts to be applied when counting lines or words.
        /// </summary>
        [Description("Collection of formats to be applied when counting lines or words.")]
        [Category("Formatted Text Block")]
        [DisplayName("Count Font Formats")]

        #region FontFormats

        public CountFontFormatCollection FontFormats
        {
            get { return (CountFontFormatCollection)GetValue(FontFormatsProperty); }
            set { SetValue(FontFormatsProperty, value); }
        }

        /// <summary>
        /// FontFormats Dependency Property
        /// </summary>
        public static readonly DependencyProperty FontFormatsProperty =
            DependencyProperty.Register("FontFormats", typeof(CountFontFormatCollection), typeof(FormattedTextBlock),
                new FrameworkPropertyMetadata(new CountFontFormatCollection(),
                    new PropertyChangedCallback(OnFontFormatsChanged)));

        /// <summary>
        /// Handles changes to the FontFormats property.
        /// </summary>
        private static void OnFontFormatsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FormattedTextBlock)d).OnFontFormatsChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the FontFormats property.
        /// </summary>
        protected virtual void OnFontFormatsChanged(DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as CountFontFormatCollection;
            var oldValue = e.NewValue as CountFontFormatCollection;
            if (newValue != null) { newValue.formattedTextBlockRef = this; }
            if (oldValue != null) { oldValue.formattedTextBlockRef = null; }
            TryInvalidateDisplay();
        }

        #endregion FontFormats

        /// <summary>
        /// Collection of fonts to be applied when text is wrapped in a specified tag.
        /// </summary>
        [Description("Collection of formats to be applied when fetching tags.")]
        [Category("Formatted Text Block")]
        [DisplayName("Tagged Font Formats")]

        #region TaggedFontFormats

        public TaggedFontFormatCollection TaggedFontFormats
        {
            get { return (TaggedFontFormatCollection)GetValue(TaggedFontFormatsProperty); }
            set { SetValue(TaggedFontFormatsProperty, value); }
        }

        /// <summary>
        /// TaggedFontFormats Dependency Property
        /// </summary>
        public static readonly DependencyProperty TaggedFontFormatsProperty =
            DependencyProperty.Register("TaggedFontFormats", typeof(TaggedFontFormatCollection), typeof(FormattedTextBlock),
                new FrameworkPropertyMetadata(new TaggedFontFormatCollection(),
                    new PropertyChangedCallback(OnTaggedFontFormatsChanged)));

        /// <summary>
        /// Handles changes to the TaggedFontFormats property.
        /// </summary>
        private static void OnTaggedFontFormatsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FormattedTextBlock)d).OnTaggedFontFormatsChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TaggedFontFormats property.
        /// </summary>
        protected virtual void OnTaggedFontFormatsChanged(DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as TaggedFontFormatCollection;
            var oldValue = e.NewValue as TaggedFontFormatCollection;
            if (oldValue != null) { oldValue.formattedTextBlockRef = null; }
            if (newValue != null) { newValue.formattedTextBlockRef = this; }
            TryInvalidateDisplay();
        }

        #endregion TaggedFontFormats

        [Localizability(LocalizationCategory.Text)]
        [DefaultValue(null)]
        [Category("Common Properties")]
        [Bindable(true)]

        #region Text

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Text Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(FormattedTextBlock),
                new FrameworkPropertyMetadata(string.Empty,
                    new PropertyChangedCallback(OnTextChanged)));

        /// <summary>
        /// Handles changes to the Text property.
        /// </summary>
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FormattedTextBlock)d).OnTextChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Text property.
        /// </summary>
        protected virtual void OnTextChanged(DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as string;
            TryInvalidateDisplay();
        }

        #endregion Text

        /// <summary>
        /// Stores the font Formats for render
        /// </summary>

        static FormattedTextBlock()
        {
            var refTextBlock = new TextBlock();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FormattedTextBlock), new FrameworkPropertyMetadata(typeof(FormattedTextBlock)));
        }

        public FormattedTextBlock()
        {
            #region Hookup to DependencyPropertyDescriptor

            // NOTE: Detects Changes on default properties and recreates the default font.
            // Control.FontFamilyProperty
            // Control.FontSizeProperty
            // Control.FontStretchProperty
            // Control.FontStyleProperty
            // Control.FontWeightProperty
            // Control.ForegroundProperty
            // Control.StyleProperty

            // NOTE: Adds an event handler when the dependency property 'Control.FontFamilyProperty' changes
            DependencyPropertyDescriptor FontFamilyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(Control.FontFamilyProperty, typeof(FormattedTextBlock));
            if (FontFamilyPropertyDescriptor != null)
            {
                FontFamilyPropertyDescriptor.AddValueChanged(this, OnFontFamilyPropertyChanged);
            }
            // NOTE: Adds an event handler when the dependency property 'Control.FontSizeProperty' changes
            DependencyPropertyDescriptor FontSizePropertyDescriptor = DependencyPropertyDescriptor.FromProperty(Control.FontSizeProperty, typeof(FormattedTextBlock));
            if (FontSizePropertyDescriptor != null)
            {
                FontSizePropertyDescriptor.AddValueChanged(this, OnFontSizePropertyChanged);
            }
            // NOTE: Adds an event handler when the dependency property 'Control.FontStretchProperty' changes
            DependencyPropertyDescriptor FontStretchPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(Control.FontStretchProperty, typeof(FormattedTextBlock));
            if (FontStretchPropertyDescriptor != null)
            {
                FontStretchPropertyDescriptor.AddValueChanged(this, OnFontStretchPropertyChanged);
            }
            // NOTE: Adds an event handler when the dependency property 'Control.FontStyleProperty' changes
            DependencyPropertyDescriptor FontStylePropertyDescriptor = DependencyPropertyDescriptor.FromProperty(Control.FontStyleProperty, typeof(FormattedTextBlock));
            if (FontStylePropertyDescriptor != null)
            {
                FontStylePropertyDescriptor.AddValueChanged(this, OnFontStylePropertyChanged);
            }
            // NOTE: Adds an event handler when the dependency property 'Control.FontWeightProperty' changes
            DependencyPropertyDescriptor FontWeightPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(Control.FontWeightProperty, typeof(FormattedTextBlock));
            if (FontWeightPropertyDescriptor != null)
            {
                FontWeightPropertyDescriptor.AddValueChanged(this, OnFontWeightPropertyChanged);
            }
            // NOTE: Adds an event handler when the dependency property 'Control.ForegroundProperty' changes
            DependencyPropertyDescriptor ForegroundPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(FormattedTextBlock));
            if (ForegroundPropertyDescriptor != null)
            {
                ForegroundPropertyDescriptor.AddValueChanged(this, OnForegroundPropertyChanged);
            }
            // NOTE: Adds an event handler when the dependency property 'Control.StyleProperty' changes
            DependencyPropertyDescriptor StylePropertyDescriptor = DependencyPropertyDescriptor.FromProperty(Control.StyleProperty, typeof(FormattedTextBlock));
            if (StylePropertyDescriptor != null)
            {
                StylePropertyDescriptor.AddValueChanged(this, OnStylePropertyChanged);
            }

            // NOTE: Adds an event handler when the dependency property 'Control.BackgroundProperty' changes
            DependencyPropertyDescriptor BackgroundPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(Control.BackgroundProperty, typeof(FormattedTextBlock));
            if (BackgroundPropertyDescriptor != null)
            {
                BackgroundPropertyDescriptor.AddValueChanged(this, OnBackgroundPropertyChanged);
            }

            #endregion Hookup to DependencyPropertyDescriptor

            FontFormats = new CountFontFormatCollection();
            FontFormats.formattedTextBlockRef = this;

            TaggedFontFormats = new TaggedFontFormatCollection();
            TaggedFontFormats.formattedTextBlockRef = this;
        }

        public override void EndInit()
        {
            base.EndInit();
            InitializeDefaultFontFormat();
        }

        //public override void EndInit()
        //{
        //    base.EndInit();
        //    InitializeResources();
        //}

        //private void InitializeResources()
        //{
        //    resourcedTags = new TagCollection();
        //    foreach (TaggedFontFormat item in TaggedFontFormats)
        //    {
        //        //r = item as FontFormat;
        //        if (item == null) continue;
        //        resourcedTags.Add(item.Tag);
        //        item.PrepareTypeface(this);
        //    }
        //}

        private void InitializeDefaultFontFormat()
        {
            DefaultFontFormat = new FontFormat()
            {
                FontFamily = FontFamily,
                FontSize = FontSize,
                FontStretch = FontStretch,
                FontStyle = FontStyle,
                FontWeight = FontWeight,
                Foreground = Foreground,
                Style = Style,
            };
            DefaultFontFormat.PrepareTypeface(this);
            TryInvalidateDisplay();
        }

        private void OnFontFamilyPropertyChanged(object sender, EventArgs e)
        { InitializeDefaultFontFormat(); }

        private void OnFontSizePropertyChanged(object sender, EventArgs e)
        { InitializeDefaultFontFormat(); }

        private void OnFontStretchPropertyChanged(object sender, EventArgs e)
        { InitializeDefaultFontFormat(); }

        private void OnFontStylePropertyChanged(object sender, EventArgs e)
        { InitializeDefaultFontFormat(); }

        private void OnFontWeightPropertyChanged(object sender, EventArgs e)
        { InitializeDefaultFontFormat(); }

        private void OnForegroundPropertyChanged(object sender, EventArgs e)
        { InitializeDefaultFontFormat(); }

        private void OnStylePropertyChanged(object sender, EventArgs e)
        { InitializeDefaultFontFormat(); }

        private void OnBackgroundPropertyChanged(object sender, EventArgs e)
        {
            TryInvalidateDisplay();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            RenderByWord(RenderSize, drawingContext);
        }

        private IEnumerable<Word> GetWords(string displayText)
        {
            if (string.IsNullOrEmpty(displayText))
            {
                yield break;
            }
            var previousTokenType = TokenType.Unknown;
            var currentTokenType = TokenType.Unknown;

            var separators = new string[] { " " };
            var tagBegins = TaggedFontFormats.Select(f => string.Format("<{0}>", f.Tag)).ToArray(); //resourcedTags.Select(i => string.Format("<{0}>", i)).ToArray();
            var tagEnds = TaggedFontFormats.Select(f => string.Format("</{0}>", f.Tag)).ToArray();//resourcedTags.Select(i => string.Format("</{0}>", i)).ToArray();

            var currentIndex = 0;
            var currentWord = new StringBuilder();
            var currentSeparator = new StringBuilder();
            var currentTagBegin = new StringBuilder();
            var currentTagEnd = new StringBuilder();
            var currentNewLineFound = false;
            var wordFound = true;

            #region GetNewWord Inline Method

            Func<Word> getNewWord = () =>
            {
                var resultWord = new Word
                {
                    Text = currentWord.ToString(),
                    Separator = currentSeparator.ToString(),
                    TagBegin = currentTagBegin.ToString(),
                    TagEnd = currentTagEnd.ToString(),
                    NewLineFound = currentNewLineFound,
                };
                currentWord.Clear();
                currentSeparator.Clear();
                currentTagBegin.Clear();
                currentTagEnd.Clear();
                currentNewLineFound = false;
                previousTokenType = TokenType.Word;
                return resultWord;
            };

            #endregion GetNewWord Inline Method

            while (currentIndex < displayText.Length)
            {
                wordFound = true;

                if (currentIndex + Environment.NewLine.Length <= displayText.Length && displayText.Substring(currentIndex, Environment.NewLine.Length) == Environment.NewLine)
                {
                    currentNewLineFound = true;
                    yield return getNewWord();
                    currentIndex += Environment.NewLine.Length;
                    continue;
                    //previousTokenType = currentTokenType;
                    //currentTokenType = TokenType.NewLine;
                    //wordFound = false;
                }

                foreach (var tagBegin in tagBegins)
                {
                    if (currentIndex + tagBegin.Length <= displayText.Length && displayText.Substring(currentIndex, tagBegin.Length) == tagBegin)
                    {
                        yield return getNewWord();
                        currentTagBegin.Append(tagBegin);
                        currentIndex += tagBegin.Length;
                        previousTokenType = currentTokenType;
                        currentTokenType = TokenType.TagBegin;
                        wordFound = false;
                    }
                }

                foreach (var tagEnd in tagEnds)
                {
                    if (currentIndex + tagEnd.Length <= displayText.Length && displayText.Substring(currentIndex, tagEnd.Length) == tagEnd)
                    {
                        currentTagEnd.Append(tagEnd);
                        currentIndex += tagEnd.Length;
                        previousTokenType = currentTokenType;
                        currentTokenType = TokenType.TagEnd;
                        wordFound = false;
                    }
                }

                foreach (var separator in separators)
                {
                    if (currentIndex + separator.Length <= displayText.Length && displayText.Substring(currentIndex, separator.Length) == separator)
                    {
                        currentSeparator.Append(separator);
                        currentIndex += separator.Length;
                        previousTokenType = currentTokenType;
                        currentTokenType = TokenType.Separator;
                        wordFound = false;
                    }
                }

                if (wordFound)
                {
                    previousTokenType = currentTokenType;
                    currentTokenType = TokenType.Word;
                }

                if ((currentTokenType == TokenType.Word) && (previousTokenType == TokenType.Separator || previousTokenType == TokenType.TagEnd))
                {
                    yield return getNewWord();
                }
                else if (currentTokenType == TokenType.Word)
                {
                    currentWord.Append(displayText[currentIndex]);
                    currentIndex++;
                }
            }

            if (currentWord.Length > 0)
            {
                yield return getNewWord();
            }
        }

        private Size RenderByWord(Size renderSize, DrawingContext drawingContext = null)
        {
            Size renderedSize = new Size(0, 0);
            if (string.IsNullOrWhiteSpace(Text)) return renderedSize;
            Brush background = Brushes.Transparent;
            Brush borderBrush = Brushes.Transparent;
            if (Background != null) { background = Background; }
            if (BorderBrush != null) { borderBrush = BorderBrush; }

            if (drawingContext != null)
            {
                drawingContext.DrawRectangle(background, new Pen() { Brush = borderBrush }, new Rect(RenderSize));
            }
            var defaultFontFormat = DefaultFontFormat;
            var flaggedFontFormat = DefaultFontFormat;// default(FontFormat); //FlaggedFontFormat;
            var previousFontFormat = defaultFontFormat;
            var fontFormats = FontFormats;

            var currentFontFormatIndex = default(int?);

            if (fontFormats.Count > 0)
            {
                currentFontFormatIndex = 0;
            }
            string displayText = Text.Replace("\t", "    ");
            Word[] words = GetWords(displayText).ToArray();

            var currentWordIndex = 0;
            var currentWord = default(string);
            var remainingWordCharacters = default(string);
            double renderingXPosition = 0d;
            int lineCount = 1;
            //var fontSize = defaultFontFormat.FontSize;
            double renderingYPosition = default(double);
            var isNewLineRequired = false;
            var currentFontFormat = DefaultFontFormat;
            var remainingWordCount = default(int?);
            var remainingLineCount = default(int?);
            bool isFlagged = false;
            double wordHeight = 0d;
            double majorLineHeight = 0d;

            #region GetNextFontFormat Inline Method

            Action getNextFontFormat = () =>
            {
                if (currentFontFormatIndex != null)
                {
                    if (currentFontFormatIndex.Value == fontFormats.Count)
                    {
                        if (!isFlagged)
                        {
                            currentFontFormat = DefaultFontFormat;
                        }
                        else
                        {
                            previousFontFormat = DefaultFontFormat;
                        }
                    }
                    else
                    {
                        if (!isFlagged)
                        {
                            currentFontFormat = fontFormats[currentFontFormatIndex.Value];
                        }
                        else
                        {
                            previousFontFormat = fontFormats[currentFontFormatIndex.Value];
                        }
                    }
                    //fontSize = currentFontFormat.FontSize;
                    if (currentFontFormat is CountFontFormat)
                    {
                        var currentCountFontFormat = currentFontFormat as CountFontFormat;

                        if (!isFlagged)
                        {
                            if (currentCountFontFormat.Type == FontFormatType.Word)
                            {
                                remainingWordCount = currentCountFontFormat.Count;
                            }
                            else
                            {
                                remainingLineCount = currentCountFontFormat.Count;
                            }
                        }
                        else
                        {
                            if (currentCountFontFormat.Type == FontFormatType.Word)
                            {
                                remainingWordCount = currentCountFontFormat.Count;
                            }
                            else
                            {
                                remainingLineCount = currentCountFontFormat.Count;
                            }
                        }
                    }
                }
            };

            #endregion GetNextFontFormat Inline Method

            getNextFontFormat();
            // Note: This must happen after the current font format is determined.
            // Note: In this case, order of execution matters.
            renderingYPosition += (LineHeight + currentFontFormat.FontSize);

            while (currentWordIndex < words.Length)
            {
                currentWord = string.Concat(words[currentWordIndex].Text, words[currentWordIndex].Separator);
                if (!string.IsNullOrEmpty(remainingWordCharacters))
                {
                    currentWord = currentWord.Substring(currentWord.Length - remainingWordCharacters.Length, remainingWordCharacters.Length);
                    remainingWordCharacters = string.Empty;
                }

                //var separator = words[currentWordIndex].Separator;
                var tagBegin = words[currentWordIndex].TagBegin;
                var tagEnd = words[currentWordIndex].TagEnd;
                double wordRenderedWidth;
                bool isWordRendered;

                if (!string.IsNullOrWhiteSpace(tagBegin))
                {
                    flaggedFontFormat = GetFontFormatForTag(tagBegin);
                }
                if (!string.IsNullOrWhiteSpace(tagBegin) && flaggedFontFormat != null && !isFlagged)
                {
                    previousFontFormat = currentFontFormat;
                    currentFontFormat = flaggedFontFormat;
                    isFlagged = true;
                }
                if (renderingYPosition > renderSize.Height) { break; }
                RenderWordRequest(renderSize, currentWord, currentFontFormat, renderingXPosition, renderingYPosition, out remainingWordCharacters, out wordRenderedWidth, out wordHeight, out isWordRendered, drawingContext);
                majorLineHeight = Math.Max(wordHeight, majorLineHeight);

                if (isWordRendered)
                {
                    if (!string.IsNullOrWhiteSpace(tagEnd) && flaggedFontFormat != null)
                    {
                        currentFontFormat = previousFontFormat;
                        previousFontFormat = defaultFontFormat;
                        isFlagged = false;
                    }

                    if (remainingWordCount != null)
                    {
                        remainingWordCount--;
                        if (remainingWordCount.Value == 0)
                        {
                            remainingWordCount = null;
                            currentFontFormatIndex++;
                            getNextFontFormat();
                        }
                    }
                    renderingXPosition += wordRenderedWidth;
                    renderedSize.Width = Math.Max(renderedSize.Width, renderingXPosition + 1);
                    isNewLineRequired |= words[currentWordIndex].NewLineFound;
                    currentWordIndex++;
                }
                else
                {
                    isNewLineRequired = true;
                }

                if (currentWordIndex >= words.Length) { break; }

                if (isNewLineRequired)
                {
                    lineCount++;
                    renderingXPosition = 0d;
                    if (remainingLineCount != null)
                    {
                        remainingLineCount--;
                        if (remainingLineCount.Value == 0)
                        {
                            remainingLineCount = null;
                            currentFontFormatIndex++;
                            getNextFontFormat();
                        }
                    }
                    renderingYPosition += (LineHeight + currentFontFormat.FontSize);
                    majorLineHeight = 0;
                    wordHeight = 0;
                    isNewLineRequired = false;
                }

                //if (renderingYPosition > renderSize.Height) { break; }
            }
            renderedSize.Height = renderingYPosition;// +majorLineHeight;
            return renderedSize;
        }

        private FontFormat GetFontFormatForTag(string tag)
        {
            var foundFontFormat = DefaultFontFormat;
            if (string.IsNullOrWhiteSpace(tag)) return DefaultFontFormat;

            foundFontFormat = TaggedFontFormats.FirstOrDefault(s => s.Tag == tag.Substring(1, tag.Length - 2)) as TaggedFontFormat;
            //TryFindResource(tag.Substring(1, tag.Length - 2)) as TaggedFontFormat;

            return foundFontFormat == null ? DefaultFontFormat : foundFontFormat;
        }

        private void RenderWordRequest(Size renderSize, string currentWord, FontFormat fontFormat, double renderingXPosition, double renderingYPosition, out string remainingWordCharacters, out double wordWidth, out double wordHeight, out bool isRendered, DrawingContext drawingContext = null)
        {
            isRendered = false;
            wordHeight = 0;
            remainingWordCharacters = string.Empty;
            if (string.IsNullOrEmpty(currentWord))
            {
                wordWidth = 0;
                isRendered = true;
                return;
            }

            var drawingText = currentWord;

            var currentCharIndex = 0;
            var currentChar = drawingText[currentCharIndex];

            var glyphIndexes = new List<ushort>();
            var advanceWidths = new List<double>();

            //var currentTypeface = fontFormat.typeface;
            var currentGlyphTypeface = fontFormat.glyphTypeface;

            var currentLineWidth = renderingXPosition;
            wordWidth = 0d;

            while (currentCharIndex < drawingText.Length)
            {
                currentChar = drawingText[currentCharIndex];
                var isHorizontalSpaceAvailable = true;
                if (!UNAVAILABLE_GLYPHS.Contains(currentChar))
                {
                    if (!currentGlyphTypeface.CharacterToGlyphMap.ContainsKey(currentChar))
                    {
                        currentChar = '_';
                    }
                    var currentCharGlyphIndex = currentGlyphTypeface.CharacterToGlyphMap[currentChar];
                    var currentCharWidth = currentGlyphTypeface.AdvanceWidths[currentCharGlyphIndex] * fontFormat.FontSize;
                    wordWidth += currentCharWidth;
                    currentLineWidth += currentCharWidth;
                    wordHeight = Math.Max(wordHeight, currentGlyphTypeface.AdvanceHeights[currentCharGlyphIndex] * fontFormat.FontSize);

                    isHorizontalSpaceAvailable = currentLineWidth < renderSize.Width;
                    if (currentCharIndex != 0 && renderingXPosition == 0 && !isHorizontalSpaceAvailable)
                    {
                        wordWidth -= currentCharWidth;
                        currentLineWidth -= currentCharWidth;
                        remainingWordCharacters = currentWord.Substring(currentCharIndex, currentWord.Length - currentCharIndex);
                        isRendered = false;
                        break;
                    }
                    if (!isHorizontalSpaceAvailable)
                    {
                        isRendered = false;
                        return;
                    }
                    glyphIndexes.Add(currentCharGlyphIndex);
                    advanceWidths.Add(currentCharWidth);
                }

                currentCharIndex++;
            }

            if (drawingContext != null)
            {
                var origin = new Point(renderingXPosition, renderingYPosition + fontFormat.LineHeight.Value);

                var glyphRun = new GlyphRun(currentGlyphTypeface, 0, false, fontFormat.FontSize,
                    glyphIndexes, origin, advanceWidths, null, null, null, null,
                    null, null);
                // Draws the current line
                drawingContext.DrawGlyphRun(fontFormat.Foreground == null ? DefaultFontFormat.Foreground : fontFormat.Foreground, glyphRun);
            }
            if (string.IsNullOrEmpty(remainingWordCharacters))
            {
                isRendered = true;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            //return base.MeasureOverride(constraint);
            var desiredSize = RenderByWord(constraint);
            //desiredSize.Height += 10;
            return desiredSize;
        }

        #region TokenType

        private enum TokenType
        {
            Unknown,
            Word,
            Separator,
            TagBegin,
            TagEnd,
            NewLine,
        }

        #endregion TokenType

        #region Word

        private class Word
        {
            public string Text { get; set; }

            public string Separator { get; set; }

            public string TagBegin { get; set; }

            public string TagEnd { get; set; }

            public bool NewLineFound { get; set; }
        }

        #endregion Word

        internal void TryInvalidateDisplay()
        {
            InvalidateMeasure();
            InvalidateVisual();
        }

        protected override void OnStyleChanged(Style oldStyle, Style newStyle)
        {
            base.OnStyleChanged(oldStyle, newStyle);
            if (newStyle == null) return;
            newStyle.Setters.OfType<Setter>().Select(s =>
            {
                SetValue(s.Property, s.Value); return s;
            }).Any();
        }
    }
}
