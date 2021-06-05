using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Woopec.Wpf
{
    enum Operation
    {
        Add,
        Remove,
        Nothing,
    }

    record CanvasChildrenChange(Operation Operation, UIElement Element);
}
