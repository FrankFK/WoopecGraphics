using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// Base-Class for ScreenLine and ScreenForm
    /// </summary>
    public class ScreenObject
    {
        public int ID { get; set; }

        public ScreenAnimation Animation { get; set; }
        public bool HasAnimation { get { return Animation != null; } }

        public ScreenObject()
        {
            Animation = null;
        }

    }
}
