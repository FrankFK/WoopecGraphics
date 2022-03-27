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
* The WPF code runs in one thread and starts a second thread, which executes the turtle code
* Communication between the WPF-thread and the turtle-thread is done by System.Threading.Channels (see [An Introduction to System.Threading.Channels](https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/) 

This works fine and fast, but debugging of the turtle code is no fun:

* Take an example with two lines of code:

  ```c#
  var seymour = Turtle.Seymour();
  seymour.Forward(100);
  seymour.Right(60);
  ```

* If the user puts a breakpoint to the second line and than steps to the following lines with the debugger, the user sees no changes in the WPF window.

* The reason for that is that the WPF-thread is "stopped" as long as the user debugs through the turtle thread. We could try to put Thread.Sleeps in the turtle code (e.g. a `Thread.Sleep(500)` between the `Forward` and the `Right` command) which give the WPF-thread a little bit time to show the turtle animations, but if these sleeps are too short, the WPF-animations would stop before they are finished. 

* An experienced user would be able to debug this by freezing the turtle thread (see [Debug a multithreaded app - Visual Studio](https://docs.microsoft.com/en-us/visualstudio/debugger/how-to-use-the-threads-window?view=vs-2022)). But this is not easy enough for a user who wants to learn programming.

## Decision
<!-- What is the change that we're proposing and/or doing? -->
For debugging It is better to have two processes 

* A WPF-process which shows the turtle animations
* A Turtle-process with the turtle code. 

The user can debug the turtle process and this won't effect the drawing by the WPF-process.



## Consequences
<!-- What becomes easier or more difficult to do because of this change? -->

Draft:

* Use [NamedPipeClientStream Class (System.IO.Pipes) | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/api/system.io.pipes.namedpipeclientstream?view=net-6.0) 
* Performance of two process version will be slower than the singe process version 
* Perhaps we make it possible for the user to choose between two versions. But this will make code maintenance more complex.