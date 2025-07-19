using System.Threading;
using System.Threading.Tasks;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Internal.Backend
{
    /// <summary>
    /// A channel for exchange of ScreenResults, from the reader's side
    /// </summary>
    internal interface IScreenResultChannelForReader
    {
        /// <summary>
        /// See same method in System.Threading.Channels.ChannelReader
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public ValueTask<ScreenResult> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
