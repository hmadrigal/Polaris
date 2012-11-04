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
    public class ShadowedTextBlock : Control
    {
        internal Typeface typeface;
        internal GlyphTypeface glyphTypeface;
        internal ShadowedTextBlock ShadowedTextBlockRef;

        /// <summary>
        /// Summary:
        ///     Gets or sets the font family of the control.
        ///
        /// Returns:
        ///     A font family. The default is the system dialog font.
        /// </summary>
        [Bindable(true)]
        [Localizability(LocalizationCategory.Font)]
        [Category("Appearance")]
        [Description("Gets or sets the font family of the control.")]
        [DisplayName("Font Family")]
        #region FontFamily
        public FontFamily FontFamily
        {
            get
            {
                var value = (FontFamily)GetValue(FontFamilyProperty);
                return value == null ?
                    (ShadowedTextBlockRef == null ? System.Windows.SystemFonts.MessageFontFamily : ShadowedTextBlockRef.FontFamily)
                    : value;
            }
            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// FontFamily Dependency Property
        /// </summary>
        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnFontFamilyChanged)));



        /// <summary>
        /// Handles changes to the FontFamily property.
        /// </summary>
        private static void OnFontFamilyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShadowedTextBlock)d).OnFontFamilyChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the FontFamily property.
        /// </summary>
        protected virtual void OnFontFamilyChanged(DependencyPropertyChangedEventArgs e)
        {
            ReloadShadowedTextBlock();
        }

        #endregion

        ///<summary>
        /// Summary:
        ///     Gets or sets the font size.
        ///
        /// Returns:
        ///     The size of the text in the System.Windows.Controls.Control. The default
        ///     is System.Windows.SystemFonts.MessageFontSize. The font size must be a positive
        ///     number.
        ///</summary>
        [TypeConverter(typeof(FontSizeConverter))]
        [Localizability(LocalizationCategory.None)]
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the font size.")]
        [DisplayName("Font Size")]
        #region FontSize
        public double FontSize
        {
            get
            {
                var value = (double)GetValue(FontSizeProperty);
                return double.IsNaN(value) ?
                    (ShadowedTextBlockRef == null ? System.Windows.SystemFonts.MessageFontSize : ShadowedTextBlockRef.FontSize)
                    : value;
            }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// FontSize Dependency Property
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(double.NaN,
                    new PropertyChangedCallback(OnFontSizeChanged)));

        /// <summary>
        /// Handles changes to the FontSize property.
        /// </summary>
        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShadowedTextBlock)d).OnFontSizeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the FontSize property.
        /// </summary>
        protected virtual void OnFontSizeChanged(DependencyPropertyChangedEventArgs e)
        {
            TryInvalidateDisplay();
        }

        #endregion

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
                var ls = FontFamily == null ? ShadowedTextBlockRef.FontFamily.LineSpacing : FontFamily.LineSpacing;
                return value == null ?
                    (ShadowedTextBlockRef == null ? System.Windows.SystemFonts.MessageFontFamily.LineSpacing : ShadowedTextBlockRef.LineHeight)
                    : (double.IsNaN(value.Value) ? ls : value);
            }
            set { SetValue(LineHeightProperty, value); }
        }

        /// <summary>
        /// LineHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty LineHeightProperty =
            DependencyProperty.Register("LineHeight", typeof(double?), typeof(ShadowedTextBlock),
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

        ///<summary>
        /// Summary:
        ///     Gets or sets the degree to which a font is condensed or expanded on the screen.
        ///
        /// Returns:
        ///     A System.Windows.FontStretch value. The default is System.Windows.FontStretches.Normal.
        ///</summary>
        [Category("Appearance")]
        [Bindable(true)]
        [Description("Gets or sets the degree to which a font is condensed or expanded on the screen.")]
        [DisplayName("Font Stretch")]
        #region FontStretch
        public FontStretch? FontStretch
        {
            get
            {
                var value = (FontStretch?)GetValue(FontStretchProperty);
                return value == null ?
                    (ShadowedTextBlockRef == null ? FontStretches.Normal : ShadowedTextBlockRef.FontStretch)
                    : value;
            }
            set { SetValue(FontStretchProperty, value); }
        }
        /// <summary>
        /// FontStretch Dependency Property
        /// </summary>
        public static readonly DependencyProperty FontStretchProperty =
            DependencyProperty.Register("FontStretch", typeof(FontStretch?), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnFontStretchChanged)));



        /// <summary>
        /// Handles changes to the FontStretch property.
        /// </summary>
        private static void OnFontStretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShadowedTextBlock)d).OnFontStretchChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the FontStretch property.
        /// </summary>
        protected virtual void OnFontStretchChanged(DependencyPropertyChangedEventArgs e)
        {
            ReloadShadowedTextBlock();
        }

        #endregion

        ///<summary>
        /// Summary:
        ///     Gets or sets the font style.
        ///
        /// Returns:
        ///     A System.Windows.FontStyle value. The default is System.Windows.FontStyles.Normal.
        ///</summary>
        [Category("Appearance")]
        [Bindable(true)]
        [Description("Gets or sets the font style.")]
        [DisplayName("Font Style")]
        #region FontStyle
        public FontStyle? FontStyle
        {
            get
            {
                var value = (FontStyle?)GetValue(FontStyleProperty);
                return value == null ?
                    (ShadowedTextBlockRef == null ? System.Windows.FontStyles.Normal : ShadowedTextBlockRef.FontStyle)
                    : value;
            }
            set { SetValue(FontStyleProperty, value); }
        }

        /// <summary>
        /// FontStyle Dependency Property
        /// </summary>
        public static readonly DependencyProperty FontStyleProperty =
            DependencyProperty.Register("FontStyle", typeof(FontStyle?), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata((FontStyle?)null,
                    new PropertyChangedCallback(OnFontStyleChanged)));



        /// <summary>
        /// Handles changes to the FontStyle property.
        /// </summary>
        private static void OnFontStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShadowedTextBlock)d).OnFontStyleChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the FontStyle property.
        /// </summary>
        protected virtual void OnFontStyleChanged(DependencyPropertyChangedEventArgs e)
        {
            ReloadShadowedTextBlock();
        }

        #endregion

        ///<summary>
        /// Summary:
        ///     Gets or sets the weight or thickness of the specified font.
        ///
        /// Returns:
        ///     A System.Windows.FontWeight value. The default is System.Windows.FontWeights.Normal.
        ///</summary>
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the weight or thickness of the specified font.")]
        [DisplayName("Font Weight")]
        #region FontWeight
        public FontWeight? FontWeight
        {
            get
            {
                var value = (FontWeight?)GetValue(FontWeightProperty);
                return value == null ?
                    (ShadowedTextBlockRef == null ? System.Windows.FontWeights.Normal : ShadowedTextBlockRef.FontWeight)
                    : value;
            }
            set { SetValue(FontWeightProperty, value); }
        }

        /// <summary>
        /// FontWeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register("FontWeight", typeof(FontWeight?), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnFontWeightChanged)));


        /// <summary>
        /// Handles changes to the FontWeight property.
        /// </summary>
        private static void OnFontWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShadowedTextBlock)d).OnFontWeightChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the FontWeight property.
        /// </summary>
        protected virtual void OnFontWeightChanged(DependencyPropertyChangedEventArgs e)
        {
            ReloadShadowedTextBlock();
        }

        #endregion

        ///<summary>
        /// Summary:
        ///     Gets or sets a brush that describes the foreground color.
        ///
        /// Returns:
        ///     The brush that paints the foreground of the control. The default value is
        ///     the system dialog font color.
        ///</summary>
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Gets or sets a brush that describes the foreground color.")]
        #region Foreground

        /// <summary>
        /// Foreground Dependency Property
        /// </summary>
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnForegroundChanged)));

        /// <summary>
        /// Gets or sets the Foreground property.  This dependency property 
        /// indicates ....
        /// </summary>
        public Brush Foreground
        {
            get
            {
                var value = (Brush)GetValue(ForegroundProperty);
                return value == null ?
                    (ShadowedTextBlockRef == null ? System.Windows.SystemColors.ControlTextBrush : ShadowedTextBlockRef.Foreground)
                    : value;
            }
            set { SetValue(ForegroundProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Foreground property.
        /// </summary>
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShadowedTextBlock)d).OnForegroundChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Foreground property.
        /// </summary>
        protected virtual void OnForegroundChanged(DependencyPropertyChangedEventArgs e)
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
                return value == null ?
                    (ShadowedTextBlockRef == null ? System.Windows.SystemColors.ControlTextBrush : ShadowedTextBlockRef.ShadowColor)
                    : value;
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

        /// <summary>
        /// Summary:
        ///     Gets or sets the style used by this element when it is rendered.
        ///
        /// Returns:
        ///     The applied, nondefault style for the element, if present. Otherwise, null.
        ///     The default for a default-constructed System.Windows.FrameworkElement is
        ///     null.
        ///</summary>
        [Bindable(true)]
        [Description("Gets or sets the style used by this element when it is rendered.")]
        #region Style
        public Style Style
        {
            get { return (Style)GetValue(StyleProperty); }
            set { SetValue(StyleProperty, value); }
        }
        /// <summary>
        /// Style Dependency Property
        /// </summary>
        public static readonly DependencyProperty StyleProperty =
            DependencyProperty.Register("Style", typeof(Style), typeof(ShadowedTextBlock),
                new FrameworkPropertyMetadata(default(Style),
                    new PropertyChangedCallback(OnStyleChanged)));



        /// <summary>
        /// Handles changes to the Style property.
        /// </summary>
        private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShadowedTextBlock)d).OnStyleChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Style property.
        /// </summary>
        protected virtual void OnStyleChanged(DependencyPropertyChangedEventArgs e)
        {
            Style oldStyle = e.OldValue as Style;
            Style newStyle = e.NewValue as Style;
            OnStyleChanged(oldStyle, newStyle);
            TryInvalidateDisplay();
        }

        protected virtual void OnStyleChanged(Style oldStyle, Style newStyle)
        {
            newStyle.Setters.OfType<Setter>().Select(s =>
            {
                SetValue(s.Property, s.Value); return s;
            }).Any();
            ReloadShadowedTextBlock();
        }

        #endregion

        protected void ReloadShadowedTextBlock()
        {
            typeface = null;
            glyphTypeface = null;
            PrepareTypeface(this.ShadowedTextBlockRef);
            TryInvalidateDisplay();
        }

        internal void TryInvalidateDisplay()
        {
            if (ShadowedTextBlockRef != null)
            {
                ShadowedTextBlockRef.InvalidateMeasure();
                ShadowedTextBlockRef.InvalidateVisual();
            }
        }

        internal void PrepareTypeface(ShadowedTextBlock textdisplay = null)
        {
            // Reference to the text display
            if (textdisplay != null)
            {
                ShadowedTextBlockRef = textdisplay;
            }
            // If there is not typeface, it creates one
            if (typeface == null)
            { typeface = new Typeface(FontFamily, FontStyle.Value, FontWeight.Value, FontStretch.Value); }

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


        }
    }
}
