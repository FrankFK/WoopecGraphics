class Shape(object):
    """Data structure modeling shapes.
    attribute _type is one of "polygon", "image", "compound"
    attribute _data is - depending on _type a poygon-tuple,
    an image or a list constructed using the addcomponent method.
    """
    def __init__(self, type_, data=None):
        self._type = type_
        if type_ == "polygon":
            if isinstance(data, list):
                data = tuple(data)
        elif type_ == "image":
            if isinstance(data, str):
                if data.lower().endswith(".gif") and isfile(data):
                    data = TurtleScreen._image(data)
                # else data assumed to be Photoimage
        elif type_ == "compound":
            data = []
        else:
            raise TurtleGraphicsError("There is no shape type %s" % type_)
        self._data = data

    def addcomponent(self, poly, fill, outline=None):
        """Add component to a shape of type compound.
        Arguments: poly is a polygon, i. e. a tuple of number pairs.
        fill is the fillcolor of the component,
        outline is the outline color of the component.
        call (for a Shapeobject namend s):
        --   s.addcomponent(((0,0), (10,10), (-10,10)), "red", "blue")
        Example:
        >>> poly = ((0,0),(10,-5),(0,10),(-10,-5))
        >>> s = Shape("compound")
        >>> s.addcomponent(poly, "red", "blue")
        >>> # .. add more components and then use register_shape()
        """
        if self._type != "compound":
            raise TurtleGraphicsError("Cannot add component to %s Shape"
                                                                % self._type)
        if outline is None:
            outline = fill
        self._data.append([poly, fill, outline])

