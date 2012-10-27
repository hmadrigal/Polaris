//-----------------------------------------------------------------------
// <copyright file="ColorExtensions.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Extensions
{
    using System;
    using System.Windows.Media;
    using Polaris.Extensions;

    public static class ColorExtensions
    {
        /// <summary>
        /// Obtains the "B" component in a HSB color representation.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double GetColorBrightness(this Color target)
        {
            return (double)target.GetMaxValue() / 255.0;
        }

        /// <summary>
        /// Obtains the S component in a HSB color representation.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double GetColorSaturation(this Color target)
        {
            double rgbMax = target.GetMaxValue();
            if (rgbMax == 0) { return 0; }
            double rgbMin = target.GetMinValue();
            double delta = rgbMax - rgbMin;
            return delta / rgbMax;
        }

        /// <summary>
        /// Obtains the H component in the HSB color representation.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double GetColorHue(this Color target)
        {
            double hueValue = 0;
            double rgbMax = target.GetMaxValue();
            if (rgbMax == 0) { return 0; }
            double rgbMin = target.GetMinValue();
            double delta = rgbMax - rgbMin;
            if (delta == 0) { return 0; }
            if (target.R == rgbMax) hueValue = (target.G - target.B) / delta;
            else if (target.G == rgbMax) hueValue = 2 + (target.B - target.R) / delta;
            else if (target.B == rgbMax) hueValue = 4 + (target.R - target.G) / delta;
            hueValue *= 60;
            if (hueValue < 0.0) hueValue += 360;
            return hueValue;
        }

        /// <summary>
        /// Sets the color hue, saturation and brightness.
        /// </summary>
        /// <param name="target">The color to apply the new values to.</param>
        /// <param name="hueValue">The new hue value to apply to this color.</param>
        /// <param name="saturationValue">The new saturation value to apply to this color.</param>
        /// <param name="brightnessValue">The new brightness value to apply to this color.</param>
        /// <returns></returns>
        public static Color SetColorHsb(this Color target, double hueValue, double saturationValue, double brightnessValue)
        {
            hueValue = Math.Min(360, Math.Max(0, hueValue));
            saturationValue = Math.Min(1, Math.Max(0, saturationValue));
            brightnessValue = Math.Min(1, Math.Max(0, brightnessValue));

            if (saturationValue == 0)
            {
                var rgbValue = (Byte)(brightnessValue * 255);
                return Color.FromArgb(target.A, rgbValue, rgbValue, rgbValue);
            }
            else
            {
                var h = (hueValue == 360) ? 0 : hueValue / 60;
#if SILVERLIGHT
                // Since hueValue is a positive number,
                // Math.Floor is equivalent to using Math.Truncate,
                // as the second function is not available in Silverlight.
                var i = (int)(Math.Floor(h));
#else
                var i = (int)(Math.Truncate(h));
#endif
                var f = h - i;

                var p = brightnessValue * (1.0 - saturationValue);
                var q = brightnessValue * (1.0 - (saturationValue * f));
                var t = brightnessValue * (1.0 - (saturationValue * (1.0 - f)));

                double r, g, b;
                switch (i)
                {
                    case 0:
                        r = brightnessValue;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = brightnessValue;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = brightnessValue;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = brightnessValue;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = brightnessValue;
                        break;

                    default:
                        r = brightnessValue;
                        g = p;
                        b = q;
                        break;
                }

                var rValue = (Byte)(r * 255);
                var gValue = (Byte)(g * 255);
                var bValue = (Byte)(b * 255);

                return Color.FromArgb(target.A, rValue, gValue, bValue);
            }
        }

        /// <summary>
        /// Modifies the hue of the color and returns a new instance with the new value.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="hueValue">The new hue to apply to the color. Hue values are represented in degrees, from 0 to 360.</param>
        /// <returns></returns>
        public static Color SetColorHue(this Color target, double hueValue)
        {
            var saturationValue = target.GetColorSaturation();
            var brightnessValue = target.GetColorBrightness();

            hueValue = Math.Min(360, Math.Max(0, hueValue));

            return target.SetColorHsb(hueValue, saturationValue, brightnessValue);
        }

        /// <summary>
        /// Modifies the saturation of the color and returns a new instance with the new value.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="saturationValue">The new saturation to apply to the color. Saturation values are represented in percentage values, using values from 0 to 1.</param>
        /// <returns></returns>
        public static Color SetColorSaturation(this Color target, double saturationValue)
        {
            var hueValue = target.GetColorHue();
            var brightnessValue = target.GetColorBrightness();

            saturationValue = Math.Min(1, Math.Max(0, saturationValue));

            return target.SetColorHsb(hueValue, saturationValue, brightnessValue);
        }

        /// <summary>
        /// Modifies the brightness of the color and returns a new instance with the new value.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="brightnessValue">The new brightness to apply to the color. Saturation values are represented in percentage values, using values from 0 to 1.</param>
        /// <returns></returns>
        public static Color SetColorBrightness(this Color target, double brightnessValue)
        {
            var hueValue = target.GetColorHue();
            var saturationValue = target.GetColorSaturation();

            brightnessValue = Math.Min(1, Math.Max(0, brightnessValue));

            return target.SetColorHsb(hueValue, saturationValue, brightnessValue);
        }

        // HSL to RGB Algorithms were extracted from http://en.wikipedia.org/wiki/HSL_and_HSV#From_HSL

        #region HSL Color Extensions

        public static double GetHSLColorLightness(this Color target)
        {
            var max = GetMaxValue(target) / 255.0;
            var min = GetMinValue(target) / 255.0;
            var lightness = (max + min) / 2.0;
            return lightness;
        }

        public static double GetHSLColorSaturation(this Color target)
        {
            var max = GetMaxValue(target) / 255.0;
            var min = GetMinValue(target) / 255.0;
            var chroma = max - min;
            if (chroma == 0.0) { return 0.0; }
            var lightness = GetHSLColorLightness(target);
            var saturation = (double)chroma / (double)(1.0 - Math.Abs(2.0 * lightness - 1.0));
            saturation = saturation.Clamp(0.0, 1.0);
            return saturation;
        }

        public static double GetHSLColorHue(this Color target)
        {
            var max = GetMaxValue(target) / 255.0;
            var min = GetMinValue(target) / 255.0;
            var r = (target.R / 255.0);
            var g = (target.G / 255.0);
            var b = (target.B / 255.0);
            var chroma = max - min;
            if (chroma == 0.0) { return 0.0; }

            var hue = 0.0;
            if (r == max) { hue = ((g - b) / chroma) % 6; }
            if (g == max) { hue = 2.0 + (b - r) / chroma; }
            if (b == max) { hue = 4.0 + (r - g) / chroma; }

            hue = hue * 60;
            if (hue < 0.0) { hue = hue + 360 * ((hue % 360) + 1); }
            hue = hue / 360.0;
            hue = hue.Clamp(0.0, 1.0);
            return hue;
        }

        public static Color SetColorHSL(this Color target, double hue, double saturation, double lightness)
        {
            hue = hue.Clamp(0.0, 1.0) * 6.0;
            saturation = saturation.Clamp(0.0, 1.0);
            lightness = lightness.Clamp(0.0, 1.0);

            var chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;

            var x = chroma * (1 - Math.Abs((hue % 2) - 1));

            double r, g, b;

            if (0 <= hue && hue < 1)
            {
                r = chroma;
                g = x;
                b = 0;
            }
            else if (1 <= hue && hue < 2)
            {
                r = x;
                g = chroma;
                b = 0;
            }
            else if (2 <= hue && hue < 3)
            {
                r = 0;
                g = chroma;
                b = x;
            }
            else if (3 <= hue && hue < 4)
            {
                r = 0;
                g = x;
                b = chroma;
            }
            else if (4 <= hue && hue < 5)
            {
                r = x;
                g = 0;
                b = chroma;
            }
            else if (5 <= hue && hue < 6)
            {
                r = chroma;
                g = 0;
                b = x;
            }
            else
            {
                r = g = b = 0;
            }
            var lightnessMatchComponent = lightness - 0.5 * chroma;
            r += lightnessMatchComponent; g += lightnessMatchComponent; b += lightnessMatchComponent;

            var rValue = (Byte)(r * 255).Clamp(0, 255);
            var gValue = (Byte)(g * 255).Clamp(0, 255);
            var bValue = (Byte)(b * 255).Clamp(0, 255);
            return Color.FromArgb(target.A, rValue, gValue, bValue);
        }

        public static Color SetHSLColorLightness(this Color target, double lightnessValue)
        {
            var hueValue = target.GetHSLColorHue();
            var saturationValue = target.GetHSLColorSaturation();
            lightnessValue = lightnessValue.Clamp(0.0, 1.0);
            return target.SetColorHSL(hueValue, saturationValue, lightnessValue);
        }

        public static Color SetHSLColoHue(this Color target, double hueValue)
        {
            var lightness = target.GetHSLColorLightness();
            var saturationValue = target.GetHSLColorSaturation();
            hueValue = hueValue.Clamp(0.0, 1.0);
            return target.SetColorHSL(hueValue, saturationValue, lightness);
        }

        public static Color SetHSLColoSaturation(this Color target, double saturationValue)
        {
            var lightness = target.GetHSLColorLightness();
            saturationValue = saturationValue.Clamp(0.0, 1.0);
            var hueValue = target.GetHSLColorHue();
            return target.SetColorHSL(hueValue, saturationValue, lightness);
        }

        #endregion HSL Color Extensions

        private static Byte GetMaxValue(this Color target)
        {
            return Math.Max(target.R, Math.Max(target.G, target.B));
        }

        private static Byte GetMinValue(this Color target)
        {
            return Math.Min(target.R, Math.Min(target.G, target.B));
        }

        /// <summary>
        /// Obtains a color from its hex representation.
        /// </summary>
        /// <param name="hexColor"></param>
        /// <returns></returns>
        public static Color FromHexColor(String hexColor)
        {
            var argumentExceptionMessage = String.Format("hexColor ({0}) does not represent a valid Hexadecimal color code. Format must be '#AARRGGBB'", hexColor);
            try
            {
                return Color.FromArgb(
                        Convert.ToByte(hexColor.Substring(1, 2), 16),
                        Convert.ToByte(hexColor.Substring(3, 2), 16),
                        Convert.ToByte(hexColor.Substring(5, 2), 16),
                        Convert.ToByte(hexColor.Substring(7, 2), 16));
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(argumentExceptionMessage, exception);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(argumentExceptionMessage, exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(argumentExceptionMessage, exception);
            }
        }

        /// <summary>
        /// Tries to parse the specified hex color string. If it succeeds, the function returns true and the parsed value is converted successfully.
        /// </summary>
        /// <param name="hexColor">Hex color string to parse.</param>
        /// <param name="resultingColor">Resulting color from the parse.</param>
        /// <returns>A boolean value on whether the parsing operation succeeded or not.</returns>
        public static Boolean TryParseHexColor(String hexColor, out Color resultingColor)
        {
            resultingColor = default(Color);
            if (hexColor.Length != 9) { return false; }
            var alphaComponentString = hexColor.Substring(1, 2);
            var redComponentString = hexColor.Substring(3, 2);
            var greenComponentString = hexColor.Substring(5, 2);
            var blueComponentString = hexColor.Substring(7, 2);

            Byte alphaComponent;
            Byte redComponent;
            Byte greenComponent;
            Byte blueComponent;

            Boolean alphaSucceeded;
            alphaSucceeded = TryParseHexToByte(alphaComponentString, out alphaComponent);
            if (!alphaSucceeded) { return false; }

            Boolean redSucceeded;
            redSucceeded = TryParseHexToByte(redComponentString, out redComponent);
            if (!redSucceeded) { return false; }

            Boolean greenSucceeded;
            greenSucceeded = TryParseHexToByte(greenComponentString, out greenComponent);
            if (!greenSucceeded) { return false; }

            Boolean blueSucceeded;
            blueSucceeded = TryParseHexToByte(blueComponentString, out blueComponent);
            if (!blueSucceeded) { return false; }

            resultingColor = Color.FromArgb(alphaComponent, redComponent, greenComponent, blueComponent);
            return true;
        }

        private static Boolean TryParseHexToByte(string hexString, out Byte resultingByte)
        {
            return Byte.TryParse(hexString, System.Globalization.NumberStyles.HexNumber, null as IFormatProvider, out resultingByte);
        }
    }
}