using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Woopec.Graphics.InternalDtos;

namespace Woopec.Wpf
{
    /// <summary>
    /// Interaction logic for NumberInputWindow.xaml
    /// </summary>
    public partial class NumberInputWindow : Window
    {
        private readonly ScreenNumberDialog _dialogInfo;
        private double? _answerAsDouble;
        private readonly Canvas _canvas;

        internal NumberInputWindow(ScreenNumberDialog dialogInfo, Canvas canvas)
        {
            _dialogInfo = dialogInfo;
            _answerAsDouble = null;
            _canvas = canvas;
            InitializeComponent();
            Title = dialogInfo.Title;
            lblQuestion.Content = dialogInfo.Prompt;
            if (dialogInfo.DefaultValue != null)
            {
                switch (dialogInfo.ReturnType)
                {
                    case ScreenNumberDialog.NumberType.Integer:
                        var defaultValue = (int)dialogInfo.DefaultValue;
                        txtAnswer.Text = defaultValue.ToString();
                        break;
                    case ScreenNumberDialog.NumberType.Double:
                        txtAnswer.Text = dialogInfo.DefaultValue.ToString();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// The Answer
        /// </summary>
        public double? AnswerAsDouble { get { return _answerAsDouble; } }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (CheckAnswer())
            {
                DialogResult = true;
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        private bool CheckAnswer()
        {
            switch (_dialogInfo.ReturnType)
            {
                case ScreenNumberDialog.NumberType.Integer:
                    return CheckInt();
                case ScreenNumberDialog.NumberType.Double:
                    return CheckDouble();
                default:
                    return false;
            }
        }

        private bool CheckDouble()
        {
            double answerAsDouble;
            double sampleValue = 3.14;

            // The text is parsed according to the current culture (and not CultureInvariant), because this is easier for the users (for instance in germany).
            if (!double.TryParse(txtAnswer.Text, out answerAsDouble))
            {
                MessageBox.Show($"The input must be a number. Example of a correct input: {sampleValue}.", "Not a number", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (_dialogInfo.MinValue != null && answerAsDouble < _dialogInfo.MinValue)
            {
                MessageBox.Show($"The allowed minimal value is {(double)_dialogInfo.MinValue}. Try again.", "Too small", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (_dialogInfo.MaxValue != null && answerAsDouble > _dialogInfo.MaxValue)
            {
                MessageBox.Show($"The allowed maximal value is {(double)_dialogInfo.MaxValue}. Try again.", "Too large", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            _answerAsDouble = answerAsDouble;
            return true;
        }

        private bool CheckInt()
        {
            int answerAsInt;
            if (!int.TryParse(txtAnswer.Text, out answerAsInt))
            {
                MessageBox.Show("The input must be an integer.", "Not a number", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (_dialogInfo.MinValue != null && answerAsInt < _dialogInfo.MinValue)
            {
                MessageBox.Show($"The allowed minimal value is {(int)_dialogInfo.MinValue}. Try again.", "Too small", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (_dialogInfo.MaxValue != null && answerAsInt > _dialogInfo.MaxValue)
            {
                MessageBox.Show($"The allowed maximal value is {(int)_dialogInfo.MaxValue}. Try again.", "Too large", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            _answerAsDouble = answerAsInt;
            return true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_dialogInfo.Position != null)
            {
                CanvasHelpers.SetLowerLeftCornerOfWindowToCanvasPoint(_dialogInfo.Position, _canvas, this);
            }
        }
    }
}
