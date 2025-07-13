using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Woopec.Graphics
{
    /// <summary>
    /// For future use: Different shape types
    /// </summary>
    public enum ShapeType
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
    public abstract class ShapeBase
    {
        /// <summary>
        /// A shape can have a name. 
        /// All predefined shapes (Turtle, Classic and so on) have a name ("turtle", "classic" and so on).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Shape type
        /// </summary>
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
            Type = ShapeType.Polygon; // As long as it only contains one component it can be seen as a polygon
            _components = new();
        }

        /// <summary>
        /// Add a polygon to a compound shape
        /// </summary>
        /// <param name="polygon">Coordinates of the polygon. For example: new() { (0,0),(10,-5),(0,10),(-10,-5) } </param>
        public void AddComponent(List<Vec2D> polygon)
        {
            AddComponent(polygon, null, null);
        }


        /// <summary>
        /// Add a polygon to a compound shape
        /// </summary>
        /// <remarks>
        /// Currently only Shapes with one polygon are rendered correctly. Therefore this method is internal
        /// </remarks>
        /// <param name="polygon">Coordinates of the polygon. For example: new() { (0,0),(10,-5),(0,10),(-10,-5) } </param>
        /// <param name="fillColor">Color the polygon will be filled with</param>
        internal void AddComponent(List<Vec2D> polygon, Color fillColor)
        {
            AddComponent(polygon, fillColor, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Currently only Shapes with one polygon are rendered correctly. Therefore this method is internal
        /// </remarks>
        /// <param name="polygon">Coordinates of the polygon. For example: new() { (0,0),(10,-5),(0,10),(-10,-5) } </param>
        /// <param name="fillColor">Color the polygon will be filled with</param>
        /// <param name="outlineColor">Color for the polygons outline</param>
        internal void AddComponent(List<Vec2D> polygon, Color fillColor, Color outlineColor)
        {
            if (_components.Count > 0)
            {
                Type = ShapeType.Compound;
            }
            _components.Add(new ShapeComponent() { Polygon = polygon, FillColor = fillColor, OutlineColor = outlineColor });
        }

        /// <summary>
        /// Only for internal purposes!
        /// </summary>
        /// <remarks>
        /// Json-Serialization in <c>ProcessChannelConverter</c> does not work if the property is internal. Therefore:
        /// - The getter (needed by Wpf-code) is public
        /// - The setter (only needed by Json-Deserialization) is private
        /// </remarks>
        [JsonInclude]
        public List<ShapeComponent> Components { get { return _components; } private set { _components = value; } }

        // null if Shape is an Image
        private List<ShapeComponent> _components;

    }

    /// <summary>
    /// One part of a complex shape
    /// </summary>
    public class ShapeComponent
    {
        /// <summary>
        /// Coordinates of the polygon. For example: new() { (0,0),(10,-5),(0,10),(-10,-5) } 
        /// </summary>
        public List<Vec2D> Polygon { get; set; }

        /// <summary>
        /// Color the polygon will be filled with
        /// </summary>
        public Color FillColor { get; set; }

        /// <summary>
        /// Color for the polygons outline
        /// </summary>
        public Color OutlineColor { get; set; }
    }


    /// <summary>
    /// An instance of this class is an image which can be used as a shape
    /// </summary>
    /// <remarks>
    /// Currently only Shapes with one polygon are rendered correctly. Therefore this method is internal
    /// </remarks>
    internal class ImageShape : ShapeBase
    {
        /// <summary>
        /// Create an image shape.
        /// </summary>
        /// <param name="imagePath">The file-path of the image</param>
        public ImageShape(string imagePath)
        {
            Type = ShapeType.Image;
            _imagePath = imagePath;

            if (!string.IsNullOrWhiteSpace(_imagePath))
                throw new NotImplementedException("Image shapes are not implemented yet.");

            // to-do: Which way?
            // (a) Load image bitmap directly into this class (but do not create dependencies to WPF by this) 
            // (b) Only save the path (but that results to problems if the drawing consumer is not located on the same computer)
            // I would prefer (a)
        }


        // Only to be used from TurtleWpf
        public string ImagePath { get { return _imagePath; } set { _imagePath = value; } }

        // null if Shape is not an Image
        private string _imagePath;
    }

    /// <summary>
    /// Predifined Shapes
    /// </summary>
    public static class Shapes
    {
        private static readonly Dictionary<string, ShapeBase> s_shapes = new();

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        // The names and geometries of these shapes are copied from python-turtle (turtle.py by Gregor Lingl)
        public static Shape Arrow { get { return CreateOrGetNamedShape("arrow", new() { (-10, 0), (10, 0), (0, 10) }); } }
        public static Shape Circle { get { return CreateOrGetNamedShape("circle", new() { (10, 0), (9.51, 3.09), (8.09, 5.88), (5.88, 8.09), (3.09, 9.51), (0, 10), (-3.09, 9.51), (-5.88, 8.09), (-8.09, 5.88), (-9.51, 3.09), (-10, 0), (-9.51, -3.09), (-8.09, -5.88), (-5.88, -8.09), (-3.09, -9.51), (-0.00, -10.00), (3.09, -9.51), (5.88, -8.09), (8.09, -5.88), (9.51, -3.09) }); } }
        public static Shape Square { get { return CreateOrGetNamedShape("square", new() { (10, -10), (10, 10), (-10, 10), (-10, -10) }); } }
        public static Shape Triangle { get { return CreateOrGetNamedShape("triangle", new() { (10, -5.77), (0, 11.55), (-10, -5.77) }); } }
        public static Shape Classic { get { return CreateOrGetNamedShape("classic", new() { (0, 0), (-5, -9), (0, -7), (5, -9) }); } }
        public static Shape Turtle { get { return CreateOrGetNamedShape("turtle", new() { (0, 16), (-2, 14), (-1, 10), (-4, 7), (-7, 9), (-9, 8), (-6, 5), (-7, 1), (-5, -3), (-8, -6), (-6, -8), (-4, -5), (0, -7), (4, -5), (6, -8), (8, -6), (5, -3), (7, 1), (6, 5), (9, 8), (7, 9), (4, 7), (1, 10), (2, 14) }); } }

        // Turtle designed by Clemens:
        // public static Shape TurtleClemens { get { return CreateNamedShape("turtle", new() { (0, -10.5), (0.75, -9), (2.7, -9), (4.2, -7.5), (6.6, -9), (6.6, -12), (9.0, -12), (9.0, -7.5), (6.3, -5.4), (6.6, -5.1), (6.6, 5.1), (6.3, 5.4), (9.0, 7.5), (9.0, 12), (6.6, 12), (6.6, 9), (4.2, 7.5), (2.7, 9), (1.5, 9), (1.5, 15.8), (-1.5, 15.8), (-1.5, 9), (-2.7, 9), (-4.2, 7.5), (-6.6, 9), (-6.6, 12), (-9.0, 12), (-9.0, 7.5), (-6.3, 5.4), (-6.6, 5.1), (-6.6, -5.1), (-6.3, -5.4), (-9.0, -7.5), (-9.0, -12), (-6.6, -12), (-6.6, -9), (-4.2, -7.5), (-2.7, -9), (-0.75, -9), (0, -10.5) }); } }

        // Bird designed by Clemens:
        public static Shape Bird { get { return CreateOrGetNamedShape("bird", new() { (1, -4), (2, -3), (2, -2.5), (5, -2.5), (9, -4.5), (8, -0.5), (6, 2.5), (2, 3.5), (2, 4), (0, 7), (-2, 4), (-2, 3.5), (-6, 2.5), (-8, -0.5), (-9, -4.5), (-5, -2.5), (-2, -2.5), (-2, -3), (-1, -4), (-3, -7), (0, -6), (3, -7), (1, -4) }); } }
        // (1,4), (2,3), (2,2.5), (5,2.5), (9,4.5), (8,0.5), (6,-2.5), (2,-3.5), (2,-4), (0,-7), (-2,-4), (-2,-3.5), (-6,-2.5), (-8,0.5), (-9,4.5), (-5,2.5), (-2,2.5), (-2,3), (-1,4), (-3,7), (0,6), (3,7), (1,4)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Add a polygon to the shapelist.
        /// </summary>
        /// <param name="name">name of the shape</param>
        /// <param name="polygon">A list of coordinates</param>
        public static void Add(string name, List<Vec2D> polygon)
        {
            if (!s_shapes.ContainsKey(name))
            {
                var shape = new Shape(polygon);
                shape.Name = name;
                s_shapes.Add(name, shape);
            }
        }

        /// <summary>
        /// Add a shape to the shapelist.
        /// </summary>
        /// <param name="name">Name of the shape</param>
        /// <param name="shape">A compound shape (existing of multiple polygons)</param>
        public static void Add(string name, Shape shape)
        {
            if (!s_shapes.ContainsKey(name))
            {
                s_shapes.Add(name, shape);
            }
        }

        /// <summary>
        /// Get the shape of the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ShapeBase Get(string name)
        {
            return s_shapes.GetValueOrDefault(name);
        }

        /// <summary>
        /// Return a list of all currently available turtle shape-names
        /// </summary>
        /// <returns></returns>
        public static List<string> GetNames()
        {
            return s_shapes.Select(s => s.Key).ToList();
        }


        private static Shape CreateOrGetNamedShape(string name, List<Vec2D> polygon)
        {
            if (s_shapes.ContainsKey(name))
            {
                return s_shapes[name] as Shape;
            }
            else
            {
                var shape = new Shape(polygon)
                {
                    Name = name
                };
                Add(name, shape);
                return shape;
            }
        }
    }

}
