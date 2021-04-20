# simple-graphics-for-csharp-beginners
Simple Graphics for C# Beginners (starting with Turtle-Graphics)

## Planned

* ScreenOutput umstellen auf: ScreenObjectBroker, ScreenObjectProducer, SreenObjectConsumer and ScreenObjectWriter
  Kompliziertere Fälle behandeln.
  Die Lösung dokumentieren. 
  Channels sind hier dokumentiert:
     Mal hiermit versuchen: https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/
     Beispiel: https://www.davidguida.net/how-to-implement-producer-consumer-with-system-threading-channels/
  Der Ansatz mit dem WPF-Dispose kommt von hier:
       https://igorpopov.io/2018/06/16/asynchronous-programming-in-csharp-with-wpf/
* Animation in WPF einbauen
* Mapping der Koordinaten von (Turtle) Screen-Koordinaten auf (Wpf) Screen-Output-Koordinaten
* Die Screen* Klassen prüfen, ob das nicht besser Records wären
* Color <- Ich möchte keine Abhängigkeit zu WPF haben. Daher kann ich System.Windows.Media.Color nicht direkt nutzen
* ScreenLine.Width
* TurtleMain über Reflection finden und automatisch aufrufen
  https://stackoverflow.com/questions/42524704/asp-net-core-find-all-class-types-in-all-assemblies/44444309
  https://stackoverflow.com/questions/1315665/c-list-all-classes-in-assembly
  `Assembly myAssembly = Assembly.GetExecutingAssembly();`

## To decide
* Vielleicht kann man eine gemeinsame Basisklasse für Pen und Form machen?

## Done

27h 

* 04.04.2021: Basic turtle movements
* 06.04.2021: Learned: Line-Drawing and Animations in WPF
* 08.04.2021: First turtle-moves visible in WPF
* 11.04.2021: Got an overview of the turtle functionality in python 
              (see [state.md](State.md), it is a lot!). 
              I do not want to make a C# reimplementation of pyhton-turtle. My first 
              milestone is a subset of the python-commands. For these commands I will 
              try to use the same class- and method-names. But if I think to have good reasons to
              make things different, I will make things different.
* 13.04.2021: Started with class-design for Pens, Forms, ScreenObjects and ScreenAnimations
* 17.04.2021: Principally working: TurtleThread writes ScreenObjects into channel, WPF-thread reads ScreenObjects from the channel and drwas them animated
* 20.04.2021: Channel-communication classes: ScreenObjectBroker, ScreenObjectProducer, SreenObjectConsumer and ScreenObjectWriter. And the first unit test for this is green.
 

