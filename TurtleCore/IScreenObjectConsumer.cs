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
        /// A Task that waits for the next object which can be given to the writer
        /// </summary>
        /// <returns></returns>
        public Task<ScreenObject> GetNextObjectForWriterAsync();

        /// <summary>
        /// Give the next object to the writer
        /// </summary>
        /// <param name="screenObject"></param>
        /// <param name="whenFinished"></param>
        public void SendNextObjectToWriter(ScreenObject screenObject);
    }
}
