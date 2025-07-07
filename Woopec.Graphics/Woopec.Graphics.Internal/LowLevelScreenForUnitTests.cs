using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics.LowLevelScreen;

namespace Woopec.Graphics.InternalBackend
{
    /// <summary>
    /// An instance of this class represents a screen which can be used for unit tests
    /// </summary>
    internal class LowLevelScreenForUnitTests : ILowLevelScreen
    {
        private int _lineCounter;
        private int _figureCounter;

        public LowLevelScreenForUnitTests()
        {
            LastIssuedAnimatonGroupID = ScreenObject.NoGroupId;
        }


        #region Methods of ILowLevelScreen

        ///<inheritdoc/>
        public int LastIssuedAnimatonGroupID { get; set; }

        ///<inheritdoc/>
        public int CreateLine()
        {
            _lineCounter++;
            return _lineCounter - 1;
        }

        ///<inheritdoc/>
        public void DrawLine(ScreenLine line)
        {
            UpdateLastIssuedAnimationGroupID(line);
        }

        ///<inheritdoc/>
        public int CreateFigure()
        {
            _figureCounter++;
            return _figureCounter - 1;
        }

        ///<inheritdoc/>
        public void UpdateFigure(ScreenFigure figure)
        {
            UpdateLastIssuedAnimationGroupID(figure);
        }

        ///<inheritdoc/>
        public async Task<string> TextInputAsync(string title, string prompt, Vec2D position)
        {
            await Task.Delay(1);
            throw new NotImplementedException("With LowLevelScreenForUnitTests TextInput is not usable.");
        }

        ///<inheritdoc/>
        public async Task<double?> NumberInputAsync(ScreenNumberDialog dialog)
        {
            await Task.Delay(1);
            throw new NotImplementedException("With LowLevelScreenForUnitTests NumberInput is not usable.");
        }



        #endregion

        private void UpdateLastIssuedAnimationGroupID(ScreenObject screenObject)
        {
            if (screenObject.HasAnimations)
            {
                LastIssuedAnimatonGroupID = screenObject.GroupID;
            }
        }

        public void ShowTextBlock(ScreenTextBlock textBlock)
        {
            throw new NotImplementedException();
        }

        public Task<Vec2D> ShowTextBlockWithReturnCoordinateAsync(ScreenTextBlock textBlock)
        {
            throw new NotImplementedException();
        }
    }
}
