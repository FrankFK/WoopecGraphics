using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Woopec.Graphics.InternalDtos
{
    /// <summary>
    /// For future use: Different shape types
    /// </summary>
    public enum DtoShapeType
    {
        /// <summary>
        /// Shape conists of one polygon
        /// </summary>
        Polygon,
        /// <summary>
        /// Shape is an image
        /// </summary>
        Image,
        /// <summary>
        /// Shape can contain more than one polygon
        /// </summary>
        Compound
    }

    /// <summary>
    /// Base class for different types of Shapes
    /// </summary>
    public abstract class DtoShapeBase
    {
        /// <summary>
        /// A shape can have a name. 
        /// All predefined shapes (Turtle, Classic and so on) have a name ("turtle", "classic" and so on).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Shape type
        /// </summary>
        public DtoShapeType Type { get; protected set; }

        #region Specific Json Serialization, because this class had derived classes
        internal enum JsonTypeDiscriminator
        {
            ShapeBase = 0,
            Shape = 1,
            ImageShape = 2
        }

        internal static DtoShapeBase JsonRead(ref Utf8JsonReader reader, int typeDiscriminatorAsInt, JsonSerializerOptions options)
        {
            return (JsonTypeDiscriminator)typeDiscriminatorAsInt switch
            {
                JsonTypeDiscriminator.Shape => (DtoShape)JsonSerializer.Deserialize(ref reader, typeof(DtoShape), options),
                JsonTypeDiscriminator.ImageShape => (DtoImageShape)JsonSerializer.Deserialize(ref reader, typeof(DtoImageShape), options),
                _ => throw new NotSupportedException(),
            };
        }

        internal static int JsonTypeDiscriminatorAsInt(DtoShapeBase obj)
        {
            if (obj is DtoShape) return (int)JsonTypeDiscriminator.Shape;
            else if (obj is DtoImageShape) return (int)JsonTypeDiscriminator.ImageShape;
            else throw new NotSupportedException();
        }

        internal static void JsonWrite(Utf8JsonWriter writer, DtoShapeBase obj, JsonSerializerOptions options)
        {
            if (obj is DtoShape shape) JsonSerializer.Serialize(writer, shape, options);
            else if (obj is DtoImageShape imageShape) JsonSerializer.Serialize(writer, imageShape, options);
            else throw new NotSupportedException();
        }
        #endregion


    }

    /// <summary>
    /// An instance of this class is one of the following
    /// - A polygon (fillcolor and outline color are specified by the turtle class)
    /// - A list of polygons (with fillcolor and outline color)
    /// </summary>
    internal class DtoShape : DtoShapeBase
    {
        public DtoShape()
        {
            Type = DtoShapeType.Polygon;
            _components = new();
        }

        public DtoShape(string name, DtoShapeType type, List<DtoShapeComponent> components)
        {
            Name = name;
            Type = type;
            _components = components;
        }
        /// <summary>
        /// Only for internal purposes!
        /// </summary>
        /// <remarks>
        /// Json-Serialization in <c>ProcessChannelConverter</c> does not work if the property is internal.
        /// </remarks>
        [JsonInclude]
        public List<DtoShapeComponent> Components { get { return _components; } set { _components = value; } }

        // null if Shape is an Image
        private List<DtoShapeComponent> _components;

    }

    /// <summary>
    /// One part of a complex shape
    /// </summary>
    internal class DtoShapeComponent
    {
        /// <summary>
        /// Coordinates of the polygon. For example: new() { (0,0),(10,-5),(0,10),(-10,-5) } 
        /// </summary>
        public List<DtoVec2D> Polygon { get; set; }

        /// <summary>
        /// Color the polygon will be filled with
        /// </summary>
        public DtoColor FillColor { get; set; }

        /// <summary>
        /// Color for the polygons outline
        /// </summary>
        public DtoColor OutlineColor { get; set; }
    }


    /// <summary>
    /// An instance of this class is an image which can be used as a shape
    /// </summary>
    /// <remarks>
    /// Currently only Shapes with one polygon are rendered correctly. Therefore this method is internal
    /// </remarks>
    internal class DtoImageShape : DtoShapeBase
    {
        public DtoImageShape()
        {
            Type = DtoShapeType.Image;
        }
        public DtoImageShape(string name, DtoShapeType type, string imagePath)
        {
            Name = name;
            Type = type;
            _imagePath = imagePath;
        }


        // Only to be used from TurtleWpf
        public string ImagePath { get { return _imagePath; } set { _imagePath = value; } }

        // null if Shape is not an Image
        private string _imagePath;
    }

}
