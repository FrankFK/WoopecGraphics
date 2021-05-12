using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    public enum ShapeType
    {
        Polygon,
        Image,
        Compound
    }

    public abstract class ShapeBase
    {
        public ShapeType Type { get; protected set; }
    }

    /// <summary>
    /// An instance of this class is one of the following
    /// - A polygon (fillcolor and outline color are specified by the turtle class)
    /// - A list of polygons (with fillcolor and outline color)
    /// </summary>
    public class Shape : ShapeBase
    {
        /// <summary>
        /// Create a polygon shape
        /// </summary>
        /// <param name="polygon">Coordinates of the polygon. For example: new() { (0,0),(10,-5),(0,10),(-10,-5) } </param>
        public Shape(List<Vec2D> polygon)
        {
            Type = ShapeType.Polygon;
            _components = new();
            _components.Add(new ShapeComponent() { Polygon = polygon, FillColor = null, OutlineColor = null });
        }

        /// <summary>
        /// Create a compound shape. Components can be added by AddComponent
        /// </summary>
        public Shape()
        {
            Type = ShapeType.Compound;
            _components = new();
        }


        /// <summary>
        /// Add a polygon to a compound shape
        /// </summary>
        /// <param name="polygon">Coordinates of the polygon. For example: new() { (0,0),(10,-5),(0,10),(-10,-5) } </param>
        /// <param name="fillColor">Color the polygon will be filled with</param>
        public void AddComponent(List<Vec2D> polygon, Color fillColor)
        {
            AddComponent(polygon, fillColor, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="polygon">Coordinates of the polygon. For example: new() { (0,0),(10,-5),(0,10),(-10,-5) } </param>
        /// <param name="fillColor">Color the polygon will be filled with</param>
        /// <param name="outlineColor">Color for the polygons outline</param>
        public void AddComponent(List<Vec2D> polygon, Color fillColor, Color outlineColor)
        {
            if (Type != ShapeType.Compound)
                throw new NotSupportedException("AddComponent is only allowed for a shape that was constructed in this way: shape = new Shape().");
            _components.Add(new ShapeComponent() { Polygon = polygon, FillColor = fillColor, OutlineColor = outlineColor });
        }

        // Only to be used from TurtleWpf
        internal List<ShapeComponent> Components { get { return _components; } }

        // null if Shape is an Image
        private readonly List<ShapeComponent> _components;

    }

    internal class ShapeComponent
    {
        public List<Vec2D> Polygon { get; set; }

        public Color FillColor { get; set; }

        public Color OutlineColor { get; set; }
    }


    /// <summary>
    /// An instance of this class is an image which can be used as a shape
    /// </summary>
    public class ImageShape : ShapeBase
    {
        /// <summary>
        /// Create an image shape.
        /// </summary>
        /// <param name="imagePath">The file-path of the image</param>
        public ImageShape(string imagePath)
        {
            Type = ShapeType.Image;
            _imagePath = imagePath;

            throw new NotImplementedException("Image shapes are not implemented yet.");

            // to-do: Which way?
            // (a) Load image bitmap directly into this class (but do not create dependencies to WPF by this) 
            // (b) Only save the path (but that results to problems if the drawing consumer is not located on the same computer)
            // I would prefer (a)
        }

        // Only to be used from TurtleWpf
        internal string ImagePath { get { return _imagePath; } }

        // null if Shape is not an Image
        private readonly string _imagePath;
    }
}
