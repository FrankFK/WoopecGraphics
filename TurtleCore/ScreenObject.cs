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
                _groupId = value;
            }

        }

        /// <summary>
        /// true if this screen object belongs to a group
        /// </summary>
        public bool BelongsToAGroup { get { return _groupId != NoGroupId; } }

        /// <summary>
        /// If false: The drawing of this object starts after all older drawings of this group are started, but it does not wait until all other drawings of its group are finished (and WaitForCompletedAnimationOfGroup does not force it to wait longer)
        /// If true:  The drawing of this object waits until all older drawings of its group are finished (and WaitForCompletedAnimationOfGroup does not force it to wait longer)
        /// </summary>
        public bool WaitForCompletedAnimationsOfSameGroup { get; set; }

        /// <summary>
        /// If equal to NoGroupId: The drawing of this object is only controlled by WaitForAnimationsOfSameGroup
        /// If different to NoGroupId: The drawing of this object waits until all older drawings of another group with the given GroupId are finished (and WaitForAnimationsOfSameGroup does not force it to wait longer)
        /// </summary>
        public int WaitForCompletedAnimationsOfAnotherGroup { get; set; }



        /// <summary>
        /// true, if this animation waits for other animations.
        /// </summary>
        public bool WaitsForAnimations
        {
            get
            {
                return (WaitForCompletedAnimationsOfSameGroup || WaitForCompletedAnimationsOfAnotherGroup != NoGroupId);
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
            WaitForCompletedAnimationsOfAnotherGroup = NoGroupId;
            WaitForCompletedAnimationsOfSameGroup = false;
            _groupId = NoGroupId;
        }

        public string AnimationInfoForDebugger()
        {
            var part1 = $"{GroupID},{ID}:{(Animation != null ? Animation.Milliseconds : 0)}";
            if (WaitsForAnimations)
            {
                if (WaitForCompletedAnimationsOfAnotherGroup != ScreenObject.NoGroupId)
                {
                    if (WaitForCompletedAnimationsOfSameGroup)
                    {
                        // Waits for animations of same group and for animations of another group
                        return $"??({WaitForCompletedAnimationsOfAnotherGroup}){part1}";
                    }
                    else
                    {
                        // Waits only for animations of another group
                        return $"?({WaitForCompletedAnimationsOfAnotherGroup}){part1}";
                    }
                }
                else
                {
                    // Waits only for animations of the same group
                    return $"?{part1}";
                }
            }
            else
            {
                return part1;
            }
        }


        #region Specific Json Serialization, because this class has derived classes
        internal enum JsonTypeDiscriminator
        {
            ScreenObject = 0,
            ScreenLine = 1,
            ScreenFigure = 2,
            ScreenDialog = 3,
            ScreenNumberDialog = 4,
        }

        internal static ScreenObject JsonRead(ref Utf8JsonReader reader, int typeDiscriminatorAsInt, JsonSerializerOptions options)
        {
            return (JsonTypeDiscriminator)typeDiscriminatorAsInt switch
            {
                JsonTypeDiscriminator.ScreenObject => (ScreenObject)JsonSerializer.Deserialize(ref reader, typeof(ScreenObject), options),
                JsonTypeDiscriminator.ScreenLine => (ScreenObject)JsonSerializer.Deserialize(ref reader, typeof(ScreenLine), options),
                JsonTypeDiscriminator.ScreenFigure => (ScreenObject)JsonSerializer.Deserialize(ref reader, typeof(ScreenFigure), options),
                JsonTypeDiscriminator.ScreenDialog => (ScreenObject)JsonSerializer.Deserialize(ref reader, typeof(ScreenDialog), options),
                JsonTypeDiscriminator.ScreenNumberDialog => (ScreenObject)JsonSerializer.Deserialize(ref reader, typeof(ScreenNumberDialog), options),
                _ => throw new NotSupportedException(),
            };
        }

        internal static int JsonTypeDiscriminatorAsInt(ScreenObject obj)
        {
            if (obj is ScreenLine) return (int)JsonTypeDiscriminator.ScreenLine;
            else if (obj is ScreenFigure) return (int)JsonTypeDiscriminator.ScreenFigure;
            else if (obj is ScreenDialog) return (int)JsonTypeDiscriminator.ScreenDialog;
            else if (obj is ScreenNumberDialog) return (int)JsonTypeDiscriminator.ScreenNumberDialog;
            else if (obj is ScreenObject) return (int)JsonTypeDiscriminator.ScreenObject;
            else throw new NotSupportedException();
        }

        internal static void JsonWrite(Utf8JsonWriter writer, ScreenObject obj, JsonSerializerOptions options)
        {
            if (obj is ScreenLine line) JsonSerializer.Serialize(writer, line, options);
            else if (obj is ScreenFigure figure) JsonSerializer.Serialize(writer, figure, options);
            else if (obj is ScreenDialog dialog) JsonSerializer.Serialize(writer, dialog, options);
            else if (obj is ScreenNumberDialog numberDialog) JsonSerializer.Serialize(writer, numberDialog, options);
            else if (obj is ScreenObject screenObject) JsonSerializer.Serialize(writer, screenObject, options);
            else throw new NotSupportedException();
        }
        #endregion


    }
}
