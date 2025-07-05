using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Animation;
using Avalonia.Animation.Easings;

namespace AvaloniaTestConsole.WoopecCoreConnection
{
    internal class AnimationCompletedEasing : Easing
    {
        private readonly Animation _animation;
        private bool _ended;

        public AnimationCompletedEasing(Animation animation)
        {
            _animation = animation;
            _ended = false;
        }

        public event EventHandler Completed;

        public override double Ease(double progress)
        {
            if (progress >= 1.0 && !_ended)
            {
                Completed?.Invoke(_animation, new EventArgs());

                _ended = true; // Ease may be called multiple times at the end
            }

            return progress;
        }
    }
}
