using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore
{
    /// <summary>
    /// An instance of this class represents the screen to which screen objects (lines, shapes, ...) are drawn
    /// </summary>
    public class Screen
    {
        private static Screen _defaultScreen;

        private readonly IScreenObjectProducer _screenObjectProducer;

        private readonly Dictionary<string, ShapeBase> _shapes;

        private Screen(IScreenObjectProducer producer)
        {
            _screenObjectProducer = producer ?? throw new ArgumentNullException("producer");
            _shapes = new();
            AddPredefinedShapes(_shapes);
            LastIssuedAnimatonGroupID = ScreenObject.NoGroupId;
        }


        /// <summary>
        /// Return the GroupId of the last animation that is drawn at the screen
        /// </summary>
        public int LastIssuedAnimatonGroupID { get; set; }

        public int CreateLine()
        {
            return _screenObjectProducer.CreateLine();
        }

        public void DrawLine(ScreenLine line)
        {
            UpdateLastIssuedAnimationGroupID(line);
            _screenObjectProducer.DrawLine(line);
        }

        public int CreateFigure(string shapeName)
        {
            int figureId = 0;
            if (_shapes.TryGetValue(shapeName, out var shape))
            {
                figureId = _screenObjectProducer.CreateFigure(shape);
            }
            else
            {
                throw new KeyNotFoundException($"A shape with name '{shapeName} does not exist. At first call RegisterShape(...) to register the shape");
            }
            return figureId;
        }

        public void UpdateFigure(ScreenFigure figure)
        {
            UpdateLastIssuedAnimationGroupID(figure);
            _screenObjectProducer.UpdateFigure(figure);
        }

        /// <summary>
        /// Add a shape to the screen's shapelist. Only these shapes can be used by Turtle.Shape = name
        /// </summary>
        /// <param name="name">name of the shape</param>
        /// <param name="shape">A Shape class or an ImageShape class</param>
        public void RegisterShape(string name, ShapeBase shape)
        {
            _shapes.Add(name, shape);
        }
        public void AddShape(string name, ShapeBase shape) => RegisterShape(name, shape);

        /// <summary>
        /// Return a list of all currently available turtle shapes
        /// </summary>
        /// <returns></returns>
        public List<string> GetShapes()
        {
            return _shapes.Select(s => s.Key).ToList();
        }

        /// <summary>
        /// Create a Screen-Instance which draws to a default-Screen
        /// </summary>
        /// <returns></returns>
        internal static Screen GetDefaultScreen()
        {
            if (_defaultScreen == null)
            {
                _defaultScreen = new Screen(TurtleOutputs.GetDefaultScreenObjectProducer());
            }
            return _defaultScreen;
        }

        /// <summary>
        /// Needed for tests
        /// </summary>
        internal static void ResetDefaultScreen()
        {
            _defaultScreen = null;
        }

        private static void AddPredefinedShapes(Dictionary<string, ShapeBase> shapes)
        {
            // The names and geometries of these shapes are copied from python-turtle (turtle.py by Gregor Lingl)
            shapes.Add(ShapeNames.Arrow, new Shape(new() { (-10, 0), (10, 0), (0, 10) }));
            shapes.Add(ShapeNames.Turtle, new Shape(new() { (0, 16), (-2, 14), (-1, 10), (-4, 7), (-7, 9), (-9, 8), (-6, 5), (-7, 1), (-5, -3), (-8, -6), (-6, -8), (-4, -5), (0, -7), (4, -5), (6, -8), (8, -6), (5, -3), (7, 1), (6, 5), (9, 8), (7, 9), (4, 7), (1, 10), (2, 14) }));
            shapes.Add(ShapeNames.Circle, new Shape(new() { (10, 0), (9.51, 3.09), (8.09, 5.88), (5.88, 8.09), (3.09, 9.51), (0, 10), (-3.09, 9.51), (-5.88, 8.09), (-8.09, 5.88), (-9.51, 3.09), (-10, 0), (-9.51, -3.09), (-8.09, -5.88), (-5.88, -8.09), (-3.09, -9.51), (-0.00, -10.00), (3.09, -9.51), (5.88, -8.09), (8.09, -5.88), (9.51, -3.09) }));
            shapes.Add(ShapeNames.Square, new Shape(new() { (10, -10), (10, 10), (-10, 10), (-10, -10) }));
            shapes.Add(ShapeNames.Triangle, new Shape(new() { (10, -5.77), (0, 11.55), (-10, -5.77) }));
            shapes.Add(ShapeNames.Classic, new Shape(new() { (0, 0), (-5, -9), (0, -7), (5, -9) }));
        }

        private void UpdateLastIssuedAnimationGroupID(ScreenObject screenObject)
        {
            if (screenObject.HasAnimations)
            {
                LastIssuedAnimatonGroupID = screenObject.GroupID;
            }
        }
    }
}
