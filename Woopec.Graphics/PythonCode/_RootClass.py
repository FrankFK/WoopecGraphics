class _Root(TK.Tk):
    """Root class for Screen based on Tkinter."""
    def __init__(self):
        TK.Tk.__init__(self)

    def setupcanvas(self, width, height, cwidth, cheight):
        self._canvas = ScrolledCanvas(self, width, height, cwidth, cheight)
        self._canvas.pack(expand=1, fill="both")

    def _getcanvas(self):
        return self._canvas

    def set_geometry(self, width, height, startx, starty):
        self.geometry("%dx%d%+d%+d"%(width, height, startx, starty))

    def ondestroy(self, destroy):
        self.wm_protocol("WM_DELETE_WINDOW", destroy)

    def win_width(self):
        return self.winfo_screenwidth()

    def win_height(self):
        return self.winfo_screenheight()
