using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using Woopec.Graphics.CommunicatedObjects;
using Woopec.Graphics.InternalFrontend;

namespace Woopec.Wpf
{
    internal class WpfScreenObjectWriter : IScreenObjectWriter
    {

        private readonly Canvas _canvas;

        private readonly CanvasLines _canvasLines;
        private readonly CanvasPathes _canvasPathes;
        private IScreenResultProducer _screenResultProducer;

        public WpfScreenObjectWriter(Canvas canvas)
        {
            _canvas = canvas;
            _canvasLines = new CanvasLines(_canvas);
            _canvasPathes = new CanvasPathes(_canvas);
        }

        public void UpdateWithAnimation(ScreenObject screenObject)
        {
            if (!screenObject.HasAnimations)
                // The screenObject only has waited for other animations to be finished. But it has no animations itself.
                // We can handle it with a normal Update:
                Update(screenObject);
            else
            {
                CanvasChildrenChange result;
                if (screenObject is ScreenLine)
                    result = _canvasLines.UpdateWithAnimation(screenObject as ScreenLine, OnAnimationIsFinished);
                else if (screenObject is ScreenFigure)
                    result = _canvasPathes.UpdateWithAnimation(screenObject as ScreenFigure, OnAnimationIsFinished);
                else
                    throw new ArgumentOutOfRangeException(nameof(screenObject), "Parameter has wrong type");

                UpdateCanvasChildren(result);
            }
        }

        public event AnimationIsFinished OnAnimationIsFinished;

        public void Update(ScreenObject screenObject)
        {
            if (screenObject is ScreenDialog dialog)
            {
                ShowDialogAndSendAnswer(dialog);
            }
            else if (screenObject is ScreenNumberDialog numberDialog)
            {
                ShowNumberDialogAndSendAnswer(numberDialog);
            }
            else if (screenObject is ScreenTextBlock textBlock)
            {
                ShowTextBlock(textBlock);
            }
            else
            {
                CanvasChildrenChange result;
                if (screenObject is ScreenFigure)
                    result = _canvasPathes.Update(screenObject as ScreenFigure);
                else if (screenObject is ScreenLine)
                    result = _canvasLines.Update(screenObject as ScreenLine);
                else
                    throw new ArgumentOutOfRangeException(nameof(screenObject), "Parameter has wrong type");

                UpdateCanvasChildren(result);

            }
        }

        public void SetScreenResultProducer(IScreenResultProducer producer)
        {
            _screenResultProducer = producer;
        }

        private void UpdateCanvasChildren(CanvasChildrenChange change)
        {
            switch (change.Operation)
            {
                case Operation.Add:
                    if (!_canvas.Children.Contains(change.Element))
                    {
                        _canvas.Children.Add(change.Element);
                    }
                    break;
                case Operation.Remove:
                    _canvas.Children.Remove(change.Element);
                    break;
                case Operation.Nothing:
                    break;
                default:
                    break;
            }
        }

        private void ShowDialogAndSendAnswer(ScreenDialog dialog)
        {
            string answer = null;

            var dialogWindow = new TextInputWindow(dialog.Title, dialog.Prompt, dialog.Position, _canvas);
            dialogWindow.Owner = Window.GetWindow(_canvas);
            if (dialogWindow.ShowDialog() == true)
            {
                answer = dialogWindow.Answer;
            }

            _screenResultProducer.SendText(answer);
        }

        private void ShowNumberDialogAndSendAnswer(ScreenNumberDialog dialog)
        {
            double? answer = null;

            var dialogWindow = new NumberInputWindow(dialog, _canvas);
            dialogWindow.Owner = Window.GetWindow(_canvas);
            if (dialogWindow.ShowDialog() == true)
            {
                answer = dialogWindow.AnswerAsDouble;
            }

            _screenResultProducer.SendNumber(answer);
        }

        private void ShowTextBlock(ScreenTextBlock textBlock)
        {
            var converter = new TextBlockConverter(textBlock);

            var wpfTextBlock = converter.CreateWpfTextBlock();
            var canvasPosition = converter.GetUpperLeftCornerOnCanvas(_canvas);

            _canvas.Children.Add(wpfTextBlock);
            Canvas.SetTop(wpfTextBlock, canvasPosition.Y);
            Canvas.SetLeft(wpfTextBlock, canvasPosition.X);

            if (textBlock.ReturnLowerRightCorner)
            {
                var lowerRightCorner = converter.GetLowerRightPointInWoopecCoordinates(_canvas);
                _screenResultProducer.SendVec2D(lowerRightCorner);
            }
        }

    }
}
