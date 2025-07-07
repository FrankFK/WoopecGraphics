using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.LowLevelScreen;

namespace Woopec.Graphics.InternalBackend
{
    internal class ScreenResultConsumer : IScreenResultConsumer
    {
        private readonly IScreenResultChannelForReader _screenResultChannel;

        public ScreenResultConsumer(IScreenResultChannelForReader screenResultChannel)
        {
            _screenResultChannel = screenResultChannel;
        }

        public async Task<string> ReadTextResultAsync()
        {
            ScreenResultText screenResultText = null;
            try
            {
                var screenResult = await _screenResultChannel.ReadAsync();
                screenResultText = screenResult as ScreenResultText;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ScreenResultConsumer ReadTextResultAsync: Exception " + ex.Message);
                throw;
            }
            return screenResultText.Text;
        }

        public async Task<double?> ReadNumberResultAsync()
        {
            ScreenResultNumber screenResultNumber = null;
            try
            {
                var screenResult = await _screenResultChannel.ReadAsync();
                screenResultNumber = screenResult as ScreenResultNumber;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ScreenResultConsumer ReadNumberResultAsync: Exception " + ex.Message);
                throw;
            }
            return screenResultNumber.Value;
        }

        public async Task<Vec2D> ReadVec2DResultAsync()
        {
            ScreenResultVec2D screenResultVec2D = null;
            try
            {
                var screenResult = await _screenResultChannel.ReadAsync();
                screenResultVec2D = screenResult as ScreenResultVec2D;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ScreenResultConsumer ReadVec2DResultAsync: Exception " + ex.Message);
                throw;
            }
            return screenResultVec2D.Value;
        }

    }
}
