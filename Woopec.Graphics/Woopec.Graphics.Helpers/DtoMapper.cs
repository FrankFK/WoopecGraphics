using System;
using Woopec.Graphics.InternalDtos;

namespace Woopec.Graphics.Helpers
{
    /// <summary>
    /// I do not want to have any dependencies to the data types (i.e. to Vec2D, Color, etc.) of Woopec.Graphics in the other namespaces.
    /// That's why I map these data types to Dto types.
    /// </summary>
    internal static class DtoMapper
    {
        public static Vec2DValue Map(Vec2D woopecValue) => (woopecValue is null) ? null : new Vec2DValue(woopecValue.X, woopecValue.Y);
        public static Vec2D Map(Vec2DValue dtoValue) => (dtoValue is null) ? null : new Vec2D(dtoValue.X, dtoValue.Y);

        public static ColorValue Map(Color woopecColor) => (woopecColor is null) ? null : new ColorValue(woopecColor.R, woopecColor.G, woopecColor.B, woopecColor.Alpha);
        public static Color Map(ColorValue dtoColor) => (dtoColor is null) ? null : new Color(dtoColor.R, dtoColor.G, dtoColor.B, dtoColor.Alpha);

        public static DtoTextAlignmentType Map(TextAlignmentType woopecValue)
        {
            return woopecValue switch
            {
                TextAlignmentType.Center => DtoTextAlignmentType.Center,
                TextAlignmentType.Left => DtoTextAlignmentType.Left,
                TextAlignmentType.Right => DtoTextAlignmentType.Right,
                _ => throw new NotImplementedException("Unexpected TextAlignmentType")
            };
        }

        public static DtoFontStyleType Map(FontStyleType woopecValue)
        {
            return woopecValue switch
            {
                FontStyleType.Normal => DtoFontStyleType.Normal,
                FontStyleType.Italic => DtoFontStyleType.Italic,
                _ => throw new NotImplementedException("Unexpected TextAlignmentType")
            };
        }

        public static DtoFontWeightType Map(FontWeightType woopecValue)
        {
            return woopecValue switch
            {
                FontWeightType.Thin => DtoFontWeightType.Thin,
                FontWeightType.Light => DtoFontWeightType.Light,
                FontWeightType.Normal => DtoFontWeightType.Normal,
                FontWeightType.Bold => DtoFontWeightType.Bold,
                _ => throw new NotImplementedException("Unexpected TextAlignmentType")
            };
        }

        public static DtoTextStyle Map(TextStyle woopecValue)
        {
            if (woopecValue is null)
                return null;

            var dtoTextStyle = new DtoTextStyle() { 
                FontFamilyName = woopecValue.FontFamilyName,
                FontSize = woopecValue.FontSize,
                FontStyle = DtoMapper.Map(woopecValue.FontStyle),
                FontWeight = DtoMapper.Map(woopecValue.FontWeight),
                BackgroundColor = DtoMapper.Map(woopecValue.BackgroundColor),
                ForegroundColor = DtoMapper.Map(woopecValue.ForegroundColor),
            };

            return dtoTextStyle;
        }

    }


}
