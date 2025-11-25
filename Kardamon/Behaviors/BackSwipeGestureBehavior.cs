using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Kardamon.Behaviors;

public class BackSwipeGestureBehavior : Behavior<Control>
{
    private bool _isPointerDown = false;
    private Point _startPoint;

    public static readonly StyledProperty<ICommand?> BackCommandProperty =
        AvaloniaProperty.Register<BackSwipeGestureBehavior, ICommand?>(nameof(BackCommand));

    public ICommand? BackCommand
    {
        get => GetValue(BackCommandProperty);
        set => SetValue(BackCommandProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.AddHandler(InputElement.PointerPressedEvent, OnPressed, RoutingStrategies.Tunnel);
        AssociatedObject.AddHandler(InputElement.PointerMovedEvent, OnMoved, RoutingStrategies.Tunnel);
        AssociatedObject.AddHandler(InputElement.PointerReleasedEvent, OnReleased, RoutingStrategies.Tunnel);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.RemoveHandler(InputElement.PointerPressedEvent, OnPressed);
        AssociatedObject.RemoveHandler(InputElement.PointerMovedEvent, OnMoved);
        AssociatedObject.RemoveHandler(InputElement.PointerReleasedEvent, OnReleased);
    }

    private void OnPressed(object? s, PointerPressedEventArgs e)
    {
        var pos = e.GetPosition((Control)s!);

        // Начало жеста только если палец касается левого края экрана
        if (pos.X <= 25)
        {
            _isPointerDown = true;
            _startPoint = pos;
            e.Pointer.Capture((Control)s!);
        }
    }

    private void OnMoved(object? s, PointerEventArgs e)
    {
        if (!_isPointerDown)
            return;

        var pos = e.GetPosition((Control)s!);
        var dx = pos.X - _startPoint.X;

        // Движение вправо > 60px — жест назад
        if (dx > 60)
        {
            _isPointerDown = false;
            e.Pointer.Capture(null);

            if (BackCommand?.CanExecute(null) == true)
                BackCommand.Execute(null);
        }
    }

    private void OnReleased(object? s, PointerReleasedEventArgs e)
    {
        _isPointerDown = false;
        e.Pointer.Capture(null);
    }
}