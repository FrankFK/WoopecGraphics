using Woopec.Core;

internal class Program
{
    public static void WoopecMain()
    {
        var seymour = Turtle.Seymour();

        seymour.Left(45);
        seymour.Forward(100);
        seymour.Right(45);
        seymour.Forward(50);
    }
}

