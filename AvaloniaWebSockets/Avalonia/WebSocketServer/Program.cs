using System.Net;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWebSockets();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var socket = await context.WebSockets.AcceptWebSocketAsync();
        Console.WriteLine("🔌 Client verbunden");

        var buffer = new byte[1024 * 4];
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

            Console.WriteLine($"⬅️ Empfangen: {message}");

            // Beispiel: Bei Pfeiltaste ein Forward-Befehl senden
            if (message.Contains("ArrowUp"))
            {
                var command = JsonSerializer.Serialize(new { type = "command", action = "forward", value = 50 });
                var data = Encoding.UTF8.GetBytes(command);
                await socket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, CancellationToken.None);
                Console.WriteLine("➡️ Gesendet: Forward 50");
            }
        }
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run("http://localhost:5000");
