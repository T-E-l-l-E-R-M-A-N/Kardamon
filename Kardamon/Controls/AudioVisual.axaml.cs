using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Kardamon.Controls;

public partial class AudioVisual : UserControl
{
    public static readonly StyledProperty<IBrush> BarBrushProperty =
        AvaloniaProperty.Register<AudioVisual, IBrush>(
            nameof(BarBrush),
            new SolidColorBrush(Color.Parse("#4CAF50")));

    public IBrush BarBrush
    {
        get => GetValue(BarBrushProperty);
        set => SetValue(BarBrushProperty, value);
    }
    
    public AudioVisual()
    {
        InitializeComponent();
    }
}