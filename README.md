# simple-graphics-for-csharp-beginners
Simple Graphics for C# Beginners (starting with Turtle-Graphics)


## Planned

* Erste Version nutzbar machen (MVP)
  * Auf anderem Rechner kann es installiert und genutzt werden (ohne Doku)
    * Wpf-Paket auf Nuget packen
    * Siehe https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package-using-visual-studio?tabs=netcore-cli
    * Ich brauche einen nuget Account (hab' ich den schon)
    * Name für das Paket Woopec.Wpf ? Woopec.Core auch schon mal draufpacken, damit ich das später mal trennen kann und das auch nutzbar ist.
    * Paktet unlisten 
    * Nutzung von Nuget woopec.wpf ausprobieren
  * Turtle und Bird Shapes von Clemens einbauen. Bird shape in einer der Demos nutzen.
  * Namespaces, Klassen und Methoden: Nur das public, was auch public sein soll. Namen passend und konsistent machen.
  * Öffentliche Methoden im Code kommentieren und Code-Kommentare in nuget-Package mitveröffentlichen
  * Hilfe-Seite mit Überlick über alle öffentlichen Methoden verfassen
  * Wordpress-Seite anlegen, auf der die Hilfe-Seite steht.
  * Wordpress-Seite anlegen, auf der beschrieben ist, wie man ein Turtle-Projekt auf dem eigenen Rechner anlegt.
  * Mail-Account anlegen und auf Wordpress-Seite verlinken
  * Wordpress-Seite als URL im Nuget-Package verlinken
  * Nuget-Package öffentlich machen
  * Code auf github kann erst mal noch privat bleiben.

* Exceptions müssen abgefangen werden -- aktuell stürzt dann das Programm ab.
  * https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?redirectedfrom=MSDN&view=net-5.0*

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
* Benötigt man überhaupt Screen.RegisterShape? Man kann einfach die Shape-Property der Turtle setzen.
* Screen-Klasse sauberer machen. Vermischt aktuell Methoden für Programmierer und interne Methoden
* Figure und Pen: Methoden revidieren. Wann gibt es Property-Setter? Wann gibt es SetXy-Methoden? Analog zur Turtle-Klasse beides anbieten?
* Vielleicht kann man eine gemeinsame Basisklasse für Pen und Form machen?

## Done

78h 

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

