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

        public List<ScreenAnimation> Animations { get; init; }

        public ScreenObject()
        {
            Animations = new List<ScreenAnimation>();
        }
        public void AddAnimation(ScreenAnimation animation)
        {
            Animations.Add(animation);
        }

    }
}
