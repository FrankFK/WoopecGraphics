using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.Internal.Backend
{
    /// <summary>
    /// Communication channel for ScreenObjectProducer
    /// </summary>
    internal interface IScreenObjectChannelForWriter
    {
        /// <summary>
        /// See same method in System.Threading.Channels.ChannelWriter
        /// </summary>
        /// <param name="screenObject"></param>
        /// <returns></returns>
        public bool TryWrite(ScreenObject screenObject);

    }

}

