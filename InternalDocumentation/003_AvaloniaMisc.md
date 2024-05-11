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
* Ziemlich ausführliche Übersicht: [Multiplatform UI Coding with AvaloniaUI in Easy Samples. Part 1 - AvaloniaUI Building Blocks - CodeProject](https://www.codeproject.com/Articles/5308645/Multiplatform-UI-Coding-with-AvaloniaUI-in-Easy-Sa)

Nächste Ziele:

* Animation

  * Linien-Endpoint kann man mit Keyframe Animation animieren:
  * ```xml
            <Line StartPoint="0,0" EndPoint="30,115" Stroke="Red" StrokeThickness="1">
                <Line.Styles>
                    <Style Selector="Line">
                        <Style.Animations>
                            <Animation Duration="0:0:3">
                                <KeyFrame Cue="0%">
                                    <Setter Property="EndPoint" Value="0,0"></Setter>
                                </KeyFrame>
                                <KeyFrame Cue="100%">
                                    <Setter Property="EndPoint" Value="30,115"></Setter>
                                </KeyFrame>
                            </Animation>
                        </Style.Animations>
                    </Style>
                </Line.Styles>
            </Line>
    ```
  * Einen Path verschieben oder rotieren mit einer Animation
  * Ich muss wissen, wann die Animation beendet ist

    * In Wpf mache ich das mit `Completed` Eventhandler. In Avalonia habe ich so etwas nicht gefunden.
    * Man kann aber wohl die Property `IsAnimated` abfragen, um zu erfahren ob die Animation beendet ist.
    * Das wird knifflig. Hintergrund: Zum Beispiel macht die Turtle hintereinander forward 100, left 90, backward100. Dann wird zuerst das forward 100 gemacht, währenddessen stehen left 90 und backward 100 in einer Warteschlange. Ich brauche einen Trigger, der mir anzeigt, dass das forward fertig ist. Ich kann das auch per idle waiting mit IsAnimated prüfen und dann selber triggern, aber schön ist das nicht. Wie häufig prüfe ich dann? Zwei Ideen:

      * Ich hoffe, dass Avalonia die Animation wie gewünscht abspielt, setze mir einen Timer und lasse den Timer das Event verschicken.
      * Ich ändere den Ansatz komplett und schicke die wartenden Animationen auch schon an Avalonia mit dem passenden Delay wann sie starten sollen. Dieser Ansatz wäre vermutlich näher an svg animationen
      * **Mein aktueller Favorit**: Ich programmiere eine eigene Ease Klasse, setze die als Animation.Easing und hoffe, dass die Klasse am Ende immer mit 1.0 aufgerufen wird. Das könnte funktionieren, weil Avalonia beim letzten Aufruf wegen Math.Min vermutlich eine 1 übergibt, siehe Avalonia Code: Avalonia.Base\Rendering\Composition\Animations\KeyFrameAnimationInstance.cs:
      * ```csharp
        var keyProgress = Math.Max(0, Math.Min(1, (iterationProgress - left.Key) / (right.Key - left.Key)));
        var easedKeyProgress = (float)right.EasingFunction.Ease(keyProgress);
        ```

  * Animationen per Code erzeugen

    * Beispiele eventuell von hier: [Avalonia/tests/Avalonia.Base.UnitTests/Animation at master · AvaloniaUI/Avalonia (github.com)](https://github.com/AvaloniaUI/Avalonia/tree/master/tests/Avalonia.Base.UnitTests/Animation)

    * Es hat mich ein zwei Stunden gekostet, festzustellen, dass man einen Setter unbedingt so erzeugen muss
      ```csharp
      var setter = new Setter(TranslateTransform.XProperty, 0d)
      ```

      und nicht so:

      ```csharp
      var setter = new Setter();
      setter.Property = TranslateTransform.XProperty;
      setter.Value = 0d;
      ```

      In dieser Variante geht massiv etwas schief. Das Rechteck wird gar nicht angezeigt und erst recht nicht animiert.
    
    * **Status** Es funktioniert jetzt, geholfen haben mir dabei auch die UnitTests aus dem Avalonia Projekt.

* Animierte Linie (entspricht turle.forward) zeichnen, analog zu C:\Users\frank\source\repos\simple-graphics-for-csharp-beginners\TurtleWpf\CanvasLines.cs

  * **Status** Animierte Linie und animiertes Rechteck funktioniert im Test-Programm
  * Darin: Koordinaten-Handling und -Umrechnung, Farb-Umrechnung
* Dann den schwierigen Teil: Kommunikation zwischen Core und Avalonia 
  * **Status: ** 
    * Am Ende ähnlich wie in Wpf. Der Dispatcher von Avalonia heißt nur etwas anders und hat andere Methoden
    * Die Magic aus TurtleWpf\WoopecCanvas.xaml.cs kann man ähnlich übertragen (man braucht aber Hirnakrobatik um es zu verstehen)
    * Noch nicht gekümmert: Handling bei Exceptions. Das funktioniert allerdings bei WPF aktuell auch noch nicht.

Avalonia ohne Gedöns nutzen

* **Status**:
  
  * In C:\Users\frank\source\repos\Avalonia.Samples\ gibt es sehr viele Beispiel-Projekte. Analog dazu konnte ich ein Projekt erstellen vom Template Consolen-Programm, dann mehre Avalonia-Packages hinzufügen. Dann die Quellen der beiden Projekte aus dem Avalonia-Template in dieses Console-Programm, noch beim Asset/Icon in den Properties angeben, dass es eine AvaloniaResource ist. Danach lief es.
  * Ob das noch einfacher geht, so wie hier? 
  
      * HIer [Add a basic, standalone xaml-free Hello World example to the docs · AvaloniaUI/Avalonia · Discussion #9006 (github.com)](https://github.com/AvaloniaUI/Avalonia/discussions/9006) hat jemand das auch versucht. Doku dazu hat er nicht gefunden. Aber anscheinend hat er es so geschafft:
        ```csharp
        Application app = Application.Current ?? AppBuilder.Configure<Application>().UsePlatformDetect().SetupWithoutStarting().Instance;
        app.Styles.Add(new DefaultTheme());
        app.Run(new Window() { Title = "Avalonia Basic Example", Content = "Hello Avalonia!" });
        ```
  * Als nächstes müsste ich versuchen, aus dem Ganzen - so wie in Wpf - ein Projekt (Library?) mit einem UserControl zu machen, so dass man das dann später mal zu einem Package machen kann.
  
      
  
  





## Status

## Kontext
## Entscheidung
## Konsequenzen
### Konzept

### Lösung

