using System;
using System.Collections.Generic;
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
using Woopec.Graphics;
using Woopec.Graphics.InternalDtos;

namespace Woopec.Wpf
{
    /// <summary>
    /// Interaction logic for TextInputWindow.xaml
    /// </summary>
    public partial class TextInputWindow : Window
    {
        private readonly Vec2DValue _position;
        private readonly Canvas _canvas;

        /// <summary>
        /// Show an input dialog
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="question">Question</param>
        /// <param name="position">(optional).  Approximate position of the lower left corner of the dialog window</param>
        /// <param name="canvas">canvas of the position</param>
        public TextInputWindow(string title, string question, Vec2DValue position, Canvas canvas)
        {
            InitializeComponent();
            Title = title;
            lblQuestion.Content = question;
            _position = position;
            _canvas = canvas;
        }

        /// <summary>
        /// The Answer
        /// </summary>
        public string Answer { get { return txtAnswer.Text; } }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_position != null)
            {
                CanvasHelpers.SetLowerLeftCornerOfWindowToCanvasPoint(_position, _canvas, this);
            }
        }
    }
}
