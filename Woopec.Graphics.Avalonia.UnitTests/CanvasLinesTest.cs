using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using FluentAssertions;
using Woopec.Graphics.Avalonia.CanvasContent;
using Woopec.Graphics.Avalonia.Converters;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Avalonia.UnitTests
{
    public class CanvasMock : ICanvas
    {
        private readonly Rect _bounds;
        public CanvasMock(Rect bounds)
        {
            _bounds = bounds;
        }
        public Rect Bounds => _bounds;
    }
    public class CanvasLinesTest
    {
        [Fact]
        public void ConvertToCanvasPoint_NullVector_ToCenter()
        {
            int width = 400;
            int height = 300;
            var canvasMock = new CanvasMock(new Rect(0, 0, width, height));

            var sut = new CanvasLines(canvasMock);

            int plusX = 20;
            int plusY = 100;
            var screenLine = new ScreenLine() { Point1 = new DtoVec2D(0, 0), Point2 = new DtoVec2D(plusX, plusY), Color = new DtoColor(1, 2, 3, 4) };

            // Act
            CanvasChildrenChange change = sut.Update(screenLine);

            // Assert
            change.Operation.Should().Be(CanvasOperation.Add);
            var line = change.Element as Line;
            line.StartPoint.X.Should().Be(width / 2);
            line.StartPoint.Y.Should().Be(height / 2);
            line.EndPoint.X.Should().Be(width / 2 + plusX);
            line.EndPoint.Y.Should().Be(height / 2 - plusY);

        }
    }

}
