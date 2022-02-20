# simple-graphics-for-csharp-beginners
Simple Graphics for C# Beginners (starting with Turtle-Graphics)


## Planned

* Erste Version nutzbar machen (MVP)
  * xml-Kommentare so einstellen, dass sie auch im nuget-package vorhanden sind
  * Wordpress-Seiten für die Hilfe-Seiten anlegen.
  * Nuget-Package mit einer Version ohne "-alpha" öffentlich machen (solange ist es nur als Prerelease gelistet und taucht im nuget-Browser nur mit gesetztem Prerelease-Haken auf)

* Hilfe-Seite mit Details zum Umgang mit mehreren Turtles

* Exceptions müssen abgefangen werden -- aktuell stürzt dann das Programm ab.
  * https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?redirectedfrom=MSDN&view=net-5.0*

* Wenn man es ganz extrem treiben will, kann man auch versuchen woopec. als Präfix auf nuget zu reservieren: https://docs.microsoft.com/de-de/nuget/nuget-org/id-prefix-reservation 
* Code auf github kann erst mal noch privat bleiben.


* Open-Data erfahrbar machen. Idee
  * Es gibt einen Rest-Service, mit dem man den aktuellen Stand von Tube-Linien in London abfragen kann: https://tfl.gov.uk/info-for/open-data-users/our-open-data?intcmp=3671#on-this-page-2
  * Details hier: https://content.tfl.gov.uk/trackernet-data-services-guide-beta.pdf
  * Man könnte es mit dem Train Predictions Service versuchen
  * Feed wird alle 30 Sekunden aktualisiert, man benötigt einen api-Key
  * Daraus eine "App" entwickeln, die anzeigt wo sie gerade die Linien befinden
  * Linie inklusive Stationen mit Pen malen (wie auf einem U-Bahn-Plan)
  * Daten abrufen, umformen und Positionen anzeigen
  * Zum ausprobieren und entwickeln: Beispieldaten in Woopec.Core ablegen (Rohdaten json)
  * Entwickler kann entscheiden, ob er Rohdaten nimmt oder sich selbst einen Api-Key besorgt
  * Keine Hilfsklassen für Aufbereitung der Rohdaten. Das ist Teil des Lernerfolgs.

* Mehr Fälle als Unit-Tests codieren. Z.B. auch dass der Channel voll läuft. Lasttests.
  Die Lösung dokumentieren. 
  Channels sind hier dokumentiert:
     Mal hiermit versuchen: https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/
     Beispiel: https://www.davidguida.net/how-to-implement-producer-consumer-with-system-threading-channels/
  Der Ansatz mit dem WPF-Dispose kommt von hier:
       https://igorpopov.io/2018/06/16/asynchronous-programming-in-csharp-with-wpf/
* Die Screen* Klassen prüfen, ob das nicht besser Records wären
* Debuggen einfacher machen: Ziel: Die Turtle-Aktion die ich im Debugger durch-steppe, sehe ich auch am Bildschirm (Idee: Im Debug-Modus Channel auf einen Eintrag beschränken, und mit asyn auf das Schreiben warten?)
* ScreenLine.Width
* Code im WPF-Text-Feld editierbar anzeigen.
  * Syntax-Coloring evtl. mit AvalonEdit (https://www.nuget.org/packages/AvalonEdit) oder RoslynPad https://github.com/aelij/RoslynPad

## To decide
* Vielleicht kann man [OpenTK](https://opentk.net/resources.html) als Alterntive zu WPF nutzen
  * Auf OpenTK bin ich durch [Artikel]()https://www.hanselman.com/blog/how-to-install-net-core-on-your-remarkable-2-eink-tablet-with-remarkablenet von Scott Hanselman gestoßen.
  * Animationen mit OpenTK macht wohl eher low level: Bei jedem neuen Frame ändert man die Rotations/Transformations/Usw-Matrizen. Siehe in diesem [Beispiel](http://neokabuto.blogspot.com/2013/07/opentk-tutorial-3-enter-third-dimension.html)
  * OpenTK scheint mir "aktiv" zu sein (aktiver als Yak2D) und wird zumindest aktuell noch im iOS-Teil von Xamarin verwendet, siehe [Microsoft-Doku](https://docs.microsoft.com/de-de/dotnet/api/opentk?view=xamarin-ios-sdk-12)
* Nachdem ich release 1.0.0 veröffentlicht hatte und nach meiner Library gegoogelt habe, bin ich noch auf andere Graphik-Paktete gestoßen:
  * [Yak2D](https://github.com/AlzPatz/yak2d) möchte Grafik-Entwicklung auch einfach machen. 
    * Projekt ist aber irgendwie stehen geblieben, keine commits mehr und die Doku ist auch nur teilweise fertig. Tutorial ab Schritt 2 nur geplant. Bin mir unsicher, ob das noch weiter geht.
  * Über Yak2D bin ich auf [Veldrid](https://github.com/mellinoe/veldrid) gestoßen.
    * Da ist viel mehr los. 
    * Das könnte eine Alternative zu WPF oder MAUI sein.
* Benötigt man überhaupt Screen.RegisterShape? Man kann einfach die Shape-Property der Turtle setzen.
* Screen-Klasse sauberer machen. Vermischt aktuell Methoden für Programmierer und interne Methoden
* Figure und Pen: Methoden revidieren. Wann gibt es Property-Setter? Wann gibt es SetXy-Methoden? Analog zur Turtle-Klasse beides anbieten?
* Vielleicht kann man eine gemeinsame Basisklasse für Pen und Form machen?

## Done

110h 

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
* 24.04.2021: First version of animation-handling-basics is working (3 unit-tests are green)
* 01.05.2021: Animation-Handling works together with WPF
* 04.05.2021: Colors for Pens
* 05.05.2021: Speed for Pens
* 10.05.2021: Penup, Pendown
* 11.05.2021: Animation of a new Pen waits until previous animations are finished
* 12.05.2021: Classes for Shapes
* 13.05.2021: Screen has a dictionary of shapes
* 15.05.2021: Turtle movement and rotation
* 17.05.2021: Cleaner code
* 18.05.2021: Cleaner code in TurtleWpf
* 22.05.2021: FillColor and OutlineColor
* 29.05.2021: One can change shapes of a turtle
* 30.05.2021: Filling
* 05.06.2021: Find TurtleMain by reflection
* 05.06.2021: Prerelease on nuget
* 07.06.2021: Started with documentation (Speed, Move and Draw Methods of Turtle)
* 08.06.2021: Documentation of first release methods is done. All other methods are set to internal.
* 08.06.2021: Shape for bird
* 09.06.2021: Public Code is consistent and documented, all non public code is internal
* 10.06.2021: publish comments with nuget packages
* 12.06.2021: Release 1.0.0 including documentation on woopec.wordpress.com (105 hours of work)
* 28.07.2021: Make github repo visible
* xx.12.2021: .NET 6
* 20.02.2022: Project Template on nuget makes creation of a project easier


 



## Contribution

### Commits

The same as [Angular Commit Message Conventions](https://github.com/angular/angular/blob/master/CONTRIBUTING.md#-commit-message-format)
without `<scope>`:

Each commit message consists of a **header**, a **body**, and a **footer**.


```
<header>
<BLANK LINE>
<body>
<BLANK LINE>
<footer>
```

The `header` is mandatory and must conform to the [Commit Message Header](#commit-header) format.

The `body` is mandatory for all commits except for those of type "docs".
When the body is present it must be at least 20 characters long and must conform to the [Commit Message Body](#commit-body) format.

The `footer` is optional. The [Commit Message Footer](#commit-footer) format describes what the footer is used for and the structure it must have.

Any line of the commit message cannot be longer than 100 characters.


#### <a name="commit-header"></a>Commit Message Header

```
<type>: <short summary>
  │              │
  │              └─⫸ Summary in present tense. Not capitalized. No period at the end.
  │
  └─⫸ Commit Type: build|ci|docs|feat|fix|perf|refactor|test
```

The `<type>` and `<summary>` fields are mandatory.

##### Type

Must be one of the following:

* **build**: Changes that affect the build system or external dependencies (example scopes: gulp, broccoli, npm)
* **ci**: Changes to our CI configuration files and scripts (example scopes: Circle, BrowserStack, SauceLabs)
* **docs**: Documentation only changes
* **feat**: A new feature
* **fix**: A bug fix
* **perf**: A code change that improves performance
* **refactor**: A code change that neither fixes a bug nor adds a feature
* **test**: Adding missing tests or correcting existing tests

##### Summary

Use the summary field to provide a succinct description of the change:

* use the imperative, present tense: "change" not "changed" nor "changes"
* don't capitalize the first letter
* no dot (.) at the end


#### <a name="commit-body"></a>Commit Message Body

Just as in the summary, use the imperative, present tense: "fix" not "fixed" nor "fixes".

Explain the motivation for the change in the commit message body. This commit message should explain _why_ you are making the change.
You can include a comparison of the previous behavior with the new behavior in order to illustrate the impact of the change.


#### <a name="commit-footer"></a>Commit Message Footer

The footer can contain information about breaking changes and is also the place to reference GitHub issues, Jira tickets, and other PRs that this commit closes or is related to.

```
BREAKING CHANGE: <breaking change summary>
<BLANK LINE>
<breaking change description + migration instructions>
<BLANK LINE>
<BLANK LINE>
Fixes #<issue number>
```

Breaking Change section should start with the phrase "BREAKING CHANGE: " followed by a summary of the breaking change, a blank line, and a detailed description of the breaking change that also includes migration instructions.

