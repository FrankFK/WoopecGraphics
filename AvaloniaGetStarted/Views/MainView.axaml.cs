using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;

namespace AvaloniaGetStarted.Views;

public class MyEasing : Easing
{
    private readonly string _name;
    private bool _ended;
    public MyEasing(string name)
    {
        _name = name;
        _ended = false;
    }
    public override double Ease(double progress)
    {
        if (progress >= 1.0 && !_ended)
        {
            Debug.WriteLine($"{_name}: Animation ended.");
            _ended = true; // Ease may be called multiple times at the end
        }

        return progress;
    }
}

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    public void ButtonClicked(object source, RoutedEventArgs args)
    {
        AddLine(MainCanvas);
        AddTransformedRectangle(MainCanvas);
        AddRotatedRectangle(MainCanvas);
    }

    public void AddLine(Canvas canvas)
    {
        /*
            <Line StartPoint="0,0" EndPoint="30,115" Stroke="Red" StrokeThickness="1">
                <Line.Styles>
                    <Style Selector="Line">
                        <Style.Animations>
                            <Animation Duration="0:0:6" FillMode="Forward">
                                <KeyFrame Cue="0%">
                                    <Setter Property="EndPoint" Value="0,0"></Setter>
                                </KeyFrame>
                                <KeyFrame Cue="100%">
                                    <Setter Property="EndPoint" Value="30,115"></Setter>
                                </KeyFrame>
                            </Animation>
                        </Style.Animations>
                    </Style>
                </Line.Styles>
            </Line>
         */
        var line = new Line();
        line.StartPoint = new Avalonia.Point(0, 0);
        line.EndPoint = new Avalonia.Point(30, 115);
        line.Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        line.StrokeThickness = 1;

        var animation = new Animation();
        animation.Duration = TimeSpan.FromSeconds(6);
        animation.FillMode = FillMode.Forward;

        var keyFrame1 = new KeyFrame();
        var setter1 = new Setter();
        setter1.Property = Line.EndPointProperty;
        setter1.Value = line.StartPoint;
        keyFrame1.Setters.Add(setter1);

        var keyFrame2 = new KeyFrame();
        keyFrame2.Cue = new Cue(1.0);
        var setter2 = new Setter();
        setter2.Property = Line.EndPointProperty;
        setter2.Value = line.EndPoint;
        keyFrame2.Setters.Add(setter2);

        animation.Children.Add(keyFrame1);
        animation.Children.Add(keyFrame2);

        animation.Easing = new MyEasing("line1");

        var style = new Style(x => x.OfType<Line>());   // Idea from  repos\Avalonia\tests\Avalonia.Base.UnitTests\Styling\StyleTests.cs
        style.Animations.Add(animation);
        line.Styles.Add(style);

        canvas.Children.Add(line);
    }

    public void AddTransformedRectangle(Canvas canvas)
    {
        /*
            <Rectangle Fill="Blue" Width="63" Height="41" Canvas.Left="40" Canvas.Top="100">
                <Rectangle.Styles>
                    <Style Selector="Rectangle">
                        <Style.Animations>
                            <Animation Duration="0:0:3" FillMode="Forward">
                                <KeyFrame Cue="0%">
                                    <Setter Property="TranslateTransform.X" Value="0"/>
                                    <Setter Property="TranslateTransform.Y" Value="0"/>
                                </KeyFrame>
                                <KeyFrame Cue="100%">
                                    <Setter Property="TranslateTransform.X" Value="40"/>
                                    <Setter Property="TranslateTransform.Y" Value="100"/>
                                </KeyFrame>
                            </Animation>
                        </Style.Animations>
                    </Style>
                </Rectangle.Styles>
            </Rectangle>
         */

        var keyframe1 = new KeyFrame()
        {
            Setters = { new Setter(TranslateTransform.XProperty, 0d), new Setter(TranslateTransform.YProperty, 0d) },
            Cue = new Cue(0.0),
        };

        var keyframe2 = new KeyFrame()
        {
            Setters = { new Setter(TranslateTransform.XProperty, 40d), new Setter(TranslateTransform.YProperty, 100d) },
            Cue = new Cue(1.0)
        };

        var animation = new Animation()
        {
            Duration = TimeSpan.FromSeconds(3),
            FillMode = FillMode.Forward,
            Children = { keyframe1, keyframe2 },
            Easing = new MyEasing("rectangle1"),
        };

        var style = new Style()
        {
            Animations = { animation },
        };

        var rectangle = new Rectangle()
        {
            Width = 63,
            Height = 41,
            Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0)),
            Styles = { style },
        };

        canvas.Children.Add(rectangle);
        Canvas.SetLeft(rectangle, 10);
        Canvas.SetTop(rectangle, 20);
    }

    public void AddRotatedRectangle(Canvas canvas)
    {
        /*
            <Rectangle Fill="Green" Width="63" Height="41" Canvas.Left="80" Canvas.Top="200">
                <Rectangle.Styles>
                    <Style Selector="Rectangle">
                        <Style.Animations>
                            <Animation Duration="0:0:3" Delay="0:0:3" FillMode="Forward">
                                <KeyFrame Cue="0%">
                                    <Setter Property="RotateTransform.Angle" Value="0"/>
                                </KeyFrame>
                                <KeyFrame Cue="100%">
                                    <Setter Property="RotateTransform.Angle" Value="90"/>
                                </KeyFrame>
                            </Animation>
                        </Style.Animations>
                    </Style>
                </Rectangle.Styles>
            </Rectangle>
         */
        var keyframe1 = new KeyFrame()
        {
            Setters = { new Setter(RotateTransform.AngleProperty, 0d) },
            Cue = new Cue(0.0),
        };

        var keyframe2 = new KeyFrame()
        {
            Setters = { new Setter(RotateTransform.AngleProperty, 90d) },
            Cue = new Cue(1.0)
        };

        var animation = new Animation()
        {
            Duration = TimeSpan.FromSeconds(3),
            Delay = TimeSpan.FromSeconds(3),
            FillMode = FillMode.Forward,
            Children = { keyframe1, keyframe2 },
            Easing = new MyEasing("rectangle2"),
        };

        var style = new Style()
        {
            Animations = { animation },
        };

        var rectangle = new Rectangle()
        {
            Width = 63,
            Height = 41,
            Fill = new SolidColorBrush(Color.FromRgb(0, 0, 255)),
            Styles = { style },
        };

        canvas.Children.Add(rectangle);
        Canvas.SetLeft(rectangle, 80);
        Canvas.SetTop(rectangle, 200);
    }



}
