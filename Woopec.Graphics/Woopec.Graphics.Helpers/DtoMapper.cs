using System;
using System.Collections.Generic;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Helpers
{
    /// <summary>
    /// I do not want to have any dependencies to the data types (i.e. to Vec2D, Color, etc.) of Woopec.Graphics in the other namespaces.
    /// That's why I map these data types to Dto types.
    /// </summary>
    internal static class DtoMapper
    {
        public static DtoVec2D Map(Vec2D woopecValue) => (woopecValue is null) ? null : new DtoVec2D(woopecValue.X, woopecValue.Y);
        public static Vec2D Map(DtoVec2D dtoValue) => (dtoValue is null) ? null : new Vec2D(dtoValue.X, dtoValue.Y);

        public static List<DtoVec2D> Map(List<Vec2D> woopecValues)
        {
            if (woopecValues is null) return null;

            var result = new List<DtoVec2D>();
            foreach (var v in woopecValues)
            {
                result.Add(Map(v));
            }
            return result;
        }

        public static DtoColor Map(Color woopecColor) => (woopecColor is null) ? null : new DtoColor(woopecColor.R, woopecColor.G, woopecColor.B, woopecColor.Alpha);

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
                _ => throw new NotImplementedException("Unexpected FontStyleType")
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
                _ => throw new NotImplementedException("Unexpected FontWeightType")
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

        public static DtoShapeType Map(ShapeType woopecValue)
        {
            return woopecValue switch
            {
                ShapeType.Polygon => DtoShapeType.Polygon,
                ShapeType.Image => DtoShapeType.Image,
                ShapeType.Compound => DtoShapeType.Compound,
                _ => throw new NotImplementedException("Unexpected ShapeType")
            };
        }

        public static DtoShapeComponent Map(ShapeComponent woopecValue)
        {
            if (woopecValue is null) return null;

            return new DtoShapeComponent()
            {
                FillColor = Map(woopecValue.FillColor),
                OutlineColor = Map(woopecValue.OutlineColor),
                Polygon = Map(woopecValue.Polygon),
            };
        }

        public static List<DtoShapeComponent> Map(List<ShapeComponent> woopecValue)
        {
            if (woopecValue is null) return null;

            var result = new List<DtoShapeComponent>();
            foreach (var item in woopecValue)
            {
                result.Add(Map(item));
            }
            return result;
        }

        public static DtoShapeBase Map(ShapeBase woopecValue)
        {
            if (woopecValue is null) return null;
            if (woopecValue is Shape) return Map(woopecValue as  Shape);
            if (woopecValue is ImageShape) return Map(woopecValue as ImageShape);
            throw new NotImplementedException("Unexpected ShapeBase type");
        }

        public static DtoShape Map(Shape woopecValue)
        {
            if (woopecValue is null) return null;

            return new DtoShape(woopecValue.Name, Map(woopecValue.Type), Map(woopecValue.Components));
        }

        public static DtoImageShape Map(ImageShape woopecValue)
        {
            if (woopecValue is null) return null;

            return new DtoImageShape(woopecValue.Name, Map(woopecValue.Type), woopecValue.ImagePath);
        }
    }


}
