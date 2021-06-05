# State

## Overview of python turtle functions

See [python: turtle - Turtle graphics](https://docs.python.org/3/library/turtle.html#module-turtle)


### Turtle Methods

| function                            | priority | state                                                |
|-------------------------------------|----------|------------------------------------------------------|
| **Move & Draw**
| forward()                           | 1       | first version |
| backward()                          | 1       | first version |
| right()                             | 1       | first version |
| left()                              | 1       | first version |
| goto()                              |         | 100% |
| setx()
| sety()
| setheading()                        | 1       | 100% |
| home()
| circle()
| dot()
| stamp()
| clearstamp()
| clearstamps()
| undo()
| speed()                             | 1       | first version |
| **Tell Turtle’s state**
| position()                          |         | first version |
| towards()
| xcor()
| ycor()
| heading()                           |         | 100% |
| distance()
| **Setting and measurement**
| degrees()
| radians()
| **Drawing state**
| pendown()                            | 1      | first version |
| penup()                              | 1      | first version |
| pensize() 
| pen()
| isdown()                             |        | first version |
| **Color Control**
| color()                              | 1       | 100% only a setter for pencolor and fillcolor, no getter  |
| pencolor()                           | 1       | first version, one polygon works |
| fillcolor()                          | 1       | first version, one polygon works |
| **Filling**
| filling()
| begin_fill()                         | 1 |  |
| end_fill()                           | 1 |  |
| **More drawing control**
| reset()
| clear()
| write()
| **Visibility**
| showturtle()                         | 1 | first version |
| hideturtle()                         | 1 | first version |
| isvisible()                          |   | first version |
| **Appearance**
| shape()                              | 2 | first version (a little bit different, because the shape is not set by its name) |
| resizemode()                         | 1 |        |
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
| setworldcoordinates()
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
| mode()
| colormode()
| getcanvas()
| getshapes()                       |           | ?? unsure, if we need this because one can set the turtle's shape directly |
| register_shape()                  |           | ?? unsure, if we need this because one can set the turtle's shape directly |
| turtles()
| window_height()
| window_width()
| **Input methods**
| textinput()
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


