using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Woopec.Graphics.CommunicatedObjects
{
    /// <summary>
    /// Some kind of information which is received from the Screen
    /// </summary>
    internal class ScreenResult
    {
        #region Specific Json Serialization, because this class had derived classes
        internal enum JsonTypeDiscriminator
        {
            ScreenResult = 0,
            ScreenResultText = 1,
            ScreenResultNumber = 2,
            ScreenResultVec2D = 3,
        }

        internal static ScreenResult JsonRead(ref Utf8JsonReader reader, int typeDiscriminatorAsInt, JsonSerializerOptions options)
        {
            return (JsonTypeDiscriminator)typeDiscriminatorAsInt switch
            {
                JsonTypeDiscriminator.ScreenResult => (ScreenResult)JsonSerializer.Deserialize(ref reader, typeof(ScreenResult), options),
                JsonTypeDiscriminator.ScreenResultText => (ScreenResultText)JsonSerializer.Deserialize(ref reader, typeof(ScreenResultText), options),
                JsonTypeDiscriminator.ScreenResultNumber => (ScreenResultNumber)JsonSerializer.Deserialize(ref reader, typeof(ScreenResultNumber), options),
                JsonTypeDiscriminator.ScreenResultVec2D => (ScreenResultVec2D)JsonSerializer.Deserialize(ref reader, typeof(ScreenResultVec2D), options),
                _ => throw new NotSupportedException(),
            };
        }

        internal static int JsonTypeDiscriminatorAsInt(ScreenResult obj)
        {
            if (obj is ScreenResultText) return (int)JsonTypeDiscriminator.ScreenResultText;
            else if (obj is ScreenResultNumber) return (int)JsonTypeDiscriminator.ScreenResultNumber;
            else if (obj is ScreenResultVec2D) return (int)JsonTypeDiscriminator.ScreenResultVec2D;
            else if (obj is ScreenResult) return (int)JsonTypeDiscriminator.ScreenResult;
            else throw new NotSupportedException();
        }

        internal static void JsonWrite(Utf8JsonWriter writer, ScreenResult obj, JsonSerializerOptions options)
        {
            if (obj is ScreenResultText text) JsonSerializer.Serialize(writer, text, options);
            else if (obj is ScreenResultNumber number) JsonSerializer.Serialize(writer, number, options);
            else if (obj is ScreenResultVec2D vec2D) JsonSerializer.Serialize(writer, vec2D, options);
            else if (obj is ScreenResult result) JsonSerializer.Serialize(writer, result, options);
            else throw new NotSupportedException();
        }
        #endregion


    }
}
