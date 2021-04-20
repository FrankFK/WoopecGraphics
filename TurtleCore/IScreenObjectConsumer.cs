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
        public Task<ScreenObject> GetNextObjectForWriterAsync();

        public void SendNextObjectToWriter(ScreenObject screenObject, Action<int> whenFinished);

    }
}
