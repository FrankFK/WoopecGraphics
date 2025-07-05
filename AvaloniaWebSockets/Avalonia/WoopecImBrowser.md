# 🧩 Architektur: Woopec mit Avalonia + bidirektionalen WebSockets

Mit MS Copilot am 29.6.25 als Idee entwickelt.

## 🖼️ Frontend: Avalonia (Cross-Plattform-GUI)

- Nutzt Avalonia zur Darstellung der **Zeichenfläche (Canvas o. ä.)**
- Zeichnet Turtle-Grafik basierend auf empfangenen Befehlen
- Unterstützt Tastatur-, Maus- oder Touch-Eingaben

## 🔌 Kommunikation: WebSocket-Verbindung

- **WebSocket-Client** in der Avalonia-App verbindet sich mit einem Server (z. B. C# ASP.NET Core oder Node.js)
- **Bidirektional**:
  - Vom **Server zur App**: Zeichenbefehle, Parameter, Steuerinfos
  - Von der **App zum Server**: Tastatureingaben, Maus-Events, Statusupdates (z. B. Zeichenposition)

## 🔄 Typische Nachrichtentypen (JSON)

### 👉 Vom Server zum Client:

```json
{ "type": "command", "action": "forward", "value": 100 }
