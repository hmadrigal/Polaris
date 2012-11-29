using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;
using Polaris.Windows.Extensions;

namespace Polaris.Windows.Controls
{
    [Localizability(LocalizationCategory.Text)]
    [DisplayName("Shadow Textblock")]
    [Description("Displays a text and draws drop shadow with it.")]
    public class ShadowedTextBlock : Control
    {
        internal Typeface _mainTypeface;
        internal GlyphTypeface _mainGlyphTypeface;
        internal Typeface _shadowTypeface;
        internal GlyphTypeface _shadowGlyphTypeface;

        protected char[] UNAVAILABLE_GLYPHS = new char[] { '\n', '\r' };

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
                return value ?? (FontFamily == null ? FontFamily.LineSpacing : System.Windows.SystemFonts.MessageFontFamily.LineSpacing);
            }
            set { SetValue(LineHeightProperty, value); }
        }

        /// <summary>
        /// LineHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty LineHeightProperty =
            DependencyProperty.Register("LineHeight", typeof(double?), typeof(FontFormat),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnLineHeightChanged)));


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
                    new PropertyChangedCallback(OnTextChanged)));

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
            var newValue = e.NewValue as string;
            TryInvalidateDisplay();
        }

        #endregion Text

        [Localizability(LocalizationCategory.Text)]
        [DefaultValue(null)]
        [Category("Common Properties")]
        [Description("Text displayed as shadow.")]
        [Bindable(true)]
        #region ShadowText

        /// <summary>
        /// ShadowText Dependency Property
        /// </summary>
        public static readonly DependencyProperty ShadowTextProperty =
            DependencyProperty.Register("ShadowText", typeof(string), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(string.Empty,
                    new PropertyChangedCallback(OnShadowTextChanged)));

        /// <summary>
        /// Gets or sets the ShadowText property.  This dependency property 
        /// indicates ....
        /// </summary>
        public string ShadowText
        {
            get { return (string)GetValue(ShadowTextProperty); }
            set { SetValue(ShadowTextProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ShadowText property.
        /// </summary>
        private static void OnShadowTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShadowedTextBlock)d).OnShadowTextChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ShadowText property.
        /// </summary>
        protected virtual void OnShadowTextChanged(DependencyPropertyChangedEventArgs e)
        {
            TryInvalidateDisplay();
        }

        #endregion

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

        /// <summary>
        /// ShadowColor Dependency Property
        /// </summary>
        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register("ShadowColor", typeof(Brush), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnShadowColorChanged)));

        /// <summary>
        /// Gets or sets the ShadowColor property.  This dependency property 
        /// indicates ....
        /// </summary>
        public Brush ShadowColor
        {
            get
            {
                var value = (Brush)GetValue(ShadowColorProperty);
                return value ?? (System.Windows.SystemColors.ControlTextBrush);
            }
            set { SetValue(ShadowColorProperty, value); }
        }

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
                return value ?? (FontFamily == null ? FontFamily.LineSpacing : System.Windows.SystemFonts.MessageFontFamily.LineSpacing);
            }
            set { SetValue(ShadowLineHeightProperty, value); }
        }

        /// <summary>
        /// ShadowLineHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty ShadowLineHeightProperty =
            DependencyProperty.Register("ShadowLineHeight", typeof(double?), typeof(FontFormat),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnShadowLineHeightChanged)));


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

        [TypeConverter(typeof(LengthConverter))]
        [Localizability(LocalizationCategory.None)]
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the shadow left offset")]
        [DisplayName("Shadow Left Offset")]
        #region ShadowLeftOffset
        /// <summary>
        /// Gets or sets the ShadowLeftOffset property. This dependency property 
        /// indicates ....
        /// </summary>
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
                    new PropertyChangedCallback(OnShadowLeftOffsetChanged)));

        /// <summary>
        /// Handles changes to the ShadowLeftOffset property.
        /// </summary>
        private static void OnShadowLeftOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ShadowedTextBlock target = (ShadowedTextBlock)d;
            double oldShadowLeftOffset = (double)e.OldValue;
            double newShadowLeftOffset = target.ShadowLeftOffset;
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

        [TypeConverter(typeof(LengthConverter))]
        [Localizability(LocalizationCategory.None)]
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the shadow top offset")]
        [DisplayName("Shadow Top Offset")]
        #region ShadowTopOffset
        /// <summary>
        /// Gets or sets the ShadowTopOffset property. This dependency property 
        /// indicates ....
        /// </summary>
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
                    new PropertyChangedCallback(OnShadowTopOffsetChanged)));

        /// <summary>
        /// Handles changes to the ShadowTopOffset property.
        /// </summary>
        private static void OnShadowTopOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ShadowedTextBlock target = (ShadowedTextBlock)d;
            double oldShadowTopOffset = (double)e.OldValue;
            double newShadowTopOffset = target.ShadowTopOffset;
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

        #region TextTrimming
        /// <summary>
        /// Gets or sets the TextTrimming property. This dependency property 
        /// indicates ....
        /// </summary>
        public System.Windows.TextTrimming TextTrimming
        {
            get { return (System.Windows.TextTrimming)GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, value); }
        }

        /// <summary>
        /// TextTrimming Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextTrimmingProperty =
            DependencyProperty.Register("TextTrimming", typeof(System.Windows.TextTrimming), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(System.Windows.TextTrimming.None,
                    new PropertyChangedCallback(OnTextTrimmingChanged)));

        /// <summary>
        /// Handles changes to the TextTrimming property.
        /// </summary>
        private static void OnTextTrimmingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ShadowedTextBlock target = (ShadowedTextBlock)d;
            System.Windows.TextTrimming oldTextTrimming = (System.Windows.TextTrimming)e.OldValue;
            System.Windows.TextTrimming newTextTrimming = target.TextTrimming;
            target.OnTextTrimmingChanged(oldTextTrimming, newTextTrimming);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TextTrimming property.
        /// </summary>
        protected virtual void OnTextTrimmingChanged(System.Windows.TextTrimming oldTextTrimming, System.Windows.TextTrimming newTextTrimming)
        {
            TryInvalidateDisplay();
        }

        #endregion

        #region TextWrapping
        /// <summary>
        /// Gets or sets the TextWrapping property. This dependency property 
        /// indicates ....
        /// </summary>
        public System.Windows.TextWrapping TextWrapping
        {
            get { return (System.Windows.TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        /// <summary>
        /// TextWrapping Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(System.Windows.TextWrapping), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(System.Windows.TextWrapping.Wrap,
                    new PropertyChangedCallback(OnTextWrappingChanged)));


        /// <summary>
        /// Handles changes to the TextWrapping property.
        /// </summary>
        private static void OnTextWrappingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ShadowedTextBlock target = (ShadowedTextBlock)d;
            System.Windows.TextWrapping oldTextWrapping = (System.Windows.TextWrapping)e.OldValue;
            System.Windows.TextWrapping newTextWrapping = target.TextWrapping;
            target.OnTextWrappingChanged(oldTextWrapping, newTextWrapping);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TextWrapping property.
        /// </summary>
        protected virtual void OnTextWrappingChanged(System.Windows.TextWrapping oldTextWrapping, System.Windows.TextWrapping newTextWrapping)
        {
            TryInvalidateDisplay();
        }

        #endregion

        static ShadowedTextBlock()
        {
            var refTextBlock = new TextBlock();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShadowedTextBlock), new FrameworkPropertyMetadata(typeof(ShadowedTextBlock)));
        }

        public override void EndInit()
        {
            base.EndInit();
            InitializeDefaultFontFormat();
        }

        private void InitializeDefaultFontFormat()
        {
            _mainTypeface = null;
            _mainGlyphTypeface = null;
            _mainTypeface = PrepareTypeface(out _mainGlyphTypeface);

            _shadowTypeface = null;
            _shadowGlyphTypeface = null;
            _shadowTypeface = PrepareTypeface(out _shadowGlyphTypeface);

            TryInvalidateDisplay();
        }

        internal void TryInvalidateDisplay()
        {
            InvalidateMeasure();
            InvalidateVisual();
        }

        internal Typeface PrepareTypeface(out GlyphTypeface glyphTypeface)
        {
            Typeface typeface = null;
            // If there is not typeface, it creates one
            if (typeface == null)
            { typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch); }

            // Tries to load the glyphTypeface
            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
            {
                // If failed loading the glyphTypeface, the it tries with the default
                typeface = System.Windows.SystemFonts.MessageFontFamily.GetTypefaces().FirstOrDefault();
                if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
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
                drawingContext.DrawRectangle(background, new Pen() { Brush = borderBrush }, new Rect(renderSize));
            #endregion

            var shadowRenderedSize = new Size(0, 0);
            if (!string.IsNullOrWhiteSpace(ShadowText))
            {
                DrawText(
                    renderSize,
                    drawingContext,
                    FontSize,
                    ShadowColor,
                    unavailableGlyphs,
                    ref shadowRenderedSize,
                    ShadowText,
                    _shadowGlyphTypeface,
                    ShadowLineHeight,
                    ShadowLeftOffset,
                    ShadowTopOffset);

            }
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
                    _mainGlyphTypeface,
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
            var periodChar = '.';
            while (remainingWordCharacters.Length > 0 && ((renderingYPosition + (lineHeight ?? 0d) + FontSize) <= (renderSize.Height + Math.Abs(topOffset))))
            {

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
                        if (hasFilledVerticalSpace && TextTrimming == System.Windows.TextTrimming.CharacterEllipsis || (TextWrapping == System.Windows.TextWrapping.NoWrap && TextTrimming == System.Windows.TextTrimming.CharacterEllipsis))
                            break;
                        if (lastSpaceIndex >= 0 )
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
                if (HasFilledVerticalSpace(ref renderSize, lineHeight, topOffset, nextRenderingYPosition) && nextRemainingWordCharacters.Length > 0 && TextTrimming != System.Windows.TextTrimming.None)
                {
                    var requieredTrimWidth = currentGlyphTypeface.AdvanceWidths[currentGlyphTypeface.CharacterToGlyphMap[periodChar]] * fontSize * 3d;
                    var currentTrimWidth = 0d;
                    List<double> advanceWidthsToRemove = new List<double>();
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

                    for (int i = 0; i < advanceWidthsToRemove.Count && advanceWidths.Count>0; i++)
                    {
                        advanceWidths.Remove(advanceWidths[advanceWidths.Count-1]);
                        glyphIndexes.Remove(glyphIndexes[glyphIndexes.Count-1]);
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        glyphIndexes.Add(currentGlyphTypeface.CharacterToGlyphMap[periodChar]);
                        advanceWidths.Add(currentGlyphTypeface.AdvanceWidths[currentGlyphTypeface.CharacterToGlyphMap[periodChar]] * fontSize);
                    }

                    nextRenderingYPosition = nextRenderingYPosition - (lineHeight ?? 0d) + FontSize;
                    nextRenderingXPosition = nextRenderingXPosition - requieredTrimWidth;
                    nextRemainingWordCharacters = string.Empty;

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


                renderedSize.Width = Math.Max(renderedSize.Width, nextRenderingXPosition);
                renderingYPosition = nextRenderingYPosition;
                renderingXPosition = leftOffset;

                if (TextWrapping == System.Windows.TextWrapping.NoWrap) { break; }
            }
            renderedSize.Height = renderingYPosition;
        }

        private bool HasFilledVerticalSpace(ref Size renderSize, double? lineHeight, double topOffset, double nextRenderingYPosition)
        {
            return ((nextRenderingYPosition + (lineHeight ?? 0d) + FontSize) >= (renderSize.Height + Math.Abs(topOffset)));
        }

    }
}
