# Nuget

## Veröffentlichen auf Nuget

Aktuell noch etwas händisch:

* In TurtleCore und TurtleWpf in den Projekt-Properties in Pack eine neue Version eintragen.
* Dann auf TurtleCore und TurtleWpf jeweils Pack aufrufen. Das erzeugt die nuget Packages
* Dann das Script nugetpublish.ps1 aus dem Root Ordner aufrufen. Das Script nutzt den ApiKey, der lokal auf meinem Rechner gespeichert ist.