using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kardamon.Views;

public partial class NowPlayingPage : UserControl
{
    public NowPlayingPage()
    {
        InitializeComponent();
    }
    
    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        
        if (e.NewSize.Width < 540)
        {
            DesktopLayout.IsVisible = false;
            MobileLayout.IsVisible = true;
        }
        else
        {
            DesktopLayout.IsVisible = true;
            MobileLayout.IsVisible = false;
        }
    }
}