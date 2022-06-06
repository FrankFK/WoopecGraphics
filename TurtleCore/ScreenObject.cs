using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Woopec.Core
{
    /// <summary>
    /// An Object (ScreenLine, ScreenFigure, ScreenDialog) to be rendered on the screen.
    /// </summary>
    internal class ScreenObject
    {
        internal const int NoGroupId = 0;
        private int _groupId;
        private int _waitForAnimationsOfGroupId = NoGroupId;

        public int ID { get; set; }

        /// <summary>
        /// Multiple ScreenObjects can belong to a group. For instance: All ScreenObjects created by the same Turtle-instance have the same GroupId.
        /// If ScreenObjects belong to a group they are written to the screen in the same order as they are inserted into the channel.
        /// </summary>
        public int GroupID
        {
            get
            {
                return _groupId;
            }
            init
            {
                // if (value == NoGroupId)
                //    throw new ArgumentOutOfRangeException($"{NoGroupId} is not allowed for GroupID");
                _groupId = value;
            }

        }

        /// <summary>
        /// true if this screen object belongs to a group
        /// </summary>
        public bool BelongsToAGroup { get { return _groupId != NoGroupId; } }

        /// <summary>
        /// When set to a value of a GroupID: This object waits until all already produced animations of the given GroupId are finished.
        /// This can be the same group as this.GroupID, it also can be another group.
        /// </summary>
        public int WaitForAnimationsOfGroupID
        {
            get
            {
                return _waitForAnimationsOfGroupId;
            }
            set
            {
                // if (value == NoGroupId)
                //    throw new ArgumentOutOfRangeException($"{NoGroupId} is not allowed for GroupId");
                _waitForAnimationsOfGroupId = value;
            }
        }

        /// <summary>
        /// true, if this animation waits for other animations.
        /// Set to false, if this animation should not wait for other animations.
        /// </summary>
        public bool WaitsForAnimations
        {
            get
            {
                return (_waitForAnimationsOfGroupId != NoGroupId);
            }
        }


        public ScreenAnimation Animation { get; set; }

        /// <summary>
        /// true if this screen object has animations
        /// </summary>
        public bool HasAnimations { get { return (Animation != null && Animation.Effects.Count > 0); } }


        public ScreenObject()
        {
            Animation = null;
            _waitForAnimationsOfGroupId = NoGroupId;
            _groupId = NoGroupId;
        }

        #region Specific Json Serialization, because this class had derived classes
        internal enum JsonTypeDiscriminator
        {
            ScreenObject = 0,
            ScreenLine = 1,
            ScreenFigure = 2,
            ScreenDialog = 3,
        }

        internal static ScreenObject JsonRead(ref Utf8JsonReader reader, int typeDiscriminatorAsInt, JsonSerializerOptions options)
        {
            return (JsonTypeDiscriminator)typeDiscriminatorAsInt switch
            {
                JsonTypeDiscriminator.ScreenObject => (ScreenObject)JsonSerializer.Deserialize(ref reader, typeof(ScreenObject), options),
                JsonTypeDiscriminator.ScreenLine => (ScreenObject)JsonSerializer.Deserialize(ref reader, typeof(ScreenLine), options),
                JsonTypeDiscriminator.ScreenFigure => (ScreenObject)JsonSerializer.Deserialize(ref reader, typeof(ScreenFigure), options),
                JsonTypeDiscriminator.ScreenDialog => (ScreenObject)JsonSerializer.Deserialize(ref reader, typeof(ScreenDialog), options),
                _ => throw new NotSupportedException(),
            };
        }

        internal static int JsonTypeDiscriminatorAsInt(ScreenObject obj)
        {
            if (obj is ScreenLine) return (int)JsonTypeDiscriminator.ScreenLine;
            else if (obj is ScreenFigure) return (int)JsonTypeDiscriminator.ScreenFigure;
            else if (obj is ScreenDialog) return (int)JsonTypeDiscriminator.ScreenDialog;
            else if (obj is ScreenObject) return (int)JsonTypeDiscriminator.ScreenObject;
            else throw new NotSupportedException();
        }

        internal static void JsonWrite(Utf8JsonWriter writer, ScreenObject obj, JsonSerializerOptions options)
        {
            if (obj is ScreenLine line) JsonSerializer.Serialize(writer, line, options);
            else if (obj is ScreenFigure figure) JsonSerializer.Serialize(writer, figure, options);
            else if (obj is ScreenDialog dialog) JsonSerializer.Serialize(writer, dialog, options);
            else if (obj is ScreenObject screenObject) JsonSerializer.Serialize(writer, screenObject, options);
            else throw new NotSupportedException();
        }
        #endregion


    }
}
