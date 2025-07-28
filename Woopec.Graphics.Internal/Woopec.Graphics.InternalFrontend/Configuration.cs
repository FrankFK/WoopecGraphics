namespace Woopec.Graphics.Internal.Frontend
{
    /// <summary>
    /// ToDo: Set via configuration file and/or methods, like settings in https://docs.python.org/3/library/turtle.html#how-to-configure-screen-and-turtles
    /// </summary>
    internal class Configuration
    {
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 800;
        public string Title { get; set; } = "Woopec Graphics";
    }
}
