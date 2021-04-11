// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TurtleCore;

namespace TurtleWpf
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TurtleCanvas : UserControl
    {
        private readonly Canvas _canvas;
        private readonly ScreenOutput _turtleScreenOutput;

        public TurtleCanvas()
        {
            InitializeComponent();
            _canvas = new Canvas() { Width = 400, Height = 400 };
            this.Content = _canvas;

            _turtleScreenOutput = new ScreenOutput(_canvas);
            TurtleOutputs.InitializeDefaultScreen(_turtleScreenOutput);
        }
    }
}
