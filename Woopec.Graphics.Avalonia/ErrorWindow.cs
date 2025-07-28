namespace Woopec.Graphics.Avalonia
{
    // Eine einfache Window-Klasse für die Fehlermeldung
    public class ErrorWindow : Window
    {
        public ErrorWindow(string errorMessage)
        {
            Title = "Fehler";
            Width = 400;
            Height = 150;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var iconText = new TextBlock
            {
                Text = "❌", // Unicode-Symbol für „Fehler“
                FontSize = 32,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10)
            };

            var messageBlock = new TextBlock
            {
                Text = errorMessage,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10)
            };

            var contentPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10)
            };
            contentPanel.Children.Add(iconText);
            contentPanel.Children.Add(messageBlock);

            var okButton = new Button
            {
                Content = "OK",
                Width = 80,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            okButton.Click += (_, _) => Close();

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 10, 10)
            };
            buttonPanel.Children.Add(okButton);

            var rootPanel = new DockPanel();
            DockPanel.SetDock(contentPanel, Dock.Top);
            DockPanel.SetDock(buttonPanel, Dock.Bottom);
            rootPanel.Children.Add(contentPanel);
            rootPanel.Children.Add(buttonPanel);

            Content = rootPanel;
        }
    }
}
