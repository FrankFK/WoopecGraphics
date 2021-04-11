# simple-graphics-for-csharp-beginners
Simple Graphics for C# Beginners (starting with Turtle-Graphics)

## Planned

* TurtleMain in separatem Thread, Outputs über Puffer mit WPF austauschen
  Mal hiermit versuchen: https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/
  Beispiel: https://www.davidguida.net/how-to-implement-producer-consumer-with-system-threading-channels/
* Animation in WPF einbauen
* TurtleMain über Reflection finden und automatisch aufrufen
  https://stackoverflow.com/questions/42524704/asp-net-core-find-all-class-types-in-all-assemblies/44444309
  https://stackoverflow.com/questions/1315665/c-list-all-classes-in-assembly
  `Assembly myAssembly = Assembly.GetExecutingAssembly();`

## Done

21h 

* 04.04.2021: Basic turtle movements
* 06.04.2021: Learned: Line-Drawing and Animations in WPF
* 08.04.2021: First turtle-moves visible in WPF
* 11.04.2021: Got an overview of the turtle functionality in python 
              (see [state.md](State.md), it is a lot!). 
              I do not want to make a C# reimplementation of pyhton-turtle. My first 
              milestone is a subset of the python-commands. For these commands I will 
              try to use the same class- and method-names. But if I think to have good reasons to
              make things different, I will make things different.
