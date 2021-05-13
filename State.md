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
| goto()
| setx()
| sety()
| setheading()
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
| heading()
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
| color()                              | 1
| pencolor()                           | 1       | first version |
| fillcolor()
| **Filling**
| filling()
| begin_fill()                         | 2 | I want to see the turtle-star-demo |
| end_fill()                           | 2 | I want to see the turtle-star-demo |
| **More drawing control**
| reset()
| clear()
| write()
| **Visibility**
| showturtle()                         | 1
| hideturtle()                         | 1
| isvisible()
| **Appearance**
| shape()
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
| bgcolor()                           | 1
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
| getshapes()                       |           | 100% |
| register_shape()                  |           | Screen.RegisterShape(string name, ShapeBase shape), images not possible yet |
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


