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

        private readonly List<ScreenAnimation> _animations = new();
        public void AddAnimation(ScreenAnimation animation)
        {
            _animations.Add(animation);
        }

    }
}
