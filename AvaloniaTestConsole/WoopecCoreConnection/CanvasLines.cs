using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using Avalonia.Controls;
using Avalonia.Media;
using Woopec.Core;
using Avalonia.Animation;
using Avalonia.Styling;
using AvaloniaTestConsole.Views;

namespace AvaloniaTestConsole.WoopecCoreConnection
{
    internal class CanvasLines
    {
        private readonly Canvas _canvas;

        private readonly List<Line> _lines;

        public CanvasLines(Canvas canvas)
        {
            _canvas = canvas;
            _lines = new();
        }

        public CanvasChildrenChange UpdateWithAnimation(ScreenLine screenLine, AnimationIsFinished finishedEvent)
        {
            var line = CreateLine(screenLine);

            var animation = screenLine.Animation;
            var effect = animation.Effects[0] as ScreenAnimationMovement;
            if (effect.AnimatedProperty != ScreenAnimationMovementProperty.Point2)
                throw new NotImplementedException();

            var canvasStartPoint = CanvasHelpers.ConvertToCanvasPoint(effect.StartValue, _canvas);

            var keyFrame1 = new KeyFrame()
            {
                Setters = { new Setter(Line.EndPointProperty, line.StartPoint) },
                Cue = new Cue(0.0),
            };

            var keyFrame2 = new KeyFrame()
            {
                Setters = { new Setter(Line.EndPointProperty, line.EndPoint) },
                Cue = new Cue(1.0),
            };

            var avaloniaAnimation = new Animation()
            {
                Children = { keyFrame1, keyFrame2 },
                Duration = TimeSpan.FromMilliseconds(effect.Milliseconds),
            };

            var easing = new AnimationCompletedEasing(avaloniaAnimation);
            easing.Completed += (sender, args) => finishedEvent(screenLine.GroupID, screenLine.ID);

            var style = new Style(x => x.OfType<Line>());   // Idea from  repos\Avalonia\tests\Avalonia.Base.UnitTests\Styling\StyleTests.cs
            style.Animations.Add(avaloniaAnimation);
            line.Styles.Add(style);

            return new(Operation.Add, line);
        }

        public CanvasChildrenChange Update(ScreenLine screenLine)
        {
            var line = CreateLine(screenLine);

            return new(Operation.Add, line);
        }

        private Line CreateLine(ScreenLine screenLine)
        {
            while (_lines.Count <= screenLine.ID)
            {
                _lines.Add(new Line());
            }

            var line = _lines[screenLine.ID];
            line.Stroke = new SolidColorBrush(ColorConverter.Convert(screenLine.Color));

            var canvasPoint1 = CanvasHelpers.ConvertToCanvasPoint(screenLine.Point1, _canvas);
            var canvasPoint2 = CanvasHelpers.ConvertToCanvasPoint(screenLine.Point2, _canvas);
            line.StartPoint = canvasPoint1;
            line.EndPoint = canvasPoint2;
            line.StrokeThickness = 1;

            return line;
        }
    }
}
