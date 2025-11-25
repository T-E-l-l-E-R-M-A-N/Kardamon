using System.Timers;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;

namespace Kardamon.Behaviors;

public class VisualBrushRedrawBehavior : Behavior<Control>
{
    private Timer _timer;

    protected override void OnAttached()
    {
        base.OnAttached();
        _timer = new Timer(100);
        _timer.Elapsed += TimerOnElapsed;
        _timer.Start();
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        var border = AssociatedObject as Border;
        if (border != null)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var visualBr = border.Background as VisualBrush;
                var visual = visualBr.Visual;
                border.Background = new VisualBrush(visual)
                {
                    Stretch = visualBr.Stretch, 
                    AlignmentX = visualBr.AlignmentX, 
                    AlignmentY = visualBr.AlignmentY, 
                    Transform = visualBr.Transform, 
                    TransformOrigin = visualBr.TransformOrigin, 
                    Opacity = visualBr.Opacity, 
                    DestinationRect = visualBr.DestinationRect, 
                    SourceRect = visualBr.SourceRect, 
                    TileMode = visualBr.TileMode
                };
            });
        }
    }
}