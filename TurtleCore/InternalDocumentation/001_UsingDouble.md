<!-- Template in [Documenting architecture decisions - Michael Nygard](http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions). -->

# ADR 001 Using double instead of decimal
<!-- Short Title -->

## Status
<!-- What is the status, such as proposed, accepted, rejected, deprecated, superseded, etc.? -->
Accepted, 04-02-2021

## Context
<!-- What is the issue that we're seeing that is motivating this decision or change? -->
Should we use double or decimal for the X- and Y-coordinates of Turtle.Vec2D?

## Decision
<!-- What is the change that we're proposing and/or doing? -->
We are using double, because calculations are faster with double.
The decimal type would be more precise and intuitive in operations +, - and *. 
But often vectors are rotated and after a rotation the decimal precision does not 
have an advantage any more.

## Consequences
<!-- What becomes easier or more difficult to do because of this change? -->
