using Avalonia;

namespace Kardamon.Helpers;

public class RuntimeHelpers : AvaloniaObject
{
    public static readonly AttachedProperty<LayoutType> LayoutTypeProperty =
        AvaloniaProperty.RegisterAttached<RuntimeHelpers, AvaloniaObject, LayoutType>("LayoutType");

    public static void SetLayoutType(AvaloniaObject obj, LayoutType value) => obj.SetValue(LayoutTypeProperty, value);
    public static LayoutType GetLayoutType(AvaloniaObject obj) => obj.GetValue(LayoutTypeProperty);
}