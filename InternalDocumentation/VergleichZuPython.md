# State

## For comparison

Documentation: see https://docs.python.org/3/library/turtle.html#module-turtle
python Implementation: see https://github.com/python/cpython/blob/master/Lib/turtle.py
Sample turtle programs: see https://github.com/python/cpython/tree/master/Lib/turtledemo


## Overview of python turtle functions

See [python: turtle - Turtle graphics](https://docs.python.org/3/library/turtle.html#module-turtle)


### Turtle Methods

Remark: I no longer aim to produce the same functionality as python turtle. Currently
the following features of python are no longer planned:
* Different "modes" (turtle.mode()) with different interpretation of heading=0 and rotation-angles
* Different possiblilities to specify rotation angles (turtle.degrees() and turtle.radians())
* Different colormodes (turtle.colormode())
* User-defined coordinate-systems (turtle.setworldcoordinates())

These decesions make it easier for me to set the state for functions to "ok".

| function                            | priority | state                                                |
|-------------------------------------|----------|------------------------------------------------------|
| **Move & Draw**
| forward()                           | 1       | ok |
| backward()                          | 1       | ok |
| right()                             | 1       | ok |
| left()                              | 1       | ok |
| goto()                              |         | ok |
| setx()
| sety()
| setheading()                        | 1       | ok |
| home()
| circle()
| dot()
| stamp()
| clearstamp()
| clearstamps()
| undo()
| speed()                             | 1       | ok |
| **Tell Turtle’s state**
| position()                          |         | ok |
| towards()
| xcor()
| ycor()
| heading()                           |         | ok |
| distance()
| **Setting and measurement**
| degrees()                           | not planned |
| radians()                           | not planned |
| **Drawing state**
| pendown()                            | 1      | ok |
| penup()                              | 1      | ok |
| pensize() 
| pen()
| isdown()                             |        | ok |
| **Color Control**
| color()                              | 1      | ok |
| pencolor()                           | 1      | ok |
| fillcolor()                          | 1      | ok |
| **Filling**
| filling()                            |   | ok |
| begin_fill()                         | 1 | ok |
| end_fill()                           | 1 |  ok |
| **More drawing control**
| reset()
| clear()
| write()
| **Visibility**
| showturtle()                         | 1 | ok  |
| hideturtle()                         | 1 | ok  |
| isvisible()                          |   | ok  |
| **Appearance**
| shape()                              | 2 | ok |
| resizemode()                        
| shapesize() 
| shearfactor()
| settiltangle()
| tiltangle()
| tilt()
| shapetransform()
| get_shapepoly()
| **Using events**
| onclick()
| onrelease()
| ondrag()
| **Special Turtle methods**
| begin_poly()
| end_poly()
| get_poly()
| clone()
| getturtle() 
| getscreen()
| setundobuffer()
| undobufferentries()


### Methods of TurtleScreen/Screen

| function                            | priority | state                                                |
|-------------------------------------|----------|------------------------------------------------------|
| **Window control**
| bgcolor()                           | 2
| bgpic()
| clear() 
| reset() 
| screensize()
| setworldcoordinates()               | not planned | 
| **Animation control**
| delay()                             | ?       | Do we need this? Isn't Turtle.Speed() enough?         |
| tracer()                            | ?       | Do we need this? Isn't Turtle.ShowTurtle() enough?    |
| update()
| **Using screen events**
| listen()
| onkey() 
| onkeypress()
| onclick() 
| ontimer()
| mainloop()
| **Settings and special methods**
| mode()                            | not planned |
| colormode()                       | not planned |
| getcanvas()
| getshapes()                       |           | ?? unsure, if we need this because one can set the turtle's shape directly |
| register_shape()                  |           | ?? unsure, if we need this because one can set the turtle's shape directly |
| turtles()
| window_height()
| window_width()
| **Input methods**
| textinput()                       |           | Screen.TextInput()
| numinput()
| **Methods specific to Screen**
| bye()
| exitonclick()
| setup()
| title()

## Overview of python turtle classes

**Die Screen-Klasse ist noch nicht gut. Sie enthält Methoden für Programmierer (z.B. RegisterShape)
und Methoden, die eher intern sind (DrawLine, CreateLine)**

| python class                      | C# class         | state                                                |
|-----------------------------------|------------------|------------------------------------------------------|
| RawTurtle(canvas or TurtleScreen) | Turtle           | konstruiert über Turtle(Screen)
| RawPen(canvas)
| Turtle                            | Turtle           | konstruiert über Turtle()
| TurtleScreen(cv)                  | Screen           | konstruiert über Screen(IScreenOutput). Es könnte mehrere geben. 
| Screen                            | Screen           | konstruiert über Screen(). Es kann nur eins geben. Hat auch Methoden für das gesamte Window <- Dafür vielleich separate Klasse
| ScrolledCanvas(master)
| Shape(type_, data)                | Shape, ImageShape | Methods of ImageShape are there, but not implemented yet|
| Vec2D                             | Vec2D           | 100%                                                  |

## Overview of internally used python turtle classes

| python clas              | C# class                                               | state                                                |
|--------------------------|--------------------------------------------------------|------------------------------------------------------|
| TurtleScreenBase(object) | TurtleCore.IScreenOutput and TurtleWpf.ScreenOutput    | 



# Nuget Infos

Auf anderem Rechner kann es installiert und genutzt werden (ohne Doku)
* Siehe https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package-using-visual-studio?tabs=netcore-cli
* Ich brauche einen nuget Account (hab' ich den schon)
* Woopec.Core ruhig auch auch schon mal als eigenes Paktet, denn Pakete sollen ja nicht so groß sein
  * Properties des nuget-Pakets, Beschreibung siehe https://docs.microsoft.com/en-us/nuget/reference/nuspec
  * Package-Id: Woopec.Core
  * Package-Version: 1.0.0-alpha (nuget untersützt, so etwas wie pre-releases, daher das -alpha Suffix)
  * Author: Laut Beschreibung sollen hier die Namen von Autoren stehen, die zu Profil-Namen auf nuget.org passen. Also Woopec
  * License: MIT
  * Tags: Ich habe keine Liste von gängigen Tags gefunden. Erst mal diese: C# Graphics Turtle Beginners
  * Repository: Habe ich erst mal noch nicht eingetragen, weil das zurzeit noch private ist
* Woopec.Wpf analog. 
  * Package-Id: Woopec.Wpf
  * Package-Version: 1.0.0-alpha (nuget untersützt, so etwas wie pre-releases, daher das -alpha Suffix)
  * Author: Laut Beschreibung sollen hier die Namen von Autoren stehen, die zu Profil-Namen auf nuget.org passen. Also Woopec
  * License: MIT
  * Tags: Ich habe keine Liste von gängigen Tags gefunden. Erst mal diese: C# Graphics Turtle Beginners
  * Repository: Habe ich erst mal noch nicht eingetragen, weil das zurzeit noch private ist
  * Dabei nutze ich erst mal Woopec.Core über Projekt-Referenz und nicht über nuget (das ist einfacher).
    Es sieht für mich so aus, als ob Nuget das aber erkennt. Weil es beim Herunterladen des Woopec.Wpf Pakets
    behauptet, dass dieses Woopec.Core verwendet (magic?)
* Nutzung von Nuget woopec.wpf ausprobieren


