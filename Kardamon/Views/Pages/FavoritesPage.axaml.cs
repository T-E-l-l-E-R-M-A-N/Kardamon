using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kardamon.Views;

public partial class FavoritesPage : UserControl
{
    public FavoritesPage()
    {
        InitializeComponent();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            IsVisible = true;
        else IsVisible = false;
    }
}