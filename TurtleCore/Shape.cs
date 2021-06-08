using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core
{
    public enum ShapeType
    {
        Polygon,
        Image,
        Compound
    }

    public abstract class ShapeBase
    {
        /// <summary>
        /// A shape can have a name. 
        /// All predefined shapes (Turtle, Classic and so on) have a name ("turtle", "classic" and so on).
        /// </summary>
        public string Name { get; set; }

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

    /// <summary>
    /// Predifined Shapes
    /// </summary>
    public static class Shapes
    {
        // The names and geometries of these shapes are copied from python-turtle (turtle.py by Gregor Lingl)
        public static Shape Arrow { get { return CreateNamedShape("arrow", new() { (-10, 0), (10, 0), (0, 10) }); } }
        public static Shape Circle { get { return CreateNamedShape("circle", new() { (10, 0), (9.51, 3.09), (8.09, 5.88), (5.88, 8.09), (3.09, 9.51), (0, 10), (-3.09, 9.51), (-5.88, 8.09), (-8.09, 5.88), (-9.51, 3.09), (-10, 0), (-9.51, -3.09), (-8.09, -5.88), (-5.88, -8.09), (-3.09, -9.51), (-0.00, -10.00), (3.09, -9.51), (5.88, -8.09), (8.09, -5.88), (9.51, -3.09) }); } }
        public static Shape Square { get { return CreateNamedShape("square", new() { (10, -10), (10, 10), (-10, 10), (-10, -10) }); } }
        public static Shape Triangle { get { return CreateNamedShape("triangle", new() { (10, -5.77), (0, 11.55), (-10, -5.77) }); } }
        public static Shape Classic { get { return CreateNamedShape("classic", new() { (0, 0), (-5, -9), (0, -7), (5, -9) }); } }
        public static Shape Turtle { get { return CreateNamedShape("turtle", new() { (0, 16), (-2, 14), (-1, 10), (-4, 7), (-7, 9), (-9, 8), (-6, 5), (-7, 1), (-5, -3), (-8, -6), (-6, -8), (-4, -5), (0, -7), (4, -5), (6, -8), (8, -6), (5, -3), (7, 1), (6, 5), (9, 8), (7, 9), (4, 7), (1, 10), (2, 14) }); } }

        // Turtle designed by Clemens:
        // public static Shape TurtleClemens { get { return CreateNamedShape("turtle", new() { (0, -10.5), (0.75, -9), (2.7, -9), (4.2, -7.5), (6.6, -9), (6.6, -12), (9.0, -12), (9.0, -7.5), (6.3, -5.4), (6.6, -5.1), (6.6, 5.1), (6.3, 5.4), (9.0, 7.5), (9.0, 12), (6.6, 12), (6.6, 9), (4.2, 7.5), (2.7, 9), (1.5, 9), (1.5, 15.8), (-1.5, 15.8), (-1.5, 9), (-2.7, 9), (-4.2, 7.5), (-6.6, 9), (-6.6, 12), (-9.0, 12), (-9.0, 7.5), (-6.3, 5.4), (-6.6, 5.1), (-6.6, -5.1), (-6.3, -5.4), (-9.0, -7.5), (-9.0, -12), (-6.6, -12), (-6.6, -9), (-4.2, -7.5), (-2.7, -9), (-0.75, -9), (0, -10.5) }); } }

        // Bird designed by Clemens:
        public static Shape Bird { get { return CreateNamedShape("bird", new() { (1, -4), (2, -3), (2, -2.5), (5, -2.5), (9, -4.5), (8, -0.5), (6, 2.5), (2, 3.5), (2, 4), (0, 7), (-2, 4), (-2, 3.5), (-6, 2.5), (-8, -0.5), (-9, -4.5), (-5, -2.5), (-2, -2.5), (-2, -3), (-1, -4), (-3, -7), (0, -6), (3, -7), (1, -4) }); } }

        // (1,4), (2,3), (2,2.5), (5,2.5), (9,4.5), (8,0.5), (6,-2.5), (2,-3.5), (2,-4), (0,-7), (-2,-4), (-2,-3.5), (-6,-2.5), (-8,0.5), (-9,4.5), (-5,2.5), (-2,2.5), (-2,3), (-1,4), (-3,7), (0,6), (3,7), (1,4)
        private static Shape CreateNamedShape(string name, List<Vec2D> polygon)
        {
            var shape = new Shape(polygon);
            shape.Name = name;
            return shape;
        }
    }

}
