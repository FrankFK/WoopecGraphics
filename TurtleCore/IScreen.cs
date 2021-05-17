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
    public interface IScreen
    {
        /// <summary>
        /// Return the GroupId of the last animation that is drawn at the screen
        /// </summary>
        public int LastIssuedAnimatonGroupID { get; set; }

        public int CreateLine();

        public void DrawLine(ScreenLine line);

        public int CreateFigure(string shapeName);

        public void UpdateFigure(ScreenFigure figure);

        /// <summary>
        /// Add a shape to the screen's shapelist. Only these shapes can be used by Turtle.Shape = name
        /// </summary>
        /// <param name="name">name of the shape</param>
        /// <param name="shape">A Shape class or an ImageShape class</param>
        public void RegisterShape(string name, ShapeBase shape);

        /// <summary>
        /// The same as AddShape
        /// </summary>
        /// <param name="name"></param>
        /// <param name="shape"></param>
        public void AddShape(string name, ShapeBase shape);

        /// <summary>
        /// Return a list of all currently available turtle shapes
        /// </summary>
        /// <returns></returns>
        public List<string> GetShapes();

    }
}
