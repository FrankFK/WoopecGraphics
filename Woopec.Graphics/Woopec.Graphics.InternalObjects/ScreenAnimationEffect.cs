using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Woopec.Graphics.InternalObjects
{
    /// <summary>
    /// Base class for ScreenAnimationMovement and so on.
    /// </summary>
    internal class ScreenAnimationEffect
    {
        /// <summary>
        /// Duration of the Effect
        /// </summary>
        public int Milliseconds { get; set; }

        #region Specific Json Serialization, because this class had derived classes
        internal enum JsonTypeDiscriminator
        {
            ScreenAnimationEffect = 0,
            ScreenAnimationMovement = 1,
            ScreenAnimationRotation = 2
        }

        internal static ScreenAnimationEffect JsonRead(ref Utf8JsonReader reader, int typeDiscriminatorAsInt, JsonSerializerOptions options)
        {
            return (JsonTypeDiscriminator)typeDiscriminatorAsInt switch
            {
                JsonTypeDiscriminator.ScreenAnimationEffect => (ScreenAnimationEffect)JsonSerializer.Deserialize(ref reader, typeof(ScreenAnimationEffect), options),
                JsonTypeDiscriminator.ScreenAnimationMovement => (ScreenAnimationMovement)JsonSerializer.Deserialize(ref reader, typeof(ScreenAnimationMovement), options),
                JsonTypeDiscriminator.ScreenAnimationRotation => (ScreenAnimationRotation)JsonSerializer.Deserialize(ref reader, typeof(ScreenAnimationRotation), options),
                _ => throw new NotSupportedException(),
            };
        }

        internal static int JsonTypeDiscriminatorAsInt(ScreenAnimationEffect obj)
        {
            if (obj is ScreenAnimationMovement) return (int)JsonTypeDiscriminator.ScreenAnimationMovement;
            else if (obj is ScreenAnimationRotation) return (int)JsonTypeDiscriminator.ScreenAnimationRotation;
            else if (obj is ScreenAnimationEffect) return (int)JsonTypeDiscriminator.ScreenAnimationEffect;
            else throw new NotSupportedException();
        }

        internal static void JsonWrite(Utf8JsonWriter writer, ScreenAnimationEffect obj, JsonSerializerOptions options)
        {
            if (obj is ScreenAnimationMovement movement) JsonSerializer.Serialize(writer, movement, options);
            else if (obj is ScreenAnimationRotation rotation) JsonSerializer.Serialize(writer, rotation, options);
            else if (obj is ScreenAnimationEffect effect) JsonSerializer.Serialize(writer, effect, options);
            else throw new NotSupportedException();
        }
        #endregion


    }
}
