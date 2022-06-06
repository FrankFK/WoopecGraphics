using Woopec.Core;

Turtle.ExperimentalInit();

var turtle = Turtle.Seymour();

turtle.Right(45);
turtle.Forward(50);
turtle.Left(90);
turtle.Forward(100);

var answer = turtle.Screen.TextInput("Color selection", "Please input the name of a color (e.g. red):");

if (answer != null)
    turtle.Color = answer;


turtle.Right(45);
turtle.Forward(200);

Console.ReadKey();

turtle.Left(90);
turtle.Forward(100);
