﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

//
// TurtleCore is GUI-agnostic. The (actual) GUI is implemented in a seperate project: TurtleWpf.
// TurtleWpf needs access to some parts of TurtleCore (for instance to ITurtleOutput). These parts
// should not be visible to Turtle-Users. Therfore these parts are declared as internal and made
// visible to TurtleWpf
// 
[assembly: InternalsVisibleTo("TurtleWpf")]
[assembly: InternalsVisibleTo("TurtleCore.UnitTests")]

