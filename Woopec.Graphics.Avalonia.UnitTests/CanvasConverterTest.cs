using Avalonia;
using FluentAssertions;
using Woopec.Graphics.Avalonia.Converters;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Avalonia.UnitTests
{
    public class CanvasConverterTest
    {
        [Fact]
        public void ConvertToCanvasPoint_NullVector_ToCenter()
        {
            var canvasBound = new Rect(0, 0, 400, 300);
            var woopecVector = new DtoVec2D(0, 0);
            Point result = CanvasConverter.ConvertToCanvasPoint(woopecVector, canvasBound);
            result.X.Should().Be(200);
            result.Y.Should().Be(150);
        }
    }
}
