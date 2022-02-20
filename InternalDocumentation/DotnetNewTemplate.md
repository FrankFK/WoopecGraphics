# Template für dotnet new

## Ziel

Der Anwender soll möglichst einfach ein erstes Projekt erstellen können. Am einfachsten so:

* Ordner anlegen
* Kommandozeile öffnen und in den Ordner navigieren
* Aufrufen `dotnet new woopec-wpf` 
* Alles ist da

## So geht's

### Dotnet Template erstellen

Ich muss ein dotnet Template erstellen. Infos dazu siehe in [Create a project template for dotnet new - .NET | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-project-template). Das habe ich hier wie folgt umgesetzt.

In dieser Solution gibt es folgende Unterverzeichnis-Struktur

```
TemplatePack
-- Templates
   -- TemplateWpf
      -- .template.config
         template.json
```

TemplateWpf ist das Template für ein WPF-Projekt mit einem einfachen Woopec-Startprogramm. Lokal lässt sich das so ausprobieren

```sh
cd TemplatePack\Templates\TemplatesWpf
dotnet new --uninstall .\
dotnet new --install .\
```

Danach irgendwo testweise ein neues Verzeichnis anlegen und dort das Template benutzen:

```sh
cd Irgendwohin
mkdir TurtleProject1
cd TurtleProject1
dotnet new woopec-wpf
dotnet run
```



### Alle Templates in einem TemplatePack zusammenfassen

Aktuell habe ich nur ein einziges Template, aber für Nuget erstellt man Template Package. So ein TemplatePack kann mehrere Templates enthalten. Die Konfiguration eines TemplatePacks ist beschrieben in [Create a template package for dotnet new - .NET | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-template-package). Letztendlich stehen alle Informationen zum Template Package in der `TemplatePack.csproj`, die im Ordner `TemplatePack` steht. 

Die aktuellen Einstellungen in der `TemplatePack.csproj` sind so, dass das Tag `<PackageVersion>` bestimmt, wie die Version für Nuget heißt. 

Die Nuget-Package-Datei (`.nupkg`) kann man über Kommando erstellen:

```sh
cd TemplatePack
dotnet pack
```

Das Kommando gibt an, wo die `.npkg` Datei erstellt wurde. Über dieses Kommando kann man basierend auf dieser Datei das Template Package installieren:

```sh
dotnet new --install DerPfad.nupkg
```

### Das Nuget-Package auf nuget hochladen

Das geht so

```sh
dotnet nuget push DerPfad.nupkg --api-key meinapikey --source https://api.nuget.org/v3/index.json

```



### Prüfen, ob das Package auf nuget angekommen ist

Ich kann die Seite [NuGet Gallery | Home](https://www.nuget.org/) aufrufen und mich dort mit meinem Windows-Account anmelden. Über rechts-oben->Woopec->Manage Packages müsste das Package dann zu sehen sein.

Dann lokal erst mal zur Sicherheit dafür sorgen, dass das Package nicht auf meinem Rechner ist. Über dieses Kommando kann ich sehen, welche Templates installiert sind und wie ich sie deinstallieren kann.

```sh
dotnet new --uninstall
```

Die Installation des Nuget-Template-Packages funktioniert so:

```sh
dotnet new --install Woopec.Templates
```







Wie ich das Package dann auf Nuget hochlade, habe ich in OneNote in Privat beschrieben. Ich sollte mir dafür wohl noch ein Powershell-Kommando erstellen, damit ich das einfacher ausführen kann.