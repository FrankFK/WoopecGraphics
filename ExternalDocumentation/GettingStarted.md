# Getting Started

To be honest: Getting started with Visual Studio and C# may be a little bit more difficult than getting started 
with - for instance - python.

But: C# is a really great programming language, really big projects are implemented with C#.
And once you've gained a foothold, you have a huge ecosystem at your disposal, for example for web development 
or game development with Unity. Furthermore: Now is a good time to start, because the .NET library has been completely 
redeveloped and will continue to take off with .NET 6.

So give it a chance.

Here are the steps.

1. You need a Windows Computer.
2. Download and install the newest version of 
  [Visual Studio Community Edition](https://visualstudio.microsoft.com/de/vs/community/). It's free!
3. Take a few minutes to learn how you can build and start your first console application with Visual Studio. 
  For instance by viewing [Getting Started with Visual Studio 2019](https://www.youtube.com/watch?v=1CgsMtUmVgs&list=RDCMUChqrDOwARrxdJF-ykAptc7w)
4. Learn how to create a WPF app on .NET Core. You can watch [Create your first WPF app on .NET Core | .NET Core 101](https://www.youtube.com/watch?v=Y4pthq_zGvI)
  for this. The complete video is interesting, but for your first turtle program you only need to follow the first two
  minutes. After that you have an empty WPF-program that you can start by Visual Studio.
5. Find out, how you can add a Nuget package to your project. This is described in [Using a Nuget Package | .NET Core 101](https://www.youtube.com/watch?v=hYbe6sFYBDY).
  You only need the first five minutes to know what you need. But again: The complete video is interesting.
  (By the way: The complete video series [.NET Core 101](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oWoazjhXQzBKMrFuArxpW80)
  is a great introduction to C# development).

Now you are prepared to start using Woopec to code your first graphic programs:

Woopec is a nuget package. Go to your WPF-project (step 4) and add a nuget-package to it (step 5): 
You have to search for `Woopec.Wpf` and add this nuget package to your project.

Then you have to change the `MainWindow.xaml` of your project such that it uses Woopec:
* Add a reference to this package: `xmlns:woopec="clr-namespace:Woopec.Wpf;assembly=Woopec.Wpf"`
* Change `Height` and `Width` of the window to `850`.
* Add a WoopecCanvas to the grid: `<woopec:WoopecCanvas></woopec:WoopecCanvas>`

Now your MainWindow.xaml should look like this:
```
<Window x:Class="UsingWoopec.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:woopec="clr-namespace:Woopec.Wpf;assembly=Woopec.Wpf"
        xmlns:local="clr-namespace:UsingWoopec"
        mc:Ignorable="d"
        Title="MainWindow" Height="850" Width="850">
    <Grid>
        <woopec:WoopecCanvas></woopec:WoopecCanvas>
    </Grid>
</Window>
```

Finally add your turle-code to the MainWindow.xaml.cs (if you do not find it: select MainWindow.xaml in the solution explorer
 and press F7):
* Add `using Woopec.Core` to the head of the file
* Add a `TurtleMain()` method to your code.

Your code should look something like this:
```csharp
using System.Windows;
using Woopec.Core;

namespace UsingWoopec
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // My first turtle program
        public static void TurtleMain()
        {
            var turtle = Turtle.Seymour();
            turtle.Right(45);
            turtle.Forward(50);
            turtle.Left(90);
            turtle.Forward(100);
            turtle.Right(45);
            turtle.Forward(20);
        }
    }
}
```

Start the program (see step 3) and it draws your graphic:

![First graphic](./FirstSample.png)

As your programs get bigger, they run a little slow when they are started with debugging.
You can start them without debugging (in the menu: debug: start without debugging, or hitting Ctrl+F5)
and they will run much faster.

You can try this with one of the examples. For instance with `ParallelTurtles`:
```csharp
    public static void TurtleMain()
    {
        Woopec.Examples.ParallelTurtles.Run();
    }
```

It is slow when you start it with F5 (with debugging). But it should be much faster when you start it with 
Ctrl+F5 (without debugging).






