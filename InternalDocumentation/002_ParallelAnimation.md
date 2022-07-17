# ADR 002 Parallele Animation
## Status
Akzeptiert, 21.03.2021

## Kontext
Turtle Grafik mit einer Turtle sind gut. Aber spannender wäre es, wenn man mehrere Turtles erzeugen könnte und wenn sich diese *gleichzeitig* bewegen würden.

## Entscheidung
Das Grundkonzept ist eine *gleichzeitige* Animation. Beispiel:

```c#
var turtle1 = new Turtle();
var turtle2 = new Turtle();

turtle1.Backward(100);
turtle2.Forward(100);
```

Als Ergebnis bewegen sich die beiden Turtles gleichzeitig. Die eine nach links, die andere nach rechts. Die Animation wird nicht durch kleine Steps approximiert, sondern durch die Animations in WPF realisiert.

## Konsequenzen
### Konzept

Auch wenn das Grundkonzept der gleichzeitigen Animation coole Effekte ermöglicht, gibt es Fälle, in denen eine sequentielle Ausführung erwünscht ist:

* *Sequentielle Ausführung innerhalb eines Objekts*
  Bei Ausführung der Befehle `turtle1.Forward(50); turtle1.Left(90); turtle.Forward(50);` ist die Erwartung, dass drei Bewegungen hintereinander ausgeführt werden. Zu beachten: Nicht alle Bewegungen innerhalb eines Objekts sollen sequentiell ausgeführt werden: Der `Turtle.Forward(50)` Befehl besteht beispielsweise aus zwei parallel stattfindenden Animationen: Der Bewegung des Pens und der Bewegung der Figure.

* *Sequentielle Ausführung eines neu angelegten Objekts*
  Bei Ausführung der Befehle `turtle1.Forward(50); var turtle2 = new Turtle(); turtle2.Forward(50);` ist die Erwartung, dass die Bewegung von turtle2 erst dann beginnt, wenn alle vor der Konstruktion von turtle2 generierten Bewegungen anderes Turtles beendet sind.

* *Sequentielle Ausführung in speziell gewünschten Fällen*

  Es gibt Situationen, in denen explizit gewünscht ist, dass Bewegungen verschiedener Turtles hintereinander ausgeführt werden.

### Lösung

Die Lösung hierfür ist kompliziert. **Hic sunt dracones!**

Ein paar Objekte aus dem Lösungsraum:

* Pen und Figure einer Turtle bilden zusammen eine *Group*. Beide haben dieselbe *GroupId*
* Alle Bildschirm-relevanten Operationen von Pens und Figures werden als *ScreenObjects* verschickt.
* Diese ScreenObjects werden von einem *ScreenObjectProducer* erzeugt. Und zwar direkt bei Ausführung der Pen- oder  Figure-Kommandos, also ohne Warten auf die Beendigung einer Animation.
* Die ScreenObjects werden von einem *ScreenObjectConsumer* empfangen. 
  * Der ScreenObjectConsumer hat als Haupteinstiegspunkt die Methode *HandleNextScreenObjectAsync*.  Diese Methode liest noch unbearbeitete ScreenObject und verarbeitet diese.  Hier gibt es zwei Fälle:
    * Entweder kann das aktuelle ScreenObject sofort ausgegeben werden. Dann wird es an den ScreenObjectWriter (siehe unten) gegeben. Danach terminiert die HandleNextScreenObjectAsync-Methode, ihr Thread wird beendet (das ist für WPF wichtig), und sie wird sofort in einem neuen Task wieder aufgerufen.
    * Oder das aktuelle ScreenObject muss auf die Ausführung anderer ScreenObjects warten. In diesem Fall wird das aktuelle ScreenObject in einem AnimationGroupState (siehe unten) aufbewahrt, bis es ausgegeben werden kann. Die Methode HandleNextScreenObjectAsync macht mit dem nächsten unbearbeiteten ScreenObject weiter.
* Direkt ausgebbare ScreenObjects werden an einen *ScreenObjectWriter* ausgegeben. 
  * Objekte ohne Animation gibt der Writer einfach aus.
  * Bei Objekten mit Animation startet der Writer die Animation dieser Objekte. Bei Beendigung einer Animation ruft der Writer (über das Event *OnAnimationIsFinished*) die Methode *AnimationOfGroupIsFinished* des Consumers auf. Der Consumer prüft die aktuellen AnimationGroupStates (siehe unten) und gibt Objekte, die jetzt starten dürfen, an den Writer weiter.
* Für die Verwaltung der wartenden ScreenObjects gibt es pro GroupId eine separate First-In-First-Out Liste, den *AnimationGroupState* 

Die ScreenObjectConsumer-Klasse und die ScreenObjectProducer-Klasse geben relativ viele Debug-Meldungen aus, damit man Probleme leichter nachvollziehen kann.

