class TPen(object):
    """Drawing part of the RawTurtle.
    Implements drawing properties.
    """
    def __init__(self, resizemode=_CFG["resizemode"]):
        self._resizemode = resizemode # or "user" or "noresize"
        self.undobuffer = None
        TPen._reset(self)

    def _reset(self, pencolor=_CFG["pencolor"],
                     fillcolor=_CFG["fillcolor"]):
        self._pensize = 1
        self._shown = True
        self._pencolor = pencolor
        self._fillcolor = fillcolor
        self._drawing = True
        self._speed = 3
        self._stretchfactor = (1., 1.)
        self._shearfactor = 0.
        self._tilt = 0.
        self._shapetrafo = (1., 0., 0., 1.)
        self._outlinewidth = 1

    def resizemode(self, rmode=None):
        """Set resizemode to one of the values: "auto", "user", "noresize".
        (Optional) Argument:
        rmode -- one of the strings "auto", "user", "noresize"
        Different resizemodes have the following effects:
          - "auto" adapts the appearance of the turtle
                   corresponding to the value of pensize.
          - "user" adapts the appearance of the turtle according to the
                   values of stretchfactor and outlinewidth (outline),
                   which are set by shapesize()
          - "noresize" no adaption of the turtle's appearance takes place.
        If no argument is given, return current resizemode.
        resizemode("user") is called by a call of shapesize with arguments.
        Examples (for a Turtle instance named turtle):
        >>> turtle.resizemode("noresize")
        >>> turtle.resizemode()
        'noresize'
        """
        if rmode is None:
            return self._resizemode
        rmode = rmode.lower()
        if rmode in ["auto", "user", "noresize"]:
            self.pen(resizemode=rmode)

    def pensize(self, width=None):
        """Set or return the line thickness.
        Aliases:  pensize | width
        Argument:
        width -- positive number
        Set the line thickness to width or return it. If resizemode is set
        to "auto" and turtleshape is a polygon, that polygon is drawn with
        the same line thickness. If no argument is given, current pensize
        is returned.
        Example (for a Turtle instance named turtle):
        >>> turtle.pensize()
        1
        >>> turtle.pensize(10)   # from here on lines of width 10 are drawn
        """
        if width is None:
            return self._pensize
        self.pen(pensize=width)


    def penup(self):
        """Pull the pen up -- no drawing when moving.
        Aliases: penup | pu | up
        No argument
        Example (for a Turtle instance named turtle):
        >>> turtle.penup()
        """
        if not self._drawing:
            return
        self.pen(pendown=False)

    def pendown(self):
        """Pull the pen down -- drawing when moving.
        Aliases: pendown | pd | down
        No argument.
        Example (for a Turtle instance named turtle):
        >>> turtle.pendown()
        """
        if self._drawing:
            return
        self.pen(pendown=True)

    def isdown(self):
        """Return True if pen is down, False if it's up.
        No argument.
        Example (for a Turtle instance named turtle):
        >>> turtle.penup()
        >>> turtle.isdown()
        False
        >>> turtle.pendown()
        >>> turtle.isdown()
        True
        """
        return self._drawing

    def speed(self, speed=None):
        """ Return or set the turtle's speed.
        Optional argument:
        speed -- an integer in the range 0..10 or a speedstring (see below)
        Set the turtle's speed to an integer value in the range 0 .. 10.
        If no argument is given: return current speed.
        If input is a number greater than 10 or smaller than 0.5,
        speed is set to 0.
        Speedstrings  are mapped to speedvalues in the following way:
            'fastest' :  0
            'fast'    :  10
            'normal'  :  6
            'slow'    :  3
            'slowest' :  1
        speeds from 1 to 10 enforce increasingly faster animation of
        line drawing and turtle turning.
        Attention:
        speed = 0 : *no* animation takes place. forward/back makes turtle jump
        and likewise left/right make the turtle turn instantly.
        Example (for a Turtle instance named turtle):
        >>> turtle.speed(3)
        """
        speeds = {'fastest':0, 'fast':10, 'normal':6, 'slow':3, 'slowest':1 }
        if speed is None:
            return self._speed
        if speed in speeds:
            speed = speeds[speed]
        elif 0.5 < speed < 10.5:
            speed = int(round(speed))
        else:
            speed = 0
        self.pen(speed=speed)

    def color(self, *args):
        """Return or set the pencolor and fillcolor.
        Arguments:
        Several input formats are allowed.
        They use 0, 1, 2, or 3 arguments as follows:
        color()
            Return the current pencolor and the current fillcolor
            as a pair of color specification strings as are returned
            by pencolor and fillcolor.
        color(colorstring), color((r,g,b)), color(r,g,b)
            inputs as in pencolor, set both, fillcolor and pencolor,
            to the given value.
        color(colorstring1, colorstring2),
        color((r1,g1,b1), (r2,g2,b2))
            equivalent to pencolor(colorstring1) and fillcolor(colorstring2)
            and analogously, if the other input format is used.
        If turtleshape is a polygon, outline and interior of that polygon
        is drawn with the newly set colors.
        For more info see: pencolor, fillcolor
        Example (for a Turtle instance named turtle):
        >>> turtle.color('red', 'green')
        >>> turtle.color()
        ('red', 'green')
        >>> colormode(255)
        >>> color((40, 80, 120), (160, 200, 240))
        >>> color()
        ('#285078', '#a0c8f0')
        """
        if args:
            l = len(args)
            if l == 1:
                pcolor = fcolor = args[0]
            elif l == 2:
                pcolor, fcolor = args
            elif l == 3:
                pcolor = fcolor = args
            pcolor = self._colorstr(pcolor)
            fcolor = self._colorstr(fcolor)
            self.pen(pencolor=pcolor, fillcolor=fcolor)
        else:
            return self._color(self._pencolor), self._color(self._fillcolor)

    def pencolor(self, *args):
        """ Return or set the pencolor.
        Arguments:
        Four input formats are allowed:
          - pencolor()
            Return the current pencolor as color specification string,
            possibly in hex-number format (see example).
            May be used as input to another color/pencolor/fillcolor call.
          - pencolor(colorstring)
            s is a Tk color specification string, such as "red" or "yellow"
          - pencolor((r, g, b))
            *a tuple* of r, g, and b, which represent, an RGB color,
            and each of r, g, and b are in the range 0..colormode,
            where colormode is either 1.0 or 255
          - pencolor(r, g, b)
            r, g, and b represent an RGB color, and each of r, g, and b
            are in the range 0..colormode
        If turtleshape is a polygon, the outline of that polygon is drawn
        with the newly set pencolor.
        Example (for a Turtle instance named turtle):
        >>> turtle.pencolor('brown')
        >>> tup = (0.2, 0.8, 0.55)
        >>> turtle.pencolor(tup)
        >>> turtle.pencolor()
        '#33cc8c'
        """
        if args:
            color = self._colorstr(args)
            if color == self._pencolor:
                return
            self.pen(pencolor=color)
        else:
            return self._color(self._pencolor)

    def fillcolor(self, *args):
        """ Return or set the fillcolor.
        Arguments:
        Four input formats are allowed:
          - fillcolor()
            Return the current fillcolor as color specification string,
            possibly in hex-number format (see example).
            May be used as input to another color/pencolor/fillcolor call.
          - fillcolor(colorstring)
            s is a Tk color specification string, such as "red" or "yellow"
          - fillcolor((r, g, b))
            *a tuple* of r, g, and b, which represent, an RGB color,
            and each of r, g, and b are in the range 0..colormode,
            where colormode is either 1.0 or 255
          - fillcolor(r, g, b)
            r, g, and b represent an RGB color, and each of r, g, and b
            are in the range 0..colormode
        If turtleshape is a polygon, the interior of that polygon is drawn
        with the newly set fillcolor.
        Example (for a Turtle instance named turtle):
        >>> turtle.fillcolor('violet')
        >>> col = turtle.pencolor()
        >>> turtle.fillcolor(col)
        >>> turtle.fillcolor(0, .5, 0)
        """
        if args:
            color = self._colorstr(args)
            if color == self._fillcolor:
                return
            self.pen(fillcolor=color)
        else:
            return self._color(self._fillcolor)

    def showturtle(self):
        """Makes the turtle visible.
        Aliases: showturtle | st
        No argument.
        Example (for a Turtle instance named turtle):
        >>> turtle.hideturtle()
        >>> turtle.showturtle()
        """
        self.pen(shown=True)

    def hideturtle(self):
        """Makes the turtle invisible.
        Aliases: hideturtle | ht
        No argument.
        It's a good idea to do this while you're in the
        middle of a complicated drawing, because hiding
        the turtle speeds up the drawing observably.
        Example (for a Turtle instance named turtle):
        >>> turtle.hideturtle()
        """
        self.pen(shown=False)

    def isvisible(self):
        """Return True if the Turtle is shown, False if it's hidden.
        No argument.
        Example (for a Turtle instance named turtle):
        >>> turtle.hideturtle()
        >>> print turtle.isvisible():
        False
        """
        return self._shown

    def pen(self, pen=None, **pendict):
        """Return or set the pen's attributes.
        Arguments:
            pen -- a dictionary with some or all of the below listed keys.
            **pendict -- one or more keyword-arguments with the below
                         listed keys as keywords.
        Return or set the pen's attributes in a 'pen-dictionary'
        with the following key/value pairs:
           "shown"      :   True/False
           "pendown"    :   True/False
           "pencolor"   :   color-string or color-tuple
           "fillcolor"  :   color-string or color-tuple
           "pensize"    :   positive number
           "speed"      :   number in range 0..10
           "resizemode" :   "auto" or "user" or "noresize"
           "stretchfactor": (positive number, positive number)
           "shearfactor":   number
           "outline"    :   positive number
           "tilt"       :   number
        This dictionary can be used as argument for a subsequent
        pen()-call to restore the former pen-state. Moreover one
        or more of these attributes can be provided as keyword-arguments.
        This can be used to set several pen attributes in one statement.
        Examples (for a Turtle instance named turtle):
        >>> turtle.pen(fillcolor="black", pencolor="red", pensize=10)
        >>> turtle.pen()
        {'pensize': 10, 'shown': True, 'resizemode': 'auto', 'outline': 1,
        'pencolor': 'red', 'pendown': True, 'fillcolor': 'black',
        'stretchfactor': (1,1), 'speed': 3, 'shearfactor': 0.0}
        >>> penstate=turtle.pen()
        >>> turtle.color("yellow","")
        >>> turtle.penup()
        >>> turtle.pen()
        {'pensize': 10, 'shown': True, 'resizemode': 'auto', 'outline': 1,
        'pencolor': 'yellow', 'pendown': False, 'fillcolor': '',
        'stretchfactor': (1,1), 'speed': 3, 'shearfactor': 0.0}
        >>> p.pen(penstate, fillcolor="green")
        >>> p.pen()
        {'pensize': 10, 'shown': True, 'resizemode': 'auto', 'outline': 1,
        'pencolor': 'red', 'pendown': True, 'fillcolor': 'green',
        'stretchfactor': (1,1), 'speed': 3, 'shearfactor': 0.0}
        """
        _pd =  {"shown"         : self._shown,
                "pendown"       : self._drawing,
                "pencolor"      : self._pencolor,
                "fillcolor"     : self._fillcolor,
                "pensize"       : self._pensize,
                "speed"         : self._speed,
                "resizemode"    : self._resizemode,
                "stretchfactor" : self._stretchfactor,
                "shearfactor"   : self._shearfactor,
                "outline"       : self._outlinewidth,
                "tilt"          : self._tilt
               }

        if not (pen or pendict):
            return _pd

        if isinstance(pen, dict):
            p = pen
        else:
            p = {}
        p.update(pendict)

        _p_buf = {}
        for key in p:
            _p_buf[key] = _pd[key]

        if self.undobuffer:
            self.undobuffer.push(("pen", _p_buf))

        newLine = False
        if "pendown" in p:
            if self._drawing != p["pendown"]:
                newLine = True
        if "pencolor" in p:
            if isinstance(p["pencolor"], tuple):
                p["pencolor"] = self._colorstr((p["pencolor"],))
            if self._pencolor != p["pencolor"]:
                newLine = True
        if "pensize" in p:
            if self._pensize != p["pensize"]:
                newLine = True
        if newLine:
            self._newLine()
        if "pendown" in p:
            self._drawing = p["pendown"]
        if "pencolor" in p:
            self._pencolor = p["pencolor"]
        if "pensize" in p:
            self._pensize = p["pensize"]
        if "fillcolor" in p:
            if isinstance(p["fillcolor"], tuple):
                p["fillcolor"] = self._colorstr((p["fillcolor"],))
            self._fillcolor = p["fillcolor"]
        if "speed" in p:
            self._speed = p["speed"]
        if "resizemode" in p:
            self._resizemode = p["resizemode"]
        if "stretchfactor" in p:
            sf = p["stretchfactor"]
            if isinstance(sf, (int, float)):
                sf = (sf, sf)
            self._stretchfactor = sf
        if "shearfactor" in p:
            self._shearfactor = p["shearfactor"]
        if "outline" in p:
            self._outlinewidth = p["outline"]
        if "shown" in p:
            self._shown = p["shown"]
        if "tilt" in p:
            self._tilt = p["tilt"]
        if "stretchfactor" in p or "tilt" in p or "shearfactor" in p:
            scx, scy = self._stretchfactor
            shf = self._shearfactor
            sa, ca = math.sin(self._tilt), math.cos(self._tilt)
            self._shapetrafo = ( scx*ca, scy*(shf*ca + sa),
                                -scx*sa, scy*(ca - shf*sa))
        self._update()

## three dummy methods to be implemented by child class:

    def _newLine(self, usePos = True):
        """dummy method - to be overwritten by child class"""
    def _update(self, count=True, forced=False):
        """dummy method - to be overwritten by child class"""
    def _color(self, args):
        """dummy method - to be overwritten by child class"""
    def _colorstr(self, args):
        """dummy method - to be overwritten by child class"""

    width = pensize
    up = penup
    pu = penup
    pd = pendown
    down = pendown
    st = showturtle
    ht = hideturtle

