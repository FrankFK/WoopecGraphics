# Interne Dokumentation

Startseite der internen Dokumentation. 

Zurzeit ist hier alles noch etwas unaufgeräumt.

Zum allgemeinen Verständnis: Das [Architektur-Dokument](Architectue.md) lesen

Aktuell arbeite ich an einer Avalonia-Version: Stand der Dinge steht im [AvaloniaMisc-Dokument](AvaloniaMisc.md) 

Generelle Infos, die ich gerne mal vergesse:

* Testen kann man Avalonia über das Projekt `UsingAvaloniaProject`.
* In den Debugger-Settings dieses Projekts ist aktuell das Programm-Argument `--one_process` gesetzt. Dies sorgt dafür, dass immer alles in einem Prozess läuft, auch wenn ein Debugger attached ist. Das ist für die Programmier-Anfänger gerade nicht das gewünschte Verhalten, aber zum Debuggen des Avalonia-Codes ist das viel einfacher.