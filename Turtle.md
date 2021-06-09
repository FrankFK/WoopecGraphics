# Woopec: Turtle-Class

## Introduction

> Turtle graphics is a popular way for introducing programming to kids. It was part of the original Logo programming language developed by Wally Feurzeig, Seymour Papert and Cynthia Solomon in 1967.
> >
> Imagine a robotic turtle starting at (0, 0) in the x-y plane. [...] give it the command turtle.Forward(15), and it moves (on-screen!) 15 pixels in the direction it is facing, drawing a line as it moves. Give it the command turtle.Right(25), and it rotates in-place 25 degrees clockwise.
>
> Turtle can draw intricate shapes using programs that repeat simple moves. By combining together these and similar commands, intricate shapes and pictures can easily be drawn

This quote from the documentation of the [pyhton-Turtle-Graphics](https://docs.python.org/3/library/turtle.html#module-turtle) describes the advantages of turtle Graphics.

Woopec-turtle is an attempt to make something similar available to C# developers as well. 

The structure (methods and their names) was largely taken over from python. Some things have been adapted to C# (upper/lower case, properties). The range of functions does not come close to that of the great python library. But it's a first step.

### Examples

Let's start with a simple example:
```csharp
    var turtle = Turtle.Seymour();

    turtle.Right(45);
    turtle.Forward(50);
    turtle.Left(90);
    turtle.Forward(100);
    turtle.Right(45);
    turtle.Forward(20);
```
This code produces the following result:





### Remarks

Ich kommentiere die Details der Methoden direkt im Code. Man kann das über Intellisense sehen.
Hier nur eine Übersicht über die vorhandenen Methoden

### Overview of Methods

Bei Microsoft gibt es für jede Methode eine separate Hilfeseite. Das würde ich gerne schlanker halten.
Daher vielleicht so wie in Python erst mal eine kurze Übersicht über alle Methoden. Und dann weiter unten
detaillierte Doku zu jeder einzelnen Methode.


#### Move, Draw and Position State

| Method                                           | Description                                           |
|--------------------------------------------------|-------------------------------------------------------|
| Forward(double distance)                         | Move forward                                          |
| Backward(double distance)                        | Move backward                                         |
| Left(double angle)                               | Rotate left                                           |
| Right(double angle)                              | Rotate right                                          |
| SetPosition(Vec2D position) GoTo(Vec2D position) | Change position.                                      |
| Position [Type is Vec2D]                         | Get or change position                                |
| SetHeading(double angle)                         | Change heading (rotate to this heading)               |
| Heading [Type is double]                         | Get or change heading                                 |
| Speed [Type is Speed]                            | Get or change speed                                 |

#### Drawing state

| Method                                           | Description                                           |
|--------------------------------------------------|-------------------------------------------------------|
| PenUp()                                          | Pull the pen down – drawing when moving                                                       |
| PenDown()                                        | Pull the pen up – no drawing when moving                                                       |
| IsDown [Type is bool]                            | Get or change state of pen                            |

#### Color control

| Method                                           | Description                                           |
|--------------------------------------------------|-------------------------------------------------------|
| PenColor [Type is Color]                         | Pencolor                                              |
| FillColor [Type is Color]                        | Fillcolor                                             |
| Color [Type is Color]                            | Change pencolor and fillcolor                         |
|                                                  |                                                       |


#### Visibility and appearance

| Method                                           | Description                                           |
|--------------------------------------------------|-------------------------------------------------------|
| HideTurtle()                                     | Make the turtle invisible                             |
| ShowTurtle()                                     | Make the turtle visible                               |
| IsVisible (Type is bool)                         | True if turtle is shown, false if is hidden           |
| Shape (Type is a Shape)                          | Get or change shape of the turtle                     |

#### Filling

| Method                                           | Description                                           |
|--------------------------------------------------|-------------------------------------------------------|
| BeginFill()                                      | Start the filling                                     |
| EndFill()                                        | Fill the shape drawn after the last call of BeginFill()|
| Filling (Type is bool)                           | Return fillstate (true if filling, false else)        |
