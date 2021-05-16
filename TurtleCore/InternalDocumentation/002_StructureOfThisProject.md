<!-- Template in [Documenting architecture decisions - Michael Nygard](http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions). -->

# ADR 001 Structure of this project
<!-- Short Title -->

## Status
<!-- What is the status, such as proposed, accepted, rejected, deprecated, superseded, etc.? -->
Accepted, 05-16-2021

## Context
<!-- What is the issue that we're seeing that is motivating this decision or change? -->
This project should contain:
* All classes a turtle programmer needs
* The underlying infrastructure, that is indepedent of the rendering (for instance WPF)
How shall we structure the content of this project

## Decision
<!-- What is the change that we're proposing and/or doing? -->
All classes of the underlying infrastructure are declared `internal` and gathered
in sub folder "Internal"

All classes that are accessible to the programmer who uses this project are located
directly in the project's folder.

These classes can be divided into the following groups:
* **Value Objects**: Color, Speed, Vec2D, Shape
* **High level Turtle classes**: Turtle, Screen
* **Middle level Turtle classes**: Pen, Figure. A turtle mainly consists of
  a pen and a figure. The pen and figure classes can be used autonmously.
* **Low level graphics classes**: ScreenLine, ScreenFigure, ScreenAnimation, etc.
  These classes describe the screen objects and their animations at a lower level. They are constructed by
  Pen and Figure, and then sent to Screen. The programmer can also use these low level
  classes.
 


## Consequences
<!-- What becomes easier or more difficult to do because of this change? -->
