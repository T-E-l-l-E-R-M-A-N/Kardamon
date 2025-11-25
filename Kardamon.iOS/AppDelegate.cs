using System;
using System.Net;
using Autofac;
using Foundation;
using UIKit;
using Avalonia;
using Avalonia.Controls;
using Avalonia.iOS;
using Avalonia.Media;
using AVFoundation;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using Kardamon.Services;
using Kardamon.ViewModels;
using MediaManager;
using MediaManager.Library;
using MediaManager.Media;
using MediaManager.Playback;
using MediaManager.Player;
using MediaPlayer;
using Projektanker.Icons.Avalonia.MaterialDesign;

namespace Kardamon.iOS;

// The UIApplicationDelegate for the application. This class is responsible for launching the 
// User Interface of the application, as well as listening (and optionally responding) to 
// application events from iOS.
[Register("AppDelegate")]
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public partial class AppDelegate : AvaloniaAppDelegate<App>
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        Projektanker.Icons.Avalonia.IconProvider.Current.Register<MaterialDesignIconProvider>();
        //CrossPushNotification.Current.RegisterForPushNotifications();
        var services = new ContainerBuilder();
        services.RegisterType<XPlatformPlaybackService>().As<IPlayback>().SingleInstance();
        IoC.RegisterServices(services);
       // CheckNotifyPermission();
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            ;
    }

}

public class XPlatformPlaybackService : IPlayback
{
    
    public event Action<long>? TimeChanged;
    public event Action<bool>? StateChanged;
    public event Action<SongModel>? SongChanged;
    public event Action? PlayFinished;
    public bool IsPaused { get; set; }
    public void Play(SongModel s)
    {
        CrossMediaManager.Current.Stop();
        CrossMediaManager.Current.Queue.Clear();
        CrossMediaManager.Current.Queue.Add(new MediaItem(s.FilePath));
        SongChanged?.Invoke(s);
        StateChanged?.Invoke(true);
        CrossMediaManager.Current.Play();
        //CrossMediaManager.Ios.Notification.
    }

    private void CurrentOnMediaItemFinished(object? sender, MediaItemEventArgs e)
    {
        PlayFinished?.Invoke();
    }

    private void CurrentOnPositionChanged(object? sender, PositionChangedEventArgs e)
    {
        TimeChanged?.Invoke(Convert.ToInt64(e.Position.TotalSeconds));
    }

    public void Pause()
    {
        
        if(IsPaused == false)
        {
            //_mediaPlayer.Play();
            CrossMediaManager.Current.Pause();
            IsPaused = true;
        }
        else
        {
            //_mediaPlayer.Pause();
            CrossMediaManager.Current.Play();
            IsPaused = false;
        }
        
        StateChanged?.Invoke(!IsPaused);
    }

    public void SeekTo(long newTime)
    {
        CrossMediaManager.Current.SeekTo(TimeSpan.FromSeconds(newTime));
    }

    public void Init()
    {
        CrossMediaManager.Current.Init();
        CrossMediaManager.Current.PositionChanged += CurrentOnPositionChanged;
        CrossMediaManager.Current.StateChanged += CurrentOnStateChanged;
        CrossMediaManager.Current.Notification.ShowNavigationControls = false;
        CrossMediaManager.Current.MediaItemFinished += CurrentOnMediaItemFinished;

    }

    private void CurrentOnStateChanged(object? sender, StateChangedEventArgs e)
    {
        StateChanged?.Invoke(e.State != MediaPlayerState.Paused);
    }
}