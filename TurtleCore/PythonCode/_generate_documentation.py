def write_docstringdict(filename="turtle_docstringdict"):
    """Create and write docstring-dictionary to file.
    Optional argument:
    filename -- a string, used as filename
                default value is turtle_docstringdict
    Has to be called explicitly, (not used by the turtle-graphics classes)
    The docstring dictionary will be written to the Python script <filname>.py
    It is intended to serve as a template for translation of the docstrings
    into different languages.
    """
    docsdict = {}

    for methodname in _tg_screen_functions:
        key = "_Screen."+methodname
        docsdict[key] = eval(key).__doc__
    for methodname in _tg_turtle_functions:
        key = "Turtle."+methodname
        docsdict[key] = eval(key).__doc__

    with open("%s.py" % filename,"w") as f:
        keys = sorted(x for x in docsdict
                      if x.split('.')[1] not in _alias_list)
        f.write('docsdict = {\n\n')
        for key in keys[:-1]:
            f.write('%s :\n' % repr(key))
            f.write('        """%s\n""",\n\n' % docsdict[key])
        key = keys[-1]
        f.write('%s :\n' % repr(key))
        f.write('        """%s\n"""\n\n' % docsdict[key])
        f.write("}\n")
        f.close()

def read_docstrings(lang):
    """Read in docstrings from lang-specific docstring dictionary.
    Transfer docstrings, translated to lang, from a dictionary-file
    to the methods of classes Screen and Turtle and - in revised form -
    to the corresponding functions.
    """
    modname = "turtle_docstringdict_%(language)s" % {'language':lang.lower()}
    module = __import__(modname)
    docsdict = module.docsdict
    for key in docsdict:
        try:
#            eval(key).im_func.__doc__ = docsdict[key]
            eval(key).__doc__ = docsdict[key]
        except Exception:
            print("Bad docstring-entry: %s" % key)

_LANGUAGE = _CFG["language"]

try:
    if _LANGUAGE != "english":
        read_docstrings(_LANGUAGE)
except ImportError:
    print("Cannot find docsdict for", _LANGUAGE)
except Exception:
    print ("Unknown Error when trying to import %s-docstring-dictionary" %
                                                                  _LANGUAGE)


def getmethparlist(ob):
    """Get strings describing the arguments for the given object
    Returns a pair of strings representing function parameter lists
    including parenthesis.  The first string is suitable for use in
    function definition and the second is suitable for use in function
    call.  The "self" parameter is not included.
    """
    defText = callText = ""
    # bit of a hack for methods - turn it into a function
    # but we drop the "self" param.
    # Try and build one for Python defined functions
    args, varargs, varkw = inspect.getargs(ob.__code__)
    items2 = args[1:]
    realArgs = args[1:]
    defaults = ob.__defaults__ or []
    defaults = ["=%r" % (value,) for value in defaults]
    defaults = [""] * (len(realArgs)-len(defaults)) + defaults
    items1 = [arg + dflt for arg, dflt in zip(realArgs, defaults)]
    if varargs is not None:
        items1.append("*" + varargs)
        items2.append("*" + varargs)
    if varkw is not None:
        items1.append("**" + varkw)
        items2.append("**" + varkw)
    defText = ", ".join(items1)
    defText = "(%s)" % defText
    callText = ", ".join(items2)
    callText = "(%s)" % callText
    return defText, callText

def _turtle_docrevise(docstr):
    """To reduce docstrings from RawTurtle class for functions
    """
    import re
    if docstr is None:
        return None
    turtlename = _CFG["exampleturtle"]
    newdocstr = docstr.replace("%s." % turtlename,"")
    parexp = re.compile(r' \(.+ %s\):' % turtlename)
    newdocstr = parexp.sub(":", newdocstr)
    return newdocstr

def _screen_docrevise(docstr):
    """To reduce docstrings from TurtleScreen class for functions
    """
    import re
    if docstr is None:
        return None
    screenname = _CFG["examplescreen"]
    newdocstr = docstr.replace("%s." % screenname,"")
    parexp = re.compile(r' \(.+ %s\):' % screenname)
    newdocstr = parexp.sub(":", newdocstr)
    return newdocstr
