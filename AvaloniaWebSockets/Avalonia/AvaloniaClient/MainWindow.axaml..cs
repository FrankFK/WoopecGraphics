using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace AvaloniaClient;

public partial class MainWindow : Window
{
    private Point _turtlePos = new(250, 250);
    private double _angle = 0;
    private readonly Canvas _canvas;
    private readonly ClientWebSocket _socket = new();

    public MainWindow()
    {
        InitializeComponent();
        _canvas = this.FindControl<Canvas>("DrawCanvas");
        ConnectWebSocket();
    }

    private async void ConnectWebSocket()
    {
        await _socket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
        _ = Task.Run(ReceiveLoop);
    }

    private async Task ReceiveLoop()
    {
        var buffer = new byte[1024 * 4];
        while (_socket.State == WebSocketState.Open)
        {
            var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var cmd = JsonSerializer.Deserialize<CommandMessage>(message);

            if (cmd?.Type == "command" && cmd.Action == "forward")
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    var dx = Math.Cos(_angle * Math.PI / 180) * cmd.Value;
                    var dy = Math.Sin(_angle * Math.PI / 180) * cmd.Value;
                    var newPos = new Point(_turtlePos.X + dx, _turtlePos.Y + dy);

                    var line = new Line
                    {
                        StartPoint = _turtlePos,
                        EndPoint = newPos,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    };

                    _canvas.Children.Add(line);
                    _turtlePos = newPos;
                });
            }
        }
    }

    private async void Window_KeyDown(object? sender, KeyEventArgs e)
    {
        var msg = JsonSerializer.Serialize(new { type = "input", key = e.Key.ToString() });
        var bytes = Encoding.UTF8.GetBytes(msg);
        await _socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}

public record CommandMessage(string Type, string Action, double Value);
