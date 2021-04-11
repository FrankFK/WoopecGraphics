#
# turtle.py: a Tkinter based turtle graphics module for Python
# Version 1.1b - 4. 5. 2009
#
# Copyright (C) 2006 - 2010  Gregor Lingl
# email: glingl@aon.at
#
# This software is provided 'as-is', without any express or implied
# warranty.  In no event will the authors be held liable for any damages
# arising from the use of this software.
#
# Permission is granted to anyone to use this software for any purpose,
# including commercial applications, and to alter it and redistribute it
# freely, subject to the following restrictions:
#
# 1. The origin of this software must not be misrepresented; you must not
#    claim that you wrote the original software. If you use this software
#    in a product, an acknowledgment in the product documentation would be
#    appreciated but is not required.
# 2. Altered source versions must be plainly marked as such, and must not be
#    misrepresented as being the original software.
# 3. This notice may not be removed or altered from any source distribution.


"""
Turtle graphics is a popular way for introducing programming to
kids. It was part of the original Logo programming language developed
by Wally Feurzig and Seymour Papert in 1966.
Imagine a robotic turtle starting at (0, 0) in the x-y plane. After an ``import turtle``, give it
the command turtle.forward(15), and it moves (on-screen!) 15 pixels in
the direction it is facing, drawing a line as it moves. Give it the
command turtle.right(25), and it rotates in-place 25 degrees clockwise.
By combining together these and similar commands, intricate shapes and
pictures can easily be drawn.
----- turtle.py
This module is an extended reimplementation of turtle.py from the
Python standard distribution up to Python 2.5. (See: http://www.python.org)
It tries to keep the merits of turtle.py and to be (nearly) 100%
compatible with it. This means in the first place to enable the
learning programmer to use all the commands, classes and methods
interactively when using the module from within IDLE run with
the -n switch.
Roughly it has the following features added:
- Better animation of the turtle movements, especially of turning the
  turtle. So the turtles can more easily be used as a visual feedback
  instrument by the (beginning) programmer.
- Different turtle shapes, gif-images as turtle shapes, user defined
  and user controllable turtle shapes, among them compound
  (multicolored) shapes. Turtle shapes can be stretched and tilted, which
  makes turtles very versatile geometrical objects.
- Fine control over turtle movement and screen updates via delay(),
  and enhanced tracer() and speed() methods.
- Aliases for the most commonly used commands, like fd for forward etc.,
  following the early Logo traditions. This reduces the boring work of
  typing long sequences of commands, which often occur in a natural way
  when kids try to program fancy pictures on their first encounter with
  turtle graphics.
- Turtles now have an undo()-method with configurable undo-buffer.
- Some simple commands/methods for creating event driven programs
  (mouse-, key-, timer-events). Especially useful for programming games.
- A scrollable Canvas class. The default scrollable Canvas can be
  extended interactively as needed while playing around with the turtle(s).
- A TurtleScreen class with methods controlling background color or
  background image, window and canvas size and other properties of the
  TurtleScreen.
- There is a method, setworldcoordinates(), to install a user defined
  coordinate-system for the TurtleScreen.
- The implementation uses a 2-vector class named Vec2D, derived from tuple.
  This class is public, so it can be imported by the application programmer,
  which makes certain types of computations very natural and compact.
- Appearance of the TurtleScreen and the Turtles at startup/import can be
  configured by means of a turtle.cfg configuration file.
  The default configuration mimics the appearance of the old turtle module.
- If configured appropriately the module reads in docstrings from a docstring
  dictionary in some different language, supplied separately  and replaces
  the English ones by those read in. There is a utility function
  write_docstringdict() to write a dictionary with the original (English)
  docstrings to disc, so it can serve as a template for translations.
Behind the scenes there are some features included with possible
extensions in mind. These will be commented and documented elsewhere.
"""

_ver = "turtle 1.1b- - for Python 3.1   -  4. 5. 2009"

# print(_ver)

import tkinter as TK
import types
import math
import time
import inspect
import sys

from os.path import isfile, split, join
from copy import deepcopy
from tkinter import simpledialog

_tg_classes = ['ScrolledCanvas', 'TurtleScreen', 'Screen',
               'RawTurtle', 'Turtle', 'RawPen', 'Pen', 'Shape', 'Vec2D']
_tg_screen_functions = ['addshape', 'bgcolor', 'bgpic', 'bye',
        'clearscreen', 'colormode', 'delay', 'exitonclick', 'getcanvas',
        'getshapes', 'listen', 'mainloop', 'mode', 'numinput',
        'onkey', 'onkeypress', 'onkeyrelease', 'onscreenclick', 'ontimer',
        'register_shape', 'resetscreen', 'screensize', 'setup',
        'setworldcoordinates', 'textinput', 'title', 'tracer', 'turtles', 'update',
        'window_height', 'window_width']
_tg_turtle_functions = ['back', 'backward', 'begin_fill', 'begin_poly', 'bk',
        'circle', 'clear', 'clearstamp', 'clearstamps', 'clone', 'color',
        'degrees', 'distance', 'dot', 'down', 'end_fill', 'end_poly', 'fd',
        'fillcolor', 'filling', 'forward', 'get_poly', 'getpen', 'getscreen', 'get_shapepoly',
        'getturtle', 'goto', 'heading', 'hideturtle', 'home', 'ht', 'isdown',
        'isvisible', 'left', 'lt', 'onclick', 'ondrag', 'onrelease', 'pd',
        'pen', 'pencolor', 'pendown', 'pensize', 'penup', 'pos', 'position',
        'pu', 'radians', 'right', 'reset', 'resizemode', 'rt',
        'seth', 'setheading', 'setpos', 'setposition', 'settiltangle',
        'setundobuffer', 'setx', 'sety', 'shape', 'shapesize', 'shapetransform', 'shearfactor', 'showturtle',
        'speed', 'st', 'stamp', 'tilt', 'tiltangle', 'towards',
        'turtlesize', 'undo', 'undobufferentries', 'up', 'width',
        'write', 'xcor', 'ycor']
_tg_utilities = ['write_docstringdict', 'done']

__all__ = (_tg_classes + _tg_screen_functions + _tg_turtle_functions +
           _tg_utilities + ['Terminator']) # + _math_functions)

_alias_list = ['addshape', 'backward', 'bk', 'fd', 'ht', 'lt', 'pd', 'pos',
               'pu', 'rt', 'seth', 'setpos', 'setposition', 'st',
               'turtlesize', 'up', 'width']

# File _configuration_handling.py

[Public]
class Vec2D(tuple):                     # File: Vec2DClass.py
    """A 2 dimensional vector class, used as a helper class
    for implementing turtle graphics.
    May be useful for turtle graphics programs also.
    Derived from tuple, so a vector is a tuple!
    Provides (for a, b vectors, k number):
       a+b vector addition
       a-b vector subtraction
       a*b inner product
       k*a and a*k multiplication with scalar
       |a| absolute value of a
       a.rotate(angle) rotation
    """

##############################################################################
### From here up to line    : Tkinter - Interface for turtle.py            ###
### May be replaced by an interface to some different graphics toolkit     ###
##############################################################################


[Public]
class ScrolledCanvas(TK.Frame):             # File: ScrolledCanvasClass.py
    """Modeled after the scrolled canvas class from Grayons's Tkinter book.
    Used as the default canvas, which pops up automatically when
    using turtle graphics functions or the Turtle class.
    """

__forwardmethods(ScrolledCanvas, TK.Canvas, '_canvas')


class _Root(TK.Tk):                         # File: _RootClass.py
    """Root class for Screen based on Tkinter."""

Canvas = TK.Canvas


class TurtleScreenBase(object):             # File: TurtleScreenBaseClass.py
    """Provide the basic graphics functionality.
       Interface between Tkinter and turtle.py.
       To port turtle.py to some different graphics toolkit
       a corresponding TurtleScreenBase class has to be implemented.
    """

##############################################################################
###                  End of Tkinter - interface                            ###
##############################################################################


class Terminator (Exception):
    """Will be raised in TurtleScreen.update, if _RUNNING becomes False.
    This stops execution of a turtle graphics script.
    Main purpose: use in the Demo-Viewer turtle.Demo.py.
    """
    pass


class TurtleGraphicsError(Exception):
    """Some TurtleGraphics Error
    """


[Public]
class Shape(object):                        # File: ShapeClass.py
    """Data structure modeling shapes.
    attribute _type is one of "polygon", "image", "compound"
    attribute _data is - depending on _type a poygon-tuple,
    an image or a list constructed using the addcomponent method.
    """


class Tbuffer(object):                      # File: TbufferClass.py
    """Ring buffer used as undobuffer for RawTurtle objects."""


[Public]
class TurtleScreen(TurtleScreenBase):       # File: TurtleScreenClass.py
    """Provides screen oriented methods like setbg etc.
    Only relies upon the methods of TurtleScreenBase and NOT
    upon components of the underlying graphics toolkit -
    which is Tkinter in this case.
    """


class TNavigator(object):                   # File: TNavigatorClass.py
    """Navigation part of the RawTurtle.
    Implements methods for turtle movement.
    """


class TPen(object):                         # File: TPenClass.py
    """Drawing part of the RawTurtle.
    Implements drawing properties.
    """


class _TurtleImage(object):                 # File: _TurtleImageClass.py
    """Helper class: Datatype to store Turtle attributes
    """

[Public]
class RawTurtle(TPen, TNavigator):          # File: RawTurtleClass.py
    """Animation part of the RawTurtle.
    Puts RawTurtle upon a TurtleScreen and provides tools for
    its animation.
    """

[Public]
RawPen = RawTurtle


###  Screen - Singleton  ########################
[Public]
def Screen():
    """Return the singleton screen object.
    If none exists at the moment, create a new one and return it,
    else return the existing one."""
    if Turtle._screen is None:
        Turtle._screen = _Screen()
    return Turtle._screen

class _Screen(TurtleScreen):                # File: _ScreenClass.py


[Public]
class Turtle(RawTurtle):                    # File: TurtleClass.py
    """RawTurtle auto-creating (scrolled) canvas.
    When a Turtle object is created or a function derived from some
    Turtle method is called a TurtleScreen object is automatically created.
    """


[Public]
Pen = Turtle

# File: _generate_documentation.py

# File: _generate_functions.py

# File: _main_with_demons.py


done = mainloop

