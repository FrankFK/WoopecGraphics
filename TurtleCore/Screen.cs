// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    public class Screen
    {
        private readonly IScreenOutput _screenOutput;


        public Screen()
        {
            _screenOutput = TurtleOutputs.GetDefaultScreenOutput();
        }

        public void DrawLine(LineOnScreen line)
        {
            _screenOutput.DrawLine(line);
        }

        private static Screen _defaultScreen;
        internal static Screen GetDefaultScreen()
        {
            if (_defaultScreen == null)
                _defaultScreen = new Screen();
            return _defaultScreen;
        }
    }
}
