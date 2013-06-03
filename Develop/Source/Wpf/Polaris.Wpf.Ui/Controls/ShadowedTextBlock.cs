using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;

namespace Polaris.Windows.Controls
{
    [Localizability(LocalizationCategory.Text)]
    [DisplayName("Shadow Textblock")]
    [Description("Displays a text and draws drop shadow with it.")]
    public class ShadowedTextBlock : Control
    {
        internal Typeface MainTypeface;
        internal GlyphTypeface MainGlyphTypeface;
        internal Typeface ShadowTypeface;
        internal GlyphTypeface ShadowGlyphTypeface;

        protected char[] UNAVAILABLE_GLYPHS = new[] { '\n', '\r' };

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
        [Category("Appearance")]
        [Description("Gets or sets the height of each line of content.")]
        [DisplayName("Line Height")]
        #region LineHeight
        public double? LineHeight
        {
            get
            {
                var value = (double?)GetValue(LineHeightProperty);
                return value ?? (FontFamily ?? SystemFonts.MessageFontFamily).LineSpacing;
            }
            set { SetValue(LineHeightProperty, value); }
        }

        /// <summary>
        /// LineHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty LineHeightProperty =
            DependencyProperty.Register("LineHeight", typeof(double?), typeof(FontFormat),
                new FrameworkPropertyMetadata(null,
                    OnLineHeightChanged));


        /// <summary>
        /// Handles changes to the LineHeight property.
        /// </summary>
        private static void OnLineHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShadowedTextBlock)d).OnLineHeightChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the LineHeight property.
        /// </summary>
        protected virtual void OnLineHeightChanged(DependencyPropertyChangedEventArgs e)
        {
            TryInvalidateDisplay();
        }

        #endregion

        [Localizability(LocalizationCategory.Text)]
        [DefaultValue(null)]
        [Category("Common Properties")]
        [Description("Text displayed in front of the canvas.")]
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
            DependencyProperty.Register("Text", typeof(string), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(string.Empty,
                    OnTextChanged));

        /// <summary>
        /// Handles changes to the Text property.
        /// </summary>
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShadowedTextBlock)d).OnTextChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Text property.
        /// </summary>
        protected virtual void OnTextChanged(DependencyPropertyChangedEventArgs e)
        {
            TryInvalidateDisplay();
        }

        #endregion Text

        ///<summary>
        /// Summary:
        ///     Gets or sets a brush that describes the ShadowColor color.
        ///
        /// Returns:
        ///     The brush that paints the ShadowColor of the control. The default value is
        ///     the system dialog font color.
        ///</summary>
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Gets or sets a brush that describes the ShadowColor color.")]
        #region ShadowColor
        public Brush ShadowColor
        {
            get
            {
                var value = (Brush)GetValue(ShadowColorProperty);
                return value ?? (SystemColors.ControlTextBrush);
            }
            set { SetValue(ShadowColorProperty, value); }
        }
        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register("ShadowColor", typeof(Brush), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(null,
                    OnShadowColorChanged));

        /// <summary>
        /// Handles changes to the ShadowColor property.
        /// </summary>
        private static void OnShadowColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShadowedTextBlock)d).OnShadowColorChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ShadowColor property.
        /// </summary>
        protected virtual void OnShadowColorChanged(DependencyPropertyChangedEventArgs e)
        {
            TryInvalidateDisplay();
        }

        #endregion

        [TypeConverter(typeof(LengthConverter))]
        [Localizability(LocalizationCategory.None)]
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the height of each line of content.")]
        [DisplayName("Shadow Line Height")]
        #region ShadowLineHeight
        public double? ShadowLineHeight
        {
            get
            {
                var value = (double?)GetValue(ShadowLineHeightProperty);
                return value ?? (FontFamily ?? SystemFonts.MessageFontFamily).LineSpacing;
            }
            set { SetValue(ShadowLineHeightProperty, value); }
        }

        /// <summary>
        /// ShadowLineHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty ShadowLineHeightProperty =
            DependencyProperty.Register("ShadowLineHeight", typeof(double?), typeof(FontFormat),
                new FrameworkPropertyMetadata(null,
                    OnShadowLineHeightChanged));


        /// <summary>
        /// Handles changes to the ShadowLineHeight property.
        /// </summary>
        private static void OnShadowLineHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShadowedTextBlock)d).OnShadowLineHeightChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the LineHeight property.
        /// </summary>
        protected virtual void OnShadowLineHeightChanged(DependencyPropertyChangedEventArgs e)
        {
            TryInvalidateDisplay();
        }

        #endregion

        /// <summary>
        /// Gets or sets the ShadowLeftOffset property. This dependency property 
        /// indicates ....
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        [Localizability(LocalizationCategory.None)]
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the shadow left offset")]
        [DisplayName("Shadow Left Offset")]
        #region ShadowLeftOffset

        public double ShadowLeftOffset
        {
            get { return (double)GetValue(ShadowLeftOffsetProperty); }
            set { SetValue(ShadowLeftOffsetProperty, value); }
        }
        /// <summary>
        /// ShadowLeftOffset Dependency Property
        /// </summary>
        public static readonly DependencyProperty ShadowLeftOffsetProperty =
            DependencyProperty.Register("ShadowLeftOffset", typeof(double), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(0d,
                    OnShadowLeftOffsetChanged));

        /// <summary>
        /// Handles changes to the ShadowLeftOffset property.
        /// </summary>
        private static void OnShadowLeftOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ShadowedTextBlock)d;
            var oldShadowLeftOffset = (double)e.OldValue;
            var newShadowLeftOffset = target.ShadowLeftOffset;
            target.OnShadowLeftOffsetChanged(oldShadowLeftOffset, newShadowLeftOffset);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ShadowLeftOffset property.
        /// </summary>
        protected virtual void OnShadowLeftOffsetChanged(double oldShadowLeftOffset, double newShadowLeftOffset)
        {
            TryInvalidateDisplay();
        }

        #endregion

        /// <summary>
        /// Gets or sets the ShadowTopOffset property. This dependency property 
        /// indicates ....
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        [Localizability(LocalizationCategory.None)]
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the shadow top offset")]
        [DisplayName("Shadow Top Offset")]
        #region ShadowTopOffset

        public double ShadowTopOffset
        {
            get { return (double)GetValue(ShadowTopOffsetProperty); }
            set { SetValue(ShadowTopOffsetProperty, value); }
        }
        /// <summary>
        /// ShadowTopOffset Dependency Property
        /// </summary>
        public static readonly DependencyProperty ShadowTopOffsetProperty =
            DependencyProperty.Register("ShadowTopOffset", typeof(double), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(0d,
                    OnShadowTopOffsetChanged));

        /// <summary>
        /// Handles changes to the ShadowTopOffset property.
        /// </summary>
        private static void OnShadowTopOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ShadowedTextBlock)d;
            var oldShadowTopOffset = (double)e.OldValue;
            var newShadowTopOffset = target.ShadowTopOffset;
            target.OnShadowTopOffsetChanged(oldShadowTopOffset, newShadowTopOffset);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ShadowTopOffset property.
        /// </summary>
        protected virtual void OnShadowTopOffsetChanged(double oldShadowTopOffset, double newShadowTopOffset)
        {
            TryInvalidateDisplay();
        }

        #endregion


        /// <summary>
        /// Gets or sets the TextTrimming property. This dependency property 
        /// indicates ....
        /// </summary>
        [TypeConverter(typeof(EnumConverter))]
        [Localizability(LocalizationCategory.None)]
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Indicates whether or not the control must show an indicator that not all the text is visible")]
        [DisplayName("Text Trimming")]
        #region TextTrimming

        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, value); }
        }

        /// <summary>
        /// TextTrimming Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextTrimmingProperty =
            DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(TextTrimming.None,
                    OnTextTrimmingChanged));

        /// <summary>
        /// Handles changes to the TextTrimming property.
        /// </summary>
        private static void OnTextTrimmingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ShadowedTextBlock)d;
            var oldTextTrimming = (TextTrimming)e.OldValue;
            var newTextTrimming = target.TextTrimming;
            target.OnTextTrimmingChanged(oldTextTrimming, newTextTrimming);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TextTrimming property.
        /// </summary>
        protected virtual void OnTextTrimmingChanged(TextTrimming oldTextTrimming, TextTrimming newTextTrimming)
        {
            TryInvalidateDisplay();
        }

        #endregion


        /// <summary>
        /// Gets or sets the TextWrapping property. This dependency property 
        /// indicates ....
        /// </summary>
        [TypeConverter(typeof(EnumConverter))]
        [Localizability(LocalizationCategory.None)]
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Indicates whether the control should try to write text in the next line when current horizontal space is not available")]
        [DisplayName("Text wrapping")]
        #region TextWrapping
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        /// <summary>
        /// TextWrapping Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(TextWrapping.Wrap,
                    OnTextWrappingChanged));


        /// <summary>
        /// Handles changes to the TextWrapping property.
        /// </summary>
        private static void OnTextWrappingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ShadowedTextBlock)d;
            var oldTextWrapping = (TextWrapping)e.OldValue;
            var newTextWrapping = target.TextWrapping;
            target.OnTextWrappingChanged(oldTextWrapping, newTextWrapping);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TextWrapping property.
        /// </summary>
        protected virtual void OnTextWrappingChanged(TextWrapping oldTextWrapping, TextWrapping newTextWrapping)
        {
            TryInvalidateDisplay();
        }

        #endregion

        static ShadowedTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShadowedTextBlock), new FrameworkPropertyMetadata(typeof(ShadowedTextBlock)));
        }

        public override void EndInit()
        {
            base.EndInit();
            InitializeDefaultFontFormat();
        }

        private void InitializeDefaultFontFormat()
        {
            MainTypeface = null;
            MainGlyphTypeface = null;
            MainTypeface = PrepareTypeface(out MainGlyphTypeface);

            ShadowTypeface = null;
            ShadowGlyphTypeface = null;
            ShadowTypeface = PrepareTypeface(out ShadowGlyphTypeface);

            TryInvalidateDisplay();
        }

        internal void TryInvalidateDisplay()
        {
            InvalidateMeasure();
            InvalidateVisual();
        }

        internal Typeface PrepareTypeface(out GlyphTypeface glyphTypeface)
        {
            // If there is not typeface, it creates one
            var typeface = MainTypeface ?? new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);

            // Tries to load the glyphTypeface
            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
            {
                // If failed loading the glyphTypeface, the it tries with the default
                typeface = SystemFonts.MessageFontFamily.GetTypefaces().FirstOrDefault();
                if (typeface != null && !typeface.TryGetGlyphTypeface(out glyphTypeface))
                {
                    throw new InvalidOperationException("Application failed loading GlyphTypeFace for the specified font and the default font.");
                }
            }

            return typeface;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var desiredSize = RenderText(constraint);
            return desiredSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            RenderText(RenderSize, drawingContext);
        }

        private Size RenderText(Size renderSize, DrawingContext drawingContext = null)
        {

            var unavailableGlyphs = UNAVAILABLE_GLYPHS;
            // Draws the background
            #region Draws background
            Brush background = Brushes.Transparent;
            Brush borderBrush = Brushes.Transparent;
            if (Background != null) { background = Background; }
            if (BorderBrush != null) { borderBrush = BorderBrush; }

            if (drawingContext != null)
                drawingContext.DrawRectangle(background, new Pen { Brush = borderBrush }, new Rect(renderSize));
            #endregion

            var shadowRenderedSize = new Size(0, 0);
            //if (!string.IsNullOrWhiteSpace(Text))
            //{
            //    DrawText(
            //        renderSize,
            //        drawingContext,
            //        FontSize,
            //        ShadowColor,
            //        unavailableGlyphs,
            //        ref shadowRenderedSize,
            //        Text,
            //        ShadowGlyphTypeface,
            //        ShadowLineHeight,
            //        ShadowLeftOffset,
            //        ShadowTopOffset);

            //}
            var renderedSize = new Size(0, 0);
            if (!string.IsNullOrWhiteSpace(Text))
            {
                DrawText(
                    renderSize,
                    drawingContext,
                    FontSize,
                    Foreground,
                    unavailableGlyphs,
                    ref renderedSize,
                    Text,
                    MainGlyphTypeface,
                    LineHeight,
                    0,
                    0);
            }
            return new Size(Math.Max(shadowRenderedSize.Width, renderedSize.Width), Math.Max(shadowRenderedSize.Height, renderedSize.Height));
        }

        private void DrawText(Size renderSize, DrawingContext drawingContext, double fontSize, Brush foreground, char[] unavailableGlyphs, ref Size renderedSize, string remainingWordCharacters, GlyphTypeface currentGlyphTypeface, double? lineHeight, double leftOffset, double topOffset)
        {
            var renderingXPosition = leftOffset;
            var renderingYPosition = (lineHeight ?? 0d) + FontSize + topOffset;
            var currentLineHeight = 0d;
            const char periodChar = '.';
            var lineCount = 0d;
            while (remainingWordCharacters.Length > 0)
            {
                if (!((renderingYPosition + (lineHeight ?? 0d) + FontSize) <= (renderSize.Height + Math.Abs(topOffset))))
                {
                    break;
                }
                double wordWidth = 0;
                var glyphIndexes = new List<ushort>();
                var advanceWidths = new List<double>();
                var currentLineWidth = renderingXPosition;
                var currentCharIndex = 0;
                var lastSpaceIndex = -1;

                while (currentCharIndex < remainingWordCharacters.Length)
                {
                    var currentChar = remainingWordCharacters[currentCharIndex];
                    // if it's a non visible char
                    if (unavailableGlyphs.Contains(currentChar))
                    {
                        wordWidth = 0;
                        currentCharIndex++;
                        continue;
                    }
                    // if it's a invalid character
                    if (!currentGlyphTypeface.CharacterToGlyphMap.ContainsKey(currentChar))
                        currentChar = '_';

                    // keeps track of the last WhiteSpace
                    if (char.IsWhiteSpace(currentChar))
                    {
                        wordWidth = 0;
                        lastSpaceIndex = currentCharIndex;
                    }

                    // Gets character dimensions
                    var currentCharGlyphIndex = currentGlyphTypeface.CharacterToGlyphMap[currentChar];
                    var currentCharWidth = currentGlyphTypeface.AdvanceWidths[currentCharGlyphIndex] * fontSize;

                    var isHorizontalSpaceAvailable = (currentLineWidth + currentCharWidth) < (renderSize.Width + Math.Abs(leftOffset));
                    if (isHorizontalSpaceAvailable)
                    {
                        // Add the current character to be rendered
                        glyphIndexes.Add(currentCharGlyphIndex);
                        advanceWidths.Add(currentCharWidth);
                        currentCharIndex++;

                        // Reserves the space (width and height) of the current character
                        wordWidth += currentCharWidth;
                        currentLineWidth += currentCharWidth;
                        currentLineHeight = Math.Max(currentLineHeight, currentGlyphTypeface.AdvanceHeights[currentCharGlyphIndex] * fontSize);
                    }
                    else
                    {
                        var hasFilledVerticalSpace = HasFilledVerticalSpace(ref renderSize, lineHeight, topOffset, renderingYPosition + (lineHeight ?? 0d) + FontSize);
                        if (hasFilledVerticalSpace && TextTrimming == TextTrimming.CharacterEllipsis || (TextWrapping == TextWrapping.NoWrap && TextTrimming == TextTrimming.CharacterEllipsis))
                            break;
                        if (lastSpaceIndex >= 0)
                        {
                            lastSpaceIndex++;
                            currentLineWidth -= wordWidth;
                            while (glyphIndexes.Count > lastSpaceIndex)
                                glyphIndexes.RemoveAt(glyphIndexes.Count - 1);
                            while (advanceWidths.Count > lastSpaceIndex)
                                advanceWidths.RemoveAt(advanceWidths.Count - 1);
                            currentCharIndex = lastSpaceIndex;
                        }
                        break;
                    }
                }
                var nextRenderingYPosition = renderingYPosition + (lineHeight ?? 0d) + FontSize;
                var nextRenderingXPosition = renderingXPosition + currentLineWidth;
                var nextRemainingWordCharacters = new String(remainingWordCharacters.Skip(currentCharIndex).ToArray());
                if (HasFilledVerticalSpace(ref renderSize, lineHeight, topOffset, nextRenderingYPosition) && nextRemainingWordCharacters.Length > 0 && TextTrimming != TextTrimming.None)
                {
                    var requieredTrimWidth = currentGlyphTypeface.AdvanceWidths[currentGlyphTypeface.CharacterToGlyphMap[periodChar]] * fontSize * 3d;
                    var currentTrimWidth = 0d;
                    var advanceWidthsToRemove = new List<double>();
                    foreach (var advanceWidth in (advanceWidths as IEnumerable<double>).Reverse())
                    {
                        if (currentTrimWidth < requieredTrimWidth)
                        {
                            currentTrimWidth += advanceWidth * fontSize;
                            advanceWidthsToRemove.Add(advanceWidth);
                            continue;
                        }
                        break;
                    }

                    for (int i = 0; i < advanceWidthsToRemove.Count && advanceWidths.Count > 0; i++)
                    {
                        advanceWidths.Remove(advanceWidths[advanceWidths.Count - 1]);
                        glyphIndexes.Remove(glyphIndexes[glyphIndexes.Count - 1]);
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        glyphIndexes.Add(currentGlyphTypeface.CharacterToGlyphMap[periodChar]);
                        advanceWidths.Add(currentGlyphTypeface.AdvanceWidths[currentGlyphTypeface.CharacterToGlyphMap[periodChar]] * fontSize);
                    }

                    nextRenderingYPosition = nextRenderingYPosition - (lineHeight ?? 0d) + FontSize;
                    nextRenderingXPosition = nextRenderingXPosition - requieredTrimWidth;
                }
                else
                {
                    remainingWordCharacters = nextRemainingWordCharacters;
                }

                if (drawingContext != null && glyphIndexes.Count > 0)
                {
                    var origin = new Point(renderingXPosition + leftOffset, renderingYPosition + (LineHeight ?? 0d) + topOffset);
                    var glyphRun = new GlyphRun(currentGlyphTypeface, 0, false, fontSize, glyphIndexes, origin, advanceWidths, null, null, null, null, null, null);
                    drawingContext.DrawGlyphRun(foreground, glyphRun);
                }
                lineCount += 1;

                renderedSize.Width = Math.Max(renderedSize.Width, nextRenderingXPosition);
                renderingYPosition = nextRenderingYPosition;
                renderingXPosition = leftOffset;

                if (TextWrapping == TextWrapping.NoWrap) { break; }
            }
            //renderedSize.Height = renderingYPosition;
            renderedSize.Height = lineCount * ((lineHeight ?? 0d) + FontSize) + topOffset;
        }

        private bool HasFilledVerticalSpace(ref Size renderSize, double? lineHeight, double topOffset, double nextRenderingYPosition)
        {
            return ((nextRenderingYPosition + (lineHeight ?? 0d) + FontSize) >= (renderSize.Height + Math.Abs(topOffset)));
        }

    }
}
