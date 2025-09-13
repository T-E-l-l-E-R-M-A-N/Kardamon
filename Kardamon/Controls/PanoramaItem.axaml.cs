using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kardamon.Controls;

public partial class PanoramaItem : UserControl
{
    public static readonly StyledProperty<string> HeaderProperty = AvaloniaProperty.Register<PanoramaItem, string>(
        nameof(Header));

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
    public PanoramaItem()
    {
        InitializeComponent();
    }
}