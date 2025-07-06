# Architecture

Solange das hier sowieso niemanden außer mir interessiert, kann ich hier auch alles auf Deutsch schreiben.

## 1 Introduction & Goals

C# ist eine großartige Programmiersprache. Eigentlich auch gut zu lernen und mit wenig Einstiegshürden. Allerdings nur solange man sich auf Consolen-Programme beschränkt. Aber wer will das schon, wenn sie/er eine Programmiersprache lernt. In anderen Sprachen ist das viel besser. In python kann man beispielsweise Turtle-Graphics nutzen und auch als Anfänger ganz leicht Programme mit Grafik programmieren. Das will ich auch für C# erreichen.

Diese Library soll absoluten Programmier-Anfänger ermöglichen, mit C# erste grafische Programme zu schreiben.

Daraus ergeben sich diese Haupt-Qualitätsziele:

* Die Befehle der Library müssen sehr leicht zu verstehen und zu benutzen sein.
* Debugging ist eine super Methode, um den geschriebenen Code nachzuvollziehen. Darum muss Debugging von Programmen, die man mit dieser Library geschrieben hat, leicht sein.
* Die Library soll auch noch für etwas fortschrittlichere Aufgaben nutzbar sein. Darum muss die Performance gut sein. Und es soll möglich sein, zeitlich parallel verschiedene Dinge auf dem Bildschirm zu animieren.

## 2 Constraints

Alles soll kostenlos nutzbar sein. Wichtig ist auch, dass ein Programmiereinsteiger auf seinem Rechner alle notwendigen Dinge schnell an den Start bringen kann. Logischerweise ist C# als Programmiersprache gesetzt. 

Eigentlich wäre es schön, wenn es geringe Systemvoraussetzungen für den Rechner des Anwenders gäbe. Man könnte beispielsweise fordern, dass ein funktionierender Browser ausreicht. Das war mir zu kompliziert und zu einschränkend. Darum gehe ich davon aus, dass der Anwender einen Rechner hat, auf dem er eine C# Entwicklungsumgebung einrichten kann. Schön wäre es, wenn das nicht zwingend ein Windows-Rechner sein müsste. Aktuell funktioniert es (wegen der Nutzung von WPF) aber nur auf Windows-Rechnern.

## 3 Context & Scope

Der User programmiert ein C# Programm. In diesem C# Programm nutzt sie die Objekte der Woopec Library. Das Programm wird auf dem Bildschirm ausgeführt. Währenddessen kann der User sich das Programm ansehen und es auch gleichzeitig debuggen.

![image-20241109152507812](Context (Klein).png)



## 4 Solution Strategy

Die wesentlichen Lösungsideen:

* Die Befehle und Objekte orientieren sich stark an [Python Turtle Graphics](https://docs.python.org/3/library/turtle.html#module-turtle). Das ist für Anfänger gut geeignet.

* Es soll auch möglich sein, mehrere Objekte (Turtles) auf dem Bildschirm gleichzeitig zu animieren. Das wollte ich nicht selbst implementieren und habe darum WPF benutzt. WPF funktioniert nur unter Windows. Damit man das später mal auf eine andere Technologie (vermutlich AvaloniaUI) umstellen kann, werden Woopec-Kern und Woopec-WPF gut entkoppelt.

* Einfaches Debuggen ist tricky:
  Der Programmieranfänger möchte eigentlich so etwas programmieren:

  ```csharp
  Turtle.Forward(50);
  Turtle.Left(90);
  Turtle.Forward(50);
  ```

  Dann möchte er mit dem Debugger durch diese Kommandos steppen und auf dem Bildschirm sehen, dass sich seine Turtle wie angegeben bewegt. 
  Der einfachste Implementierungsansatz wäre: Über ein UI-Event wird der obige Code aufgerufen, die Turtle-Befehle werden in UI-Befehle umgewandelt, und dann geht die Kontrolle wieder an das UI. Dieser Ansatz führt aber dazu, dass beim Debuggen obiger Zeilen auf dem UI *nichts passiert*, denn das UI hat noch nicht die Kontrolle zurück.
  Darum ist das hier anders gelöst: Beim Debuggen werden zwei Prozesse gestartet: Der eine Prozess durchläuft die Turtle Kommandos und übermittelt sie über einen Channel an den zweiten Prozess. Der zweite Prozess ist der UI-Prozess. Dieser nimmt die UI-Kommandos auf und zeigt sie an.

## 5 Building Block View

### 5.1 Gesamtsystem

Auf der obersten Ebene besteht das Programm des Benutzers aus dem individuellen Code des Benutzers und den beiden Libraries Woopec.WPF und Woopec.Core:

![](Level1 (Klein).png)

Erläuterungen:

| Block           | Erläuterung                                                  |
| --------------- | ------------------------------------------------------------ |
| Woopec.Graphics | Diese Library enthält die Klassen (z.B. `Turtle`) und Methoden (z.B. `Left`), die der User in seinem Code verwendet. Die Library erzeugt dementsprechend Änderungsbefehle für das UI, die über einen Channel an Woopec.WPF weitergegeben werden. Informationen vom UI (z.B. im UI eingegebene Texte) werden aus einem zweiten Rückkanal gelesen |
| Woopec.WPF      | Diese Library liest die Änderungsbefehle aus dem Channel und zeigt die Änderungen in einem WPF UserControl `WoopecCanvas` an. Im UI erfasste Informationen (z.B. im UI eingegebene Texte) werden in einen Rückkanal geschrieben. |
| C# code of user | Der C# code des Users nutzt in seinem Main Window ein UI-Control vom Typ `WoopecCanvas`, in dem die Woopec-Objekte angezeigt werden. Der Code des Benutzers muss eine Methode mit Namen `WoopecMain` enthalten. In dieser Methode (und von ihr aufgerufenen Methoden) benutzt der Anwender die Klassen (z.B. `Turtle`) und Methoden (z.B. `Left`) von Woopec.Core, um die gewünschten Grafiken zu zeichnen. |
| Channels        | Channels zum Austausch der Daten zwischen Woopec.Graphics Library und dem Frontend |

### 5.2 Level 2

**!!! Schaubild fehlt noch !!!**



Erläuterungen der Namespaces:

| Block                          | Erläuterung                                                  |
| ------------------------------ | ------------------------------------------------------------ |
| Woopec.Graphics                | Dieser Namespace enthält die Klassen, die von Usern verwendet werden können (Turtle, Figure, Pen, etc.). Diese Klassen sind in der öffentlichen [Woopec Dokumentation](https://frank.woopec.net/woopec-docs-index.html) beschrieben |
| Woopec.Graphics.Helper         | Einfache interne Hilfs-Objekte, die von Woopec.Graphics benutzt werden, kein Teil der des LowLevelScreens |
| Woopec.Graphics.LowLevelScreen | Woopec.Graphics tauscht Daten mit dem ILowLevelScreen. Dazu werden (teilweise asynchrone) Methode von ILowLevelScreen aufgerufen. Die Parameter enthalten die auszutauschenden Daten. Die Implementierung des LowLevelScreens ist in Woopec.Graphics.Internal realisiert. |
| Woopec.Graphics.Internal       | Wie in Abschnitt 5.1 erläutert, tauscht Woopec.Graphics über Channels Informationen mit dem UI aus. Der technische interne Unterbau für Woopec.Graphics.LowLevelScreen. Der wird weiter unten auf Level 3 genauer beschrieben. |
| Woopec.Graphics.Factories      | Factories um die passende Implementierung für ein Interface zu generieren. Aktuell wird in der Solution keine Dependency Injection benutzt. Das könnte man vielleicht mal umstellen. |



## 6 Runtime View

## 7 Deployment View

## 8 Crosscutting Concepts

## 9 Architectural Decisions

## 10 Quality Requirements

## 11 Risks & Technical Debt

## 12 Glossary





