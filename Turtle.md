# Woopec.Core Classes and Methods


## Introduction

## Turtle Class

Kurze Info zu dieser Klasse.
Was ist der Pen, was ist die (Figure) / Turtle...

### Examples

The following example ...
Sollte hier erst mal ein möglichst einfaches Beispiel sein (Quadrat oder so). Damit jeder erst mal
das Prinzip verstehen kann. Vielleicht auch ein Bild dazu, das das Ergebnis zeigt.

```csharp
    var joe = new Turtle();

    joe.Right(45);
    joe.Forward(50);
    joe.Left(90);
    joe.Forward(100);
    joe.Right(45);
    joe.Forward(20);

```

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
