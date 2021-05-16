using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// Get screen objects from producer(s) and hand them over to a screen object writer
    /// </summary>
    internal interface IScreenObjectConsumer
    {
        /// <summary>
        /// A Task that waits for the next object and sends it to the writer
        /// </summary>
        /// <returns></returns>
        public Task HandleNextScreenObjectAsync();
    }
}
