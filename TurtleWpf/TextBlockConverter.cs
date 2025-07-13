using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Woopec.Graphics;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
using Woopec.Graphics.InternalDtos;

namespace Woopec.Wpf
{
    internal class TextBlockConverter
    {
        internal ScreenTextBlock _woopecTextBlock;

        public TextBlockConverter(ScreenTextBlock woopecTextBlock)
        {
            _woopecTextBlock = woopecTextBlock;
        }

        public TextBlock CreateWpfTextBlock()
        {
            var wpfTextBlock = new TextBlock();
            wpfTextBlock.Text = _woopecTextBlock.Text;
            wpfTextBlock.HorizontalAlignment = HorizontalAlignmentOf(_woopecTextBlock.Alignment);
            wpfTextBlock.FontFamily = FontFamilyOf(_woopecTextBlock.TextStyle.FontFamilyName);
            wpfTextBlock.FontSize = _woopecTextBlock.TextStyle.FontSize;
            wpfTextBlock.FontStyle = FontStyleOf(_woopecTextBlock.TextStyle.FontStyle);
            wpfTextBlock.FontWeight = FontWeightOf(_woopecTextBlock.TextStyle.FontWeight);
            wpfTextBlock.Background = BrushOf(_woopecTextBlock.TextStyle.BackgroundColor);
            wpfTextBlock.Foreground = BrushOf(_woopecTextBlock.TextStyle.ForegroundColor);
            wpfTextBlock.TextWrapping = TextWrapping.Wrap;

            return wpfTextBlock;
        }

        public Point GetUpperLeftCornerOnCanvas(Canvas canvas)
        {
            return CanvasHelpers.ConvertToCanvasPoint(_woopecTextBlock.Position, canvas);
        }

        public Vec2DValue GetLowerRightPointInWoopecCoordinates(Canvas canvas)
        {
            var tempWpfTextBlock = CreateWpfTextBlock();

            // Size determination according to an answer in https://stackoverflow.com/questions/9264398/how-to-calculate-wpf-textblock-width-for-its-known-font-size-and-characters

            // Put the text in a box of infinite size and arrange it
            tempWpfTextBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            tempWpfTextBlock.Arrange(new Rect(tempWpfTextBlock.DesiredSize));

            // Now we can read the size of the text block
            var wpfTextBlockWidth = tempWpfTextBlock.ActualWidth;
            var wpfTextBlockHeight = tempWpfTextBlock.ActualHeight;

            // position of lower right corner on canvas
            var upperLeftOnCanvas = GetUpperLeftCornerOnCanvas(canvas);
            var lowerRightOnCanvas = new Point(upperLeftOnCanvas.X + wpfTextBlockWidth, upperLeftOnCanvas.Y + wpfTextBlockHeight);
            var lowerRightOnWoopec = CanvasHelpers.ConvertToVec2DPoint(lowerRightOnCanvas, canvas);

            return lowerRightOnWoopec;

        }

        private static HorizontalAlignment HorizontalAlignmentOf(DtoTextAlignmentType woopecAlignment)
        {
            HorizontalAlignment result = woopecAlignment switch
            {
                DtoTextAlignmentType.Right => HorizontalAlignment.Right,
                DtoTextAlignmentType.Left => HorizontalAlignment.Left,
                DtoTextAlignmentType.Center => HorizontalAlignment.Center,
                _ => throw new NotImplementedException("Unexpected TextAlignmentType")
            };
            return result;
        }
        private static FontFamily FontFamilyOf(string fontFamilyName)
        {
            return new FontFamily(fontFamilyName);
        }

        private static FontStyle FontStyleOf(DtoFontStyleType woopecFontStyle)
        {
            FontStyle result = woopecFontStyle switch
            {
                DtoFontStyleType.Normal => FontStyles.Normal,
                DtoFontStyleType.Italic => FontStyles.Italic,
                _ => throw new NotImplementedException("Unexpected FontStyleType")
            };

            return result;
        }

        private static FontWeight FontWeightOf(DtoFontWeightType woopecFontWeight)
        {
            FontWeight result = woopecFontWeight switch
            {
                DtoFontWeightType.Thin => FontWeights.Thin,
                DtoFontWeightType.Light => FontWeights.Light,
                DtoFontWeightType.Normal => FontWeights.Normal,
                DtoFontWeightType.Bold => FontWeights.Bold,
                _ => throw new NotImplementedException("Unexpected FontWeightType")
            };

            return result;
        }

        private static Brush BrushOf(ColorValue color)
        {
            if (color == null)
                return null;

            var wpfColor = ColorConverter.Convert(color);

            return new SolidColorBrush(wpfColor);
        }
    }
}

