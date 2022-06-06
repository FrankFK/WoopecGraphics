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

namespace Woopec.Wpf
{
    /// <summary>
    /// Interaction logic for TextInputWindow.xaml
    /// </summary>
    public partial class TextInputWindow : Window
    {
        /// <summary>
        /// Show an input dialog
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="question">Question</param>
        /// <param name="defaultAnswer">Default Answert</param>
        public TextInputWindow(string title, string question, string defaultAnswer = "")
        {
            InitializeComponent();
            Title = title;
            lblQuestion.Content = question;
            txtAnswer.Text = defaultAnswer;
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

    }
}
