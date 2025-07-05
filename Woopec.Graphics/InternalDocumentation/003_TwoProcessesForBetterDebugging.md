# ADR 003 Use two processes to make debugging easy
<!-- Short Title -->

## Status
<!-- What is the status, such as proposed, accepted, rejected, deprecated, superseded, etc.? -->
Proposed, 03-21-2022

## Context
<!-- What is the issue that we're seeing that is motivating this decision or change? -->
For users who want to debug their turtle code, it should be as easy as possible to do this. 

The first implementation of the WPF-Version of Woopec works like this:

* Turtle code and WPF code run in one process. 
* The WPF code runs in one thread and starts a second thread which executes the turtle code
* Communication between the WPF-thread and the turtle-thread is done by System.Threading.Channels (see [An Introduction to System.Threading.Channels](https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/) 

This works fine and fast, but debugging of the turtle code is no fun:

* Take an example with two lines of code:

  ```c#
  var seymour = Turtle.Seymour();
  seymour.Forward(100);
  seymour.Right(60);
  ```

* If the user tries to debug this code and puts a breakpoint at the second line and then the user steps with the debugger to the next line, the user sees no changes in the WPF window!
  The reason for that is that the WPF-thread is "stopped" as long as the user debugs through the turtle thread. We could try to put Thread.Sleep in the turtle code (e.g. a `Thread.Sleep(500)` between the `Forward` and the `Right` command) which give the WPF-thread a little bit time to show the turtle animations, but if these sleeps are too short, the WPF-animations would stop before they are finished. 

* An experienced user would be able to debug this by freezing the turtle thread (see [Debug a multithreaded app - Visual Studio](https://docs.microsoft.com/en-us/visualstudio/debugger/how-to-use-the-threads-window?view=vs-2022)). But this is not easy enough for a user who wants to learn programming.

## Decision
<!-- What is the change that we're proposing and/or doing? -->
For debugging It is better to have two processes:

* A WPF-process which shows the turtle animations
* A Turtle-process with the turtle code. 

The user can debug the turtle process and this won't effect the drawing by the WPF-process. 



## Consequences
<!-- What becomes easier or more difficult to do because of this change? -->

Pros:

* For unexperienced users it would be much easier to debug their code.
* This solution will also give as the opportunity to complete decouple the turtle-code from the presentation-code (WPF) in the future.

Neutral:

* The "debug-version" with two processes will be slower than the "non-debug-version" with two threads. That is no problem, because we will provide both versions.

Cons:

* We provide two versions: A debug-version which works with processes, and a non-debug-version which works with threads. Both versions have to be tested and maintained.


## Notes

The non-debug version works as before: WPF code in one thread, turtle code in another thread, communication with System.Threading.Channels.

The debug-version works like this: If the process is running with `Debugger.IsAttached` a second process of the same executable is started. This second process renders the turtle objects on WPF. The first process sends the information which data has to be rendered to the second process via an anonymous pipe.

Details:

* Why is the "render-process" a copy of the executable of the "turtle-code-process"? It would be nice to have a generic "render-executable", which can be used for the render-process. Then the solution will work the same way as python turtle works. But at the moment I have no idea how I can easily install this "render-executable" on the users computer. I can do that sometime later. Project `UsingTurtleCore.csproj` contains an experimental version which only works on my machine. This project is a standard C# Console program (no is WPF needed) and the following `Program.cs` works:
  ```c#
  using Woopec.Core;
  
  Turtle.ExperimentalInit();
  
  var turtle = Turtle.Seymour();
  
  turtle.Right(45);
  turtle.Forward(50);
  ```

* The communication between the two processes is done by anonymous pipes (see: [How to: Use Anonymous Pipes for Local Interprocess Communication | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-use-anonymous-pipes-for-local-interprocess-communication))

* For sending the objects through a pipe, we have to serialize and deserialize the objects. The `System.Text.Json` serializer is used for that. This was a little bit complicated because the code has several polymorphic objects (`ScreenObject, ShapeBase` and `ScreenAnimationEffect`) that are even nested. `System.Text.Json` cannot handle this out of the box.

  * The solution uses ideas from [c# - Is polymorphic deserialization possible in System.Text.Json? - Stack Overflow](https://stackoverflow.com/questions/58074304/is-polymorphic-deserialization-possible-in-system-text-json), [How to write custom converters for JSON serialization - .NET | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-6-0#support-polymorphic-deserialization) and [Recursive Polymorphic Deserialization with System.Text.Json | Ben Gribaudo](https://bengribaudo.com/blog/2022/02/22/6569/recursive-polymorphic-deserialization-with-system-text-json)
  * I have created one class `ProcessChannelConverter<T>` which handles all general stuff. The constructor of this class has three delegate-arguments which  handle the stuff that is specific for a type `T`. For instance
    `var shapeBaseConverter = new ProcessChannelConverter<ShapeBase>(ShapeBase.JsonTypeDiscriminatorAsInt, ShapeBase.JsonWrite, ShapeBase.JsonRead);`
  * Inside the implementation of `ProcessChannelConverter<T>` it was a little bit tricky to get it working for the nested polymorphic objects (`ScreenObject` is polymorphic and has properties of type `ShapeBase` and `ScreenAnimationEffect` which are polymorphic too)). See implementation of `Write` method in `ProcessChannelConverter<T>` for details.

* The turtle-process writes successively serialized objects to the pipe. The render-process has to read and deserialize them successively too. At first I thought it was possible to do that with `Deserializer.DeserializeAsync(Stream utf8Json, ...)`. But this did not work because this method wants to read until the end of the stream is reached. The actual implementation separates the serialized objects with a new-line. The render-process reads a single line into a string and deserializes the actual object from this string.

* After writing a serialized object into the pipe, it is important to flush the content so that the reading process receives the content immediately.

* For serialization a few properties have to be made public (see: [System.Text.Json - How to serialize non-public properties (makolyte.com)](https://makolyte.com/system-text-json-how-to-serialize-non-public-properties/)

