﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics
{
    /// <summary>
    /// A color specified by the values for Red, Green and Blue, Alpha.
    /// The range for all of these values is 0-255.
    /// </summary>
    /// <remarks>
    /// Often color-systems also support a fourth value: the alpha-channel.
    /// This value indicates the transperency of the color. This is not implemented here.
    /// </remarks>
    public record Color(int R, int G, int B, int Alpha)
    {
        /// <summary>
        /// Create a color by red, green and blue values
        /// </summary>
        /// <param name="R">red value (between 0 and 255)</param>
        /// <param name="G">green value (between 0 and 255)</param>
        /// <param name="B">blue value (between 0 and 255)</param>
        public Color(int R, int G, int B) : this(R, G, B, 255)
        {
        }

        /// <summary>
        /// Black Color
        /// </summary>
        public Color() : this(0, 0, 0)
        {

        }

        /// <summary>
        /// Create a transparent version of this color
        /// </summary>
        /// <param name="transparent">Transparence: 0.0 = fully opaque, 1.0 = fully transparent</param>
        /// <returns>The transparent color</returns>
        public Color Transparent(double transparent)
        {
            var color = new Color(R, G, B, Convert.ToInt32(255 * (1.0 - transparent)));
            return color;
        }

        /// <summary>
        /// Change the hue of the color by <paramref name="angle"/> units and return the resulting color
        /// </summary>
        /// <param name="angle">Rotation-value specified in degrees (value of 360 is a full rotation). Positive values turn red - green - blue , negative values turn blue - green - red.</param>
        public Color RotateHue(double angle)
        {
            var (hue, saturation, value) = ToHSV();

            var newHue = (hue + angle) % 360;
            if (newHue < 0) newHue += 360;

            return FromHSV(newHue, saturation, value);
        }

        /// <summary>
        /// Create Color by string
        /// </summary>
        /// <param name="colorName"></param>
        public static implicit operator Color(string colorName) => ConvertName(colorName);

        /// <summary>
        /// Create a color from hue, saturation and value
        /// </summary>
        /// <param name="hue">Angle on the color circle. 0 red, 120 green, 240 blue, between 240 and 360 position on purple line</param>
        /// <param name="saturation">Saturation. 0.0 neutral gray, 0.5 little saturation, 1.0 pure color</param>
        /// <param name="value">Value. 0.0 no brightness (black), 1.0 full brighntess</param>
        /// <returns>The color</returns>
        public static Color FromHSV(double hue, double saturation, double value)
        {
            if (hue > 360.0)
                hue = hue % 360;

            hue = Math.Max(hue, 0.0);
            saturation = Math.Max(Math.Min(saturation, 1.0), 0.0);
            value = Math.Max(Math.Min(value, 1.0), 0.0);

            var hi = (int)Math.Truncate(hue / 60.0);
            var f = (hue / 60.0 - hi);
            var p = value * (1.0 - saturation);
            var q = value * (1.0 - saturation * f);
            var t = value * (1.0 - saturation * (1 - f));

            var rgbTupel = hi switch
            {
                0 => (value, t, p),
                6 => (value, t, p),
                1 => (q, value, p),
                2 => (p, value, t),
                3 => (p, q, value),
                4 => (t, p, value),
                5 => (value, p, q),
                _ => (0.0, 0.0, 0.0)
            };

            return new Color((int)(rgbTupel.Item1 * 255), (int)(rgbTupel.Item2 * 255), (int)(rgbTupel.Item3 * 255));
        }

        /// <summary>
        /// Calculate the HSV values for a given Color
        /// </summary>
        /// <returns>A 3-tupel with hue, saturation and value. See method FromHSV() for the meaning of these values.</returns>
        public (double Hue, double Saturation, double Value) ToHSV()
        {
            // Calculation according to https://de.wikipedia.org/wiki/HSV-Farbraum
            double red = R / 255.0;
            double green = G / 255.0;
            double blue = B / 255.0;

            double min = Math.Min(Math.Min(red, green), blue);
            double max = Math.Max(Math.Max(red, green), blue);

            double hue = 0;
            if (max == min)
                hue = 0;
            else if (max == red)
                hue = 60.0 * (0 + (green - blue) / (max - min));
            else if (max == green)
                hue = 60.0 * (2 + (blue - red) / (max - min));
            else // max == blue
                hue = 60.0 * (4 + (red - green) / (max - min));
            if (hue < 0)
                hue += 360.0;

            double saturation = (max == min) ? 0 : (max - min) / max;

            double value = max;

            return (hue, saturation, value);
        }

        private static Dictionary<string, Color> s_colorsByName = null;
        private static readonly object s_lockObj = new();

        private static Color ConvertName(string colorName)
        {
            if (s_colorsByName == null)
            {
                lock (s_lockObj)
                {
                    if (s_colorsByName == null)
                    {
                        s_colorsByName = new();
                        var colorValueMethods = typeof(Colors).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                        foreach (var colorValueMethod in colorValueMethods)
                        {
                            var result = colorValueMethod.Invoke(null, null);
                            var col = (Color)result;
                            var methodName = colorValueMethod.Name[4..]; // das get_ am Anfang abschneiden
                            var key = methodName.ToLower();
                            s_colorsByName.Add(key, col);
                        }
                    }
                }
            }

            return s_colorsByName.GetValueOrDefault(colorName, Colors.Black);
        }

    }

    /// <summary>
    /// Predefined Colors
    /// </summary>
    public class Colors
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Color MediumPurple { get { return new Color(147, 112, 219); } }
        public static Color MediumSeaGreen { get { return new Color(60, 179, 113); } }
        public static Color MediumSlateBlue { get { return new Color(123, 104, 238); } }
        public static Color MediumSpringGreen { get { return new Color(0, 250, 154); } }
        public static Color MediumTurquoise { get { return new Color(72, 209, 204); } }
        public static Color MediumVioletRed { get { return new Color(199, 21, 133); } }
        public static Color MidnightBlue { get { return new Color(25, 25, 112); } }
        public static Color MintCream { get { return new Color(245, 255, 250); } }
        public static Color MistyRose { get { return new Color(255, 228, 225); } }
        public static Color Moccasin { get { return new Color(255, 228, 181); } }
        public static Color NavajoWhite { get { return new Color(255, 222, 173); } }
        public static Color Navy { get { return new Color(0, 0, 128); } }
        public static Color OldLace { get { return new Color(253, 245, 230); } }
        public static Color Olive { get { return new Color(128, 128, 0); } }
        public static Color OliveDrab { get { return new Color(107, 142, 35); } }
        public static Color Orange { get { return new Color(255, 165, 0); } }
        public static Color OrangeRed { get { return new Color(255, 69, 0); } }
        public static Color Orchid { get { return new Color(218, 112, 214); } }
        public static Color PaleGoldenrod { get { return new Color(238, 232, 170); } }
        public static Color PaleGreen { get { return new Color(152, 251, 152); } }
        public static Color PaleTurquoise { get { return new Color(175, 238, 238); } }
        public static Color PaleVioletRed { get { return new Color(219, 112, 147); } }
        public static Color PapayaWhip { get { return new Color(255, 239, 213); } }
        public static Color PeachPuff { get { return new Color(255, 218, 185); } }
        public static Color Peru { get { return new Color(205, 133, 63); } }
        public static Color Pink { get { return new Color(255, 192, 203); } }
        public static Color Plum { get { return new Color(221, 160, 221); } }
        public static Color PowderBlue { get { return new Color(176, 224, 230); } }
        public static Color Purple { get { return new Color(128, 0, 128); } }
        public static Color Red { get { return new Color(255, 0, 0); } }
        public static Color RosyBrown { get { return new Color(188, 143, 143); } }
        public static Color RoyalBlue { get { return new Color(65, 105, 225); } }
        public static Color SaddleBrown { get { return new Color(139, 69, 19); } }
        public static Color Salmon { get { return new Color(250, 128, 114); } }
        public static Color SandyBrown { get { return new Color(244, 164, 96); } }
        public static Color SeaGreen { get { return new Color(46, 139, 87); } }
        public static Color SeaShell { get { return new Color(255, 245, 238); } }
        public static Color Sienna { get { return new Color(160, 82, 45); } }
        public static Color Silver { get { return new Color(192, 192, 192); } }
        public static Color SkyBlue { get { return new Color(135, 206, 235); } }
        public static Color SlateBlue { get { return new Color(106, 90, 205); } }
        public static Color SlateGray { get { return new Color(112, 128, 144); } }
        public static Color Snow { get { return new Color(255, 250, 250); } }
        public static Color SpringGreen { get { return new Color(0, 255, 127); } }
        public static Color SteelBlue { get { return new Color(70, 130, 180); } }
        public static Color Tan { get { return new Color(210, 180, 140); } }
        public static Color Teal { get { return new Color(0, 128, 128); } }
        public static Color Thistle { get { return new Color(216, 191, 216); } }
        public static Color Tomato { get { return new Color(255, 99, 71); } }
        public static Color Transparent { get { return new Color(255, 255, 255); } }
        public static Color Turquoise { get { return new Color(64, 224, 208); } }
        public static Color Violet { get { return new Color(238, 130, 238); } }
        public static Color Wheat { get { return new Color(245, 222, 179); } }
        public static Color White { get { return new Color(255, 255, 255); } }
        public static Color WhiteSmoke { get { return new Color(245, 245, 245); } }
        public static Color Yellow { get { return new Color(255, 255, 0); } }
        public static Color YellowGreen { get { return new Color(154, 205, 50); } }
        public static Color AliceBlue { get { return new Color(240, 248, 255); } }
        public static Color AntiqueWhite { get { return new Color(250, 235, 215); } }
        public static Color Aqua { get { return new Color(0, 255, 255); } }
        public static Color Aquamarine { get { return new Color(127, 255, 212); } }
        public static Color Azure { get { return new Color(240, 255, 255); } }
        public static Color Beige { get { return new Color(245, 245, 220); } }
        public static Color Bisque { get { return new Color(255, 228, 196); } }
        public static Color Black { get { return new Color(0, 0, 0); } }
        public static Color BlanchedAlmond { get { return new Color(255, 235, 205); } }
        public static Color Blue { get { return new Color(0, 0, 255); } }
        public static Color BlueViolet { get { return new Color(138, 43, 226); } }
        public static Color Brown { get { return new Color(165, 42, 42); } }
        public static Color BurlyWood { get { return new Color(222, 184, 135); } }
        public static Color CadetBlue { get { return new Color(95, 158, 160); } }
        public static Color Chartreuse { get { return new Color(127, 255, 0); } }
        public static Color Chocolate { get { return new Color(210, 105, 30); } }
        public static Color Coral { get { return new Color(255, 127, 80); } }
        public static Color CornflowerBlue { get { return new Color(100, 149, 237); } }
        public static Color Cornsilk { get { return new Color(255, 248, 220); } }
        public static Color Crimson { get { return new Color(220, 20, 60); } }
        public static Color Cyan { get { return new Color(0, 255, 255); } }
        public static Color DarkBlue { get { return new Color(0, 0, 139); } }
        public static Color DarkCyan { get { return new Color(0, 139, 139); } }
        public static Color DarkGoldenrod { get { return new Color(184, 134, 11); } }
        public static Color DarkGray { get { return new Color(169, 169, 169); } }
        public static Color DarkGreen { get { return new Color(0, 100, 0); } }
        public static Color DarkKhaki { get { return new Color(189, 183, 107); } }
        public static Color DarkMagenta { get { return new Color(139, 0, 139); } }
        public static Color DarkOliveGreen { get { return new Color(85, 107, 47); } }
        public static Color DarkOrange { get { return new Color(255, 140, 0); } }
        public static Color DarkOrchid { get { return new Color(153, 50, 204); } }
        public static Color DarkRed { get { return new Color(139, 0, 0); } }
        public static Color DarkSalmon { get { return new Color(233, 150, 122); } }
        public static Color DarkSeaGreen { get { return new Color(143, 188, 143); } }
        public static Color DarkSlateBlue { get { return new Color(72, 61, 139); } }
        public static Color DarkSlateGray { get { return new Color(47, 79, 79); } }
        public static Color DarkTurquoise { get { return new Color(0, 206, 209); } }
        public static Color DarkViolet { get { return new Color(148, 0, 211); } }
        public static Color DeepPink { get { return new Color(255, 20, 147); } }
        public static Color DeepSkyBlue { get { return new Color(0, 191, 255); } }
        public static Color DimGray { get { return new Color(105, 105, 105); } }
        public static Color DodgerBlue { get { return new Color(30, 144, 255); } }
        public static Color Firebrick { get { return new Color(178, 34, 34); } }
        public static Color FloralWhite { get { return new Color(255, 250, 240); } }
        public static Color ForestGreen { get { return new Color(34, 139, 34); } }
        public static Color Fuchsia { get { return new Color(255, 0, 255); } }
        public static Color Gainsboro { get { return new Color(220, 220, 220); } }
        public static Color GhostWhite { get { return new Color(248, 248, 255); } }
        public static Color Gold { get { return new Color(255, 215, 0); } }
        public static Color Goldenrod { get { return new Color(218, 165, 32); } }
        public static Color Gray { get { return new Color(128, 128, 128); } }
        public static Color Green { get { return new Color(0, 128, 0); } }
        public static Color GreenYellow { get { return new Color(173, 255, 47); } }
        public static Color Honeydew { get { return new Color(240, 255, 240); } }
        public static Color HotPink { get { return new Color(255, 105, 180); } }
        public static Color IndianRed { get { return new Color(205, 92, 92); } }
        public static Color Indigo { get { return new Color(75, 0, 130); } }
        public static Color Ivory { get { return new Color(255, 255, 240); } }
        public static Color Khaki { get { return new Color(240, 230, 140); } }
        public static Color Lavender { get { return new Color(230, 230, 250); } }
        public static Color LavenderBlush { get { return new Color(255, 240, 245); } }
        public static Color LawnGreen { get { return new Color(124, 252, 0); } }
        public static Color LemonChiffon { get { return new Color(255, 250, 205); } }
        public static Color LightBlue { get { return new Color(173, 216, 230); } }
        public static Color LightCoral { get { return new Color(240, 128, 128); } }
        public static Color LightCyan { get { return new Color(224, 255, 255); } }
        public static Color LightGoldenrodYellow { get { return new Color(250, 250, 210); } }
        public static Color LightGray { get { return new Color(211, 211, 211); } }
        public static Color LightGreen { get { return new Color(144, 238, 144); } }
        public static Color LightPink { get { return new Color(255, 182, 193); } }
        public static Color LightSalmon { get { return new Color(255, 160, 122); } }
        public static Color LightSeaGreen { get { return new Color(32, 178, 170); } }
        public static Color LightSkyBlue { get { return new Color(135, 206, 250); } }
        public static Color LightSlateGray { get { return new Color(119, 136, 153); } }
        public static Color LightSteelBlue { get { return new Color(176, 196, 222); } }
        public static Color LightYellow { get { return new Color(255, 255, 224); } }
        public static Color Lime { get { return new Color(0, 255, 0); } }
        public static Color LimeGreen { get { return new Color(50, 205, 50); } }
        public static Color Linen { get { return new Color(250, 240, 230); } }
        public static Color Magenta { get { return new Color(255, 0, 255); } }
        public static Color Maroon { get { return new Color(128, 0, 0); } }
        public static Color MediumAquamarine { get { return new Color(102, 205, 170); } }
        public static Color MediumBlue { get { return new Color(0, 0, 205); } }
        public static Color MediumOrchid { get { return new Color(186, 85, 211); } }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

}
