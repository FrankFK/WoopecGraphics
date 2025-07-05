using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;

namespace AvaloniaTestConsole.WoopecCoreConnection
{
    enum Operation
    {
        Add,
        Remove,
        Nothing,
    }

    record CanvasChildrenChange(Operation Operation, Shape Element);
}
