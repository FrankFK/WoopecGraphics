using FluentAssertions;
using Woopec.Graphics.Avalonia.Converters;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Avalonia.UnitTests
{
    public class ColorConverterTest
    {
        [Fact]
        public void Convert_Red_IsOk()
        {
            int red = 0;
            int green = 255;
            int blue = 120;
            int alpha = 10;
            var dtoColor = new DtoColor(red, green, blue, alpha);
            var result = ColorConverter.ConvertDto(dtoColor);
            Convert.ToInt32(result.R).Should().Be(red);
            Convert.ToInt32(result.G).Should().Be(green);
            Convert.ToInt32(result.B).Should().Be(blue);
            Convert.ToInt32(result.A).Should().Be(alpha);
        }
    }
}
