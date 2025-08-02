using Woopec.Graphics.Avalonia.CanvasContent;
using Woopec.Graphics.Interface.Dtos;
using Woopec.Graphics.Internal.Frontend;

namespace Woopec.Graphics.Avalonia
{
    internal class CanvasAccess : ICanvas
    {
        private readonly Canvas _canvas;
        public CanvasAccess(Canvas canvas)
        {
            _canvas = canvas;
        }
        public Rect Bounds => _canvas.Bounds;

    }
    internal class AvaloniaScreenObjectWriter : IScreenObjectWriter
    {
        private readonly Canvas _canvas;
        private IScreenResultProducer _screenResultProducer;    // The data entered by the user in a dialog is sent back to this object.
        private readonly CanvasLines _canvasLines;

        public AvaloniaScreenObjectWriter(Canvas canvas)
        {
            _canvas = canvas;
            _canvasLines = new CanvasLines(new CanvasAccess(_canvas));
        }

        #region IScreenObjectWriter Implementation

        public event AnimationIsFinished OnAnimationIsFinished;

        public void SetScreenResultProducer(IScreenResultProducer producer)
        {
            _screenResultProducer = producer;
        }

        public void Update(ScreenObject screenObject)
        {
            if (screenObject is ScreenDialog dialog)
            {
                throw new NotImplementedException("Updata ScreenDialog");
            }
            else if (screenObject is ScreenNumberDialog numberDialog)
            {
                throw new NotImplementedException("Updata ScreenNumberDialog");
            }
            else if (screenObject is ScreenTextBlock textBlock)
            {
                throw new NotImplementedException("Updata ScreenTextBlock");
            }
            else
            {
                CanvasChildrenChange result;
                if (screenObject is ScreenFigure)
                    throw new NotImplementedException("Updata ScreenFigure");
                else if (screenObject is ScreenLine)
                    result = _canvasLines.Update(screenObject as ScreenLine);
                else
                    throw new ArgumentOutOfRangeException(nameof(screenObject), "Parameter has wrong type");

                UpdateCanvasChildren(result);

            }
        }

        public void UpdateWithAnimation(ScreenObject screenObject)
        {
            throw new NotImplementedException();
        }
        #endregion


        private void UpdateCanvasChildren(CanvasChildrenChange change)
        {
            switch (change.Operation)
            {
                case CanvasOperation.Add:
                    if (!_canvas.Children.Contains(change.Element))
                    {
                        _canvas.Children.Add(change.Element);
                    }
                    break;
                case CanvasOperation.Remove:
                    _canvas.Children.Remove(change.Element);
                    break;
                case CanvasOperation.Nothing:
                    break;
                default:
                    break;
            }
        }

    }


}
