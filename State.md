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
| **Tell Turtle�s state**
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

**Die Screen-Klasse ist noch nicht gut. Sie enth�lt Methoden f�r Programmierer (z.B. RegisterShape)
und Methoden, die eher intern sind (DrawLine, CreateLine)**

| python class                      | C# class         | state                                                |
|-----------------------------------|------------------|------------------------------------------------------|
| RawTurtle(canvas or TurtleScreen) | Turtle           | konstruiert �ber Turtle(Screen)
| RawPen(canvas)
| Turtle                            | Turtle           | konstruiert �ber Turtle()
| TurtleScreen(cv)                  | Screen           | konstruiert �ber Screen(IScreenOutput). Es k�nnte mehrere geben. 
| Screen                            | Screen           | konstruiert �ber Screen(). Es kann nur eins geben. Hat auch Methoden f�r das gesamte Window <- Daf�r vielleich separate Klasse
| ScrolledCanvas(master)
| Shape(type_, data)                | Shape, ImageShape | Methods of ImageShape are there, but not implemented yet|
| Vec2D                             | Vec2D           | 100%                                                  |

## Overview of internally used python turtle classes

| python clas              | C# class                                               | state                                                |
|--------------------------|--------------------------------------------------------|------------------------------------------------------|
| TurtleScreenBase(object) | TurtleCore.IScreenOutput and TurtleWpf.ScreenOutput    | 


