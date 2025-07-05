class _TurtleImage(object):
    """Helper class: Datatype to store Turtle attributes
    """

    def __init__(self, screen, shapeIndex):
        self.screen = screen
        self._type = None
        self._setshape(shapeIndex)

    def _setshape(self, shapeIndex):
        screen = self.screen
        self.shapeIndex = shapeIndex
        if self._type == "polygon" == screen._shapes[shapeIndex]._type:
            return
        if self._type == "image" == screen._shapes[shapeIndex]._type:
            return
        if self._type in ["image", "polygon"]:
            screen._delete(self._item)
        elif self._type == "compound":
            for item in self._item:
                screen._delete(item)
        self._type = screen._shapes[shapeIndex]._type
        if self._type == "polygon":
            self._item = screen._createpoly()
        elif self._type == "image":
            self._item = screen._createimage(screen._shapes["blank"]._data)
        elif self._type == "compound":
            self._item = [screen._createpoly() for item in
                                          screen._shapes[shapeIndex]._data]
