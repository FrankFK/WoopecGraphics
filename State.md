# State

## Overview of python turtle functions

See [python: turtle - Turtle graphics](https://docs.python.org/3/library/turtle.html#module-turtle)


### Turtle Methods

| function                            | priority | state                                                |
|-------------------------------------|----------|------------------------------------------------------|
| **Move & Draw**
| forward()                           | 1
| backward()                          | 1
| right()                             | 1
| left()                              | 1
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
| speed()                             | 1
| **Tell Turtle’s state**
| position() 
| towards()
| xcor()
| ycor()
| heading()
| distance()
| **Setting and measurement**
| degrees()
| radians()
| **Drawing state**
| pendown()                            | 1
| penup()                              | 1
| pensize() 
| pen()
| isdown()
| **Color Control**
| color()                              | 1
| pencolor()                           | 1
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
| getshapes()
| register_shape() 
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

| python class                      | C# class         | state                                                |
|-----------------------------------|------------------|------------------------------------------------------|
| RawTurtle(canvas or TurtleScreen) | Turtle           | konstruiert über Turtle(Screen)
| RawPen(canvas)
| Turtle                            | Turtle           | konstruiert über Turtle()
| TurtleScreen(cv)                  | Screen           | konstruiert über Screen(IScreenOutput). Es könnte mehrere geben. 
| Screen                            | Screen           | konstruiert über Screen(). Es kann nur eins geben. Hat auch Methoden für das gesamte Window <- Dafür vielleich separate Klasse
| ScrolledCanvas(master)
| Shape(type_, data)
| Vec2D                             | Vec2D           | 100%                                                  |

## Overview of internally used python turtle classes

| python clas              | C# class                                               | state                                                |
|--------------------------|--------------------------------------------------------|------------------------------------------------------|
| TurtleScreenBase(object) | TurtleCore.IScreenOutput and TurtleWpf.ScreenOutput    | 


