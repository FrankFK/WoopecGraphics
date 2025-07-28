using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Internal.Frontend
{
    /// <summary>
    /// A channel for exchange of ScreenResults, from the writer's side
    /// </summary>
    internal interface IScreenResultChannelForWriter
    {
        /// <summary>
        /// See same method in System.Threading.Channels.ChannelWriter
        /// </summary>
        /// <param name="screenObject"></param>
        /// <returns></returns>
        public bool TryWrite(ScreenResult screenObject);

    }
}

