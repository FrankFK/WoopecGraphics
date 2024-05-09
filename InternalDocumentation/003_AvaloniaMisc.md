# ADR 003 Avalonia work in progress
# Notizen

Erst mal reinkommen:

* Das Get Started [Install | Avalonia Docs (avaloniaui.net)](https://docs.avaloniaui.net/docs/get-started/install) benötigt sieben Templates. Das will ich am Ende natürlich nicht. Aber zum Kennenlernen der Basics vielleicht erst mal besser.
* Also `dotnet new install Avalonia.Templates` hat funktioniert.
* Dann [Set Up an Editor | Avalonia Docs (avaloniaui.net)](https://docs.avaloniaui.net/docs/get-started/set-up-an-editor) Avalonia Visual Studio 2022 Extension installiert
* Dann das Beispiel gemacht. 
* Es gibt Unterschiede zu WPF, zum Beispiel
  * [UIElement, FrameworkElement and Control | Avalonia Docs (avaloniaui.net)](https://docs.avaloniaui.net/docs/get-started/wpf/uielement-frameworkelement-and-control)
  * [Class Handlers | Avalonia Docs (avaloniaui.net)](https://docs.avaloniaui.net/docs/get-started/wpf/class-handlers)
  * Transforms haben einen anderen Default-Origin: [RenderTransforms and RenderTransformOrigin | Avalonia Docs (avaloniaui.net)](https://docs.avaloniaui.net/docs/get-started/wpf/rendertransforms-and-rendertransformorigin). Man kann den RenderTransformOrigin aber explizit angeben als RelativePoint.TopLeft. Dann sollte es wie in Wpf sein.
* Das, was ich brauche ist etwas weg vom Mainstream (DataBinding, Styles, etc.), aber auch für mich gibt es noch Dinge in der Doku. Stichpunkte aus den Abschnitten:
  * [How To Draw Graphics | Avalonia Docs (avaloniaui.net)](https://docs.avaloniaui.net/docs/guides/graphics-and-animation/graphics-and-animations)
    * device-independent pixel, which is 1/96th of an inch,
    * By default Avalonia uses the [Skia rendering engine](https://skia.org/),
    * Beispiel für 2D shapes such as `Ellipse`, `Line`, `Path`, `Polygon` and `Rectangle`.
    * Avalonia uses a CSS-like animation system which supports [property transitions](https://docs.avaloniaui.net/docs/guides/graphics-and-animation/transitions) and [keyframe animations](https://docs.avaloniaui.net/docs/guides/graphics-and-animation/keyframe-animations).
  * [How To Use Keyframe Animations | Avalonia Docs (avaloniaui.net)](https://docs.avaloniaui.net/docs/guides/graphics-and-animation/keyframe-animations)
    *  keyframe work in CSS, you will recognise the similarity with how they are done in in Avalonia UI.
    * Man kann auch zwei Properties gleichzeitig animieren
    * The fill mode attribute of an animation defines how the properties being set will persist after it runs, or during any gaps between runs
  * [How To Use Transitions | Avalonia Docs (avaloniaui.net)](https://docs.avaloniaui.net/docs/guides/graphics-and-animation/transitions)
    * Transitions in Avalonia are also heavily inspired by CSS Animations. They listen to any changes in target property's value and subsequently animates the change according to its parameters
    * Transitions kann man nicht mit den WPF-artigen Transforms kombinieren (keine Ahnung, ob das für mich relevant ist): "Avalonia also supports WPF-style render transforms such as `RotateTransform`, `ScaleTransform`, etc. These transforms cannot be transitioned: always use the CSS-like format if you want to apply a transition to a render transform."

Nächste Ziele:

* Animierte Linie (entspricht turle.forward) zeichnen, analog zu C:\Users\frank\source\repos\simple-graphics-for-csharp-beginners\TurtleWpf\CanvasLines.cs
  * Darin: Koordinaten-Handling und -Umrechnung, Farb-Umrechnung
* Dann den schwierigen Teil: Kommunikation zwischen Core und Avalonia 
  * Die magic aus C:\Users\frank\source\repos\simple-graphics-for-csharp-beginners\TurtleWpf\WoopecCanvas.xaml.cs übersetzen nach Avalonia
  * Eventuell geht das aber auch einfacher, ich habe doch schon mal irgendwo ein Avalonia Programm gesehen, was aus der Konsole etwas öffnet und Daten anzeigt. Weiß nur nicht mehr wo.
  * Darin: Event-Handling bei beendeten Animations



Avalonia ohne Gedöns nutzen

* HIer [Add a basic, standalone xaml-free Hello World example to the docs · AvaloniaUI/Avalonia · Discussion #9006 (github.com)](https://github.com/AvaloniaUI/Avalonia/discussions/9006) hat jemand das auch versucht. Doku dazu hat er nicht gefunden. Aber anscheinend hat er es so geschafft:
  ```csharp
  Application app = Application.Current ?? AppBuilder.Configure<Application>().UsePlatformDetect().SetupWithoutStarting().Instance;
  app.Styles.Add(new DefaultTheme());
  app.Run(new Window() { Title = "Avalonia Basic Example", Content = "Hello Avalonia!" });
  ```

  





## Status

## Kontext
## Entscheidung
## Konsequenzen
### Konzept

### Lösung

