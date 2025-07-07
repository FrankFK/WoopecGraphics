using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Woopec.Graphics.InternalCommunication
{
    /// <summary>
    /// An instance of this class serializes and deserializes objects of type T such that they can be transferred through the <c>ServerProcessChannel</c> and
    /// <c>ClientProcessChannel</c>.
    /// This Converter has to be used for all polymorphic base-classes (e.g. ShapeBase has two derived classes Shape and ShapeImage)
    /// </summary>
    /// <typeparam name="T">Baseclass</typeparam>
    internal class ProcessChannelConverter<T> : JsonConverter<T>
    {
        public delegate int TypeAsIntDelegate(T obj);
        public delegate T ReaderDelegate(ref Utf8JsonReader reader, int typeDiscriminatorAsIn, JsonSerializerOptions options);
        public delegate void WriterDelegate(Utf8JsonWriter writer, T value, JsonSerializerOptions options);


        private readonly TypeAsIntDelegate _typeAsIntDelegate;
        private readonly WriterDelegate _writerDelegate;
        private readonly ReaderDelegate _readerDelegate;

        private static readonly string s_typeDiscriminator = "TypeDiscriminator";
        private static readonly string s_typeValue = "TypeValue";


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeAsInt">A method which returns the type of the derived class as an int</param>
        /// <param name="writer">A method which serializes the class as the right derived class</param>
        /// <param name="reader">A method which deserializes the class as the right derived class</param>
        public ProcessChannelConverter(TypeAsIntDelegate typeAsInt, WriterDelegate writer, ReaderDelegate reader)
        {
            _typeAsIntDelegate = typeAsInt;
            _writerDelegate = writer;
            _readerDelegate = reader;
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type type)
        {
            return typeof(T).IsAssignableFrom(type);
            // return typeof(T) == type;
        }

        /// <inheritdoc/>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var typeDiscriminatorAsInt = ReadToStartOfNextTypeValue(ref reader);
            var newOptions = CreateOptionsWithoutThisConverter(options);
            var result = _readerDelegate(ref reader, typeDiscriminatorAsInt, newOptions);
            ReadEndOfTypeValue(ref reader);
            return result;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            var typeAsInt = _typeAsIntDelegate(value);

            writer.WriteNumber(s_typeDiscriminator, typeAsInt);
            writer.WritePropertyName(s_typeValue);

            // Tricky: The value (e.g. a ScreenObject) contains polymorphic properties  (e.g. ShapeBase). The custom converters
            // for the polymorphic classes of this properties are contained in the options. Therefore the _writerDelegate has
            // to use these options. But if the _writerDelegate uses the converter we are in right now, we get an endless loop.
            // Therefore this converter is removed:
            var newOptions = CreateOptionsWithoutThisConverter(options);

            _writerDelegate(writer, value, newOptions);

            writer.WriteEndObject();
        }

        private static int ReadToStartOfNextTypeValue(ref Utf8JsonReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            if (!reader.Read()
                    || reader.TokenType != JsonTokenType.PropertyName
                    || reader.GetString() != s_typeDiscriminator)
            {
                throw new JsonException();
            }

            if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            var typeDiscriminatorAsInt = reader.GetInt32();

            if (!reader.Read() || reader.GetString() != s_typeValue)
            {
                throw new JsonException();
            }
            if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            return typeDiscriminatorAsInt;
        }
        private static void ReadEndOfTypeValue(ref Utf8JsonReader reader)
        {
            if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }
        }

        /// <summary>
        /// If we do not delete the actual converter from the options we will get an endless recursion.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private JsonSerializerOptions CreateOptionsWithoutThisConverter(JsonSerializerOptions options)
        {
            var newOptions = new JsonSerializerOptions(options);
            foreach (var converter in newOptions.Converters)
            {
                if (converter.GetType() == typeof(ProcessChannelConverter<T>))
                {
                    newOptions.Converters.Remove(converter);
                    break;
                }
            }

            return newOptions;
        }

    }
}
