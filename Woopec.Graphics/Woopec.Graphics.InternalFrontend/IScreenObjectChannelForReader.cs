using System.Threading;
using System.Threading.Tasks;
using Woopec.Graphics.LowLevelScreen;

namespace Woopec.Graphics.InternalFrontend
{
    /// <summary>
    /// Communication channel for ScreenObjectConsumer
    /// </summary>
    internal interface IScreenObjectChannelForReader
    {
        /// <summary>
        /// See same method in System.Threading.Channels.ChannelReader
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public ValueTask<ScreenObject> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
