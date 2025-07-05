# 🚀 Woopec WebSocket Starter

Ein Proof-of-Concept für eine plattformübergreifende Turtle-Grafik mit Avalonia – gesteuert per WebSocket.

## 🔧 Komponenten

- **AvaloniaClient**: Empfängt Zeichenbefehle (z. B. `forward 100`) und zeichnet Linien
- **WebSocketServer**: Simpler C#-Server (ASP.NET Core), der Befehle sendet und Tastatureingaben entgegennimmt
- **protocol.md**: JSON-Protokollbeschreibung zur Kommunikation

## ▶️ Schnellstart

### Voraussetzungen

- [.NET 8 SDK](https://dotnet.microsoft.com)
- Avalonia 11+

### Ausführen

```bash
# Terminal 1
dotnet run --project WebSocketServer

# Terminal 2
dotnet run --project AvaloniaClient