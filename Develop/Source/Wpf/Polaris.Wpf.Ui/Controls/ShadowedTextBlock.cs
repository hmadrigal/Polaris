using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        internal Typeface _typeface;
        internal GlyphTypeface _glyphTypeface;
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
        [Category("Text displayed as shadow.")]
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
            _typeface = null;
            _glyphTypeface = null;
            _typeface = PrepareTypeface(out _glyphTypeface);
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
            double renderingXPosition = 0d;
            double renderingYPosition = (LineHeight ?? 0d) + FontSize;
            double wordWidth = 0, currentLineHeight = 0;
            bool isHorizontalSpaceAvailable = true;
            //bool canRendered = false;

            var fontSize = FontSize;
            var foreground = Foreground;
            var unavailableGlyphs = UNAVAILABLE_GLYPHS;
            Brush background = Brushes.Transparent;
            Brush borderBrush = Brushes.Transparent;
            if (Background != null) { background = Background; }
            if (BorderBrush != null) { borderBrush = BorderBrush; }

            if (drawingContext != null)
            {
                drawingContext.DrawRectangle(background, new Pen() { Brush = borderBrush }, new Rect(renderSize));
            }

            Size renderedSize = new Size(0, 0);
            if (string.IsNullOrWhiteSpace(Text))
                return renderedSize;

            string remainingWordCharacters = Text;
            var currentGlyphTypeface = _glyphTypeface;
            while (remainingWordCharacters.Length > 0)
            {
                // End of vertical space
                if (renderingYPosition > renderSize.Height) { break; }

                wordWidth = 0;
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

                    isHorizontalSpaceAvailable = (currentLineWidth + currentCharWidth) < renderSize.Width;
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
                        if (lastSpaceIndex >= 0)
                        {
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
                remainingWordCharacters = new String(remainingWordCharacters.Skip(currentCharIndex).ToArray());

                if (drawingContext != null && glyphIndexes.Count > 0)
                {
                    var origin = new Point(renderingXPosition, renderingYPosition + LineHeight.Value);
                    var glyphRun = new GlyphRun(currentGlyphTypeface, 0, false, fontSize, glyphIndexes, origin, advanceWidths, null, null, null, null, null, null);
                    drawingContext.DrawGlyphRun(foreground == null ? foreground : foreground, glyphRun);
                }

                renderingYPosition += (LineHeight ?? 0d) + fontSize;
                renderingXPosition += currentLineWidth;
                renderedSize.Width = Math.Max(renderedSize.Width, renderingXPosition + 1);

                if (string.IsNullOrEmpty(remainingWordCharacters))
                    break;
                renderingXPosition = 0;

            }
            renderedSize.Height = renderingYPosition;
            return renderedSize;
        }

    }
}
