﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// At the moment this class only contains the default canvas, which should be used if
    /// the contructor of Turtle() is called without arguments
    /// </summary>
    internal class TurtleOutputs
    {
        private static IScreenOutput s_defaultScreenOutput;

        public static void InitializeDefaultScreen(IScreenOutput screenOutput)
        {
            s_defaultScreenOutput = screenOutput;
        }

        public static IScreenOutput GetDefaultScreenOutput()
        {
            return s_defaultScreenOutput;
        }

    }
}