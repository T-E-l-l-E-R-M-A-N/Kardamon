using System.Timers;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Input.GestureRecognizers;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using Kardamon.ViewModels;

namespace Kardamon.Behaviors;

public class SliderDragBehavior : Behavior<Slider>
{
    public static readonly StyledProperty<ICommand> CommandProperty = AvaloniaProperty.Register<SliderDragBehavior, ICommand>(
        nameof(Command));

    public static readonly StyledProperty<ICommand> DragStartCommandProperty = AvaloniaProperty.Register<SliderDragBehavior, ICommand>(
        nameof(DragStartCommand));

    public ICommand DragStartCommand
    {
        get => GetValue(DragStartCommandProperty);
        set => SetValue(DragStartCommandProperty, value);
    }

    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    
    protected override void OnAttached()
    {
        base.OnAttached();
        
        AssociatedObject.TemplateApplied += AssociatedObjectOnTemplateApplied;
    }

    private void AssociatedObjectOnTemplateApplied(object? sender, TemplateAppliedEventArgs e)
    {
        var thumb = e.NameScope.Find<Thumb>("thumb");
        thumb.DragStarted += ThumbOnDragStarted;
        thumb.DragCompleted += ThumbOnDragCompleted;
        thumb.DragDelta += ThumbOnDragDelta;
        thumb.Tapped += ThumbOnTapped;
    }

    private void ThumbOnDragDelta(object? sender, VectorEventArgs e)
    {
        try
        {
            Command?.Execute(Convert.ToInt64(AssociatedObject.Value));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }

    private void ThumbOnTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            Command?.Execute(Convert.ToInt64(AssociatedObject.Value));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }

    private void ThumbOnDragStarted(object? sender, VectorEventArgs e)
    {
        try
        {
            Command?.Execute(Convert.ToInt64(AssociatedObject.Value));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }

    private void ThumbOnDragCompleted(object? sender, VectorEventArgs e)
    {
        try
        {
            Command?.Execute(Convert.ToInt64(AssociatedObject.Value));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}

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
                border.Background = new VisualBrush(visual) {Stretch = Stretch.Fill};
            });
        }
    }
}