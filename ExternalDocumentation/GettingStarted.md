# Getting Started with turtle graphics in C#

To be honest: Getting started with Visual Studio and C# may be a little bit more difficult than getting started with - for instance - python.

But: C# is a really great programming language, really big projects are implemented with C#. And once you've gained a foothold, you have a huge ecosystem at your disposal, for example for web development or game development with Unity.

So give it a chance.

At first you have to prepare your computer for it:

1. You need a Windows Computer.

2. Download and install the newest version of  [Visual Studio Community Edition](https://visualstudio.microsoft.com/de/vs/community/). It's free!
   
3. Take five minutes to learn how you can build and start your first console application with Visual Studio. View [Getting Started with Visual Studio 2019](https://www.youtube.com/watch?v=1CgsMtUmVgs&list=RDCMUChqrDOwARrxdJF-ykAptc7w) for this.

4. Close Visual Studio and open it again. In the first screen choose the link "Continue without code".

5. In Visual Studio choose the menu item `Tools -> Command Line -> Developer Command Prompt` to open a command window. In the command window type this command: 

    ```sh
    dotnet new --install Woopec.Templates
    ```

    Press the Enter-key and close the command window.

Now your computer is ready for programming turtle-programs. 

Follow the steps of [Getting Started with Visual Studio 2019](https://www.youtube.com/watch?v=1CgsMtUmVgs&list=RDCMUChqrDOwARrxdJF-ykAptc7w) to create a new project, but instead of choosing the "Console App" template search for for the template "Woopec Turtle WPF Project" and choose this template.

If you select the file `Program.cs` in the solution folder you see the following code:

```c#
using Woopec.Core;

namespace Project1
{
    internal class Program
    {
        public static void TurtleMain()
        {
            var seymour = Turtle.Seymour();

            seymour.Left(45);
            seymour.Forward(100);
            seymour.Right(45);
            seymour.Forward(50);
       }
    }
}
```

This program gets a turtle with name Seymour and moves this turtle a little bit. Start the program by pressing F5 and you see the turtle moving.

Now close the turtle window and try to change the program code such that it creates this graphic:

![First graphic](./FirstSample.png)

Start the program again to see if the result is OK. Your first turtle program is finished! 



# A few more tips

### Speed

As your programs get bigger, they run a little slow when they are started in debug mode (F5). You can start them without debugging (in the menu: debug: start without debugging, or hitting Ctrl+F5) and they will run much faster.

You can try this with one of the examples. For instance with `ParallelTurtles`:

```csharp
    public static void TurtleMain()
    {
        Woopec.Examples.ParallelTurtles.Run();
    }
```

It is slow when you start it with F5 (with debugging). But it should be much faster when you start it with Ctrl+F5 (without debugging).



### Intellisense

For all Woopec methods you can see a documentation in Visual Studio Intellisense. Look into the video  [Getting Started with Visual Studio 2019](https://www.youtube.com/watch?v=1CgsMtUmVgs&list=RDCMUChqrDOwARrxdJF-ykAptc7w) to learn how to use Intellisense



### Embed Woopec Graphics into an WPF project

In the example above the "Woopec WPF Project" template was used. If you do not want to use this template you can create a WPF project and embed Woopec Graphics into it:

* Add the nuget-package "Woopec.Wpf" to the dependencies of your project.

* Add the Woopec.Canvas to your window. Something like that:

  ```
  <Window x:Class="UsingWoopec.MainWindow"
          ...
          xmlns:local="clr-namespace:UsingWoopec"
          ...
          Title="MainWindow" Height="850" Width="850">
      <Grid>
          <woopec:WoopecCanvas></woopec:WoopecCanvas>
      </Grid>
  </Window>
  ```

* Add the method `public static void TurtleMain()` to one of your classes and put your turtle code into this method.
  



