using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kardamon.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property.Name == nameof(WindowState))
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (WindowState == WindowState.FullScreen)
                {
                    MaxWidth = Double.PositiveInfinity;
                }
                else
                {
                    MaxWidth = 1100;
                }
            }
        }
    }
}