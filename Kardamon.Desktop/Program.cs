using System;
using System.Timers;
using Autofac;
using Avalonia;
using Kardamon.Services;
using Kardamon.ViewModels;
using ManagedBass;
using MediaManager;
using Projektanker.Icons.Avalonia.MaterialDesign;

namespace Kardamon.Desktop;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        Projektanker.Icons.Avalonia.IconProvider.Current.Register<MaterialDesignIconProvider>();

        var services = new ContainerBuilder();
        services.RegisterType<DesktopPLaybackService>().As<IPlayback>().SingleInstance();
        IoC.RegisterServices(services);
        //CrossMediaManager.Current.Init();
        
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}

public sealed class DesktopPLaybackService : IPlayback
{
    private Timer _timer;
    private MediaPlayer _player;
    
    public event Action<long>? TimeChanged;
    public event Action<bool>? StateChanged;
    public event Action<SongModel>? SongChanged;
    public event Action? PlayFinished;
    public bool IsPaused { get; set; }
    public async void Play(SongModel s)
    {
        if (_player != null)
        {
            try
            {
                _player.Dispose();
                try
                {
                    await _player.LoadAsync(s.FilePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                SongChanged?.Invoke(s);
                _player.Play();
                IsPaused = false;
                StateChanged?.Invoke(true);
                _timer.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        else
        {
            Init();
            Play(s);
        }
        
    }

    public void Pause()
    {
        if (_player.State == PlaybackState.Playing)
        {
            _player.Pause();
            StateChanged?.Invoke(false);
            IsPaused = true;
            _timer.Stop();
        }
        else if (_player.State == PlaybackState.Paused)
        {
            _player.Play();
            StateChanged?.Invoke(true);
            IsPaused = false;
            _timer.Start();
        }
    }

    public void SeekTo(long newTime)
    {
        if (IsPaused)
        {
            _player.Position = TimeSpan.FromSeconds(newTime);
            _player.Play();
            StateChanged?.Invoke(true);
            IsPaused = false;
            _timer.Start();
        }
        else
        {
            Pause();
            _player.Position = TimeSpan.FromSeconds(newTime);
            Pause();
        }
    }

    public void Init()
    {
        _player = new MediaPlayer();
        _player.MediaEnded += PlayerOnMediaEnded;
        _timer = new Timer(1000);
        _timer.Elapsed += TimerOnElapsed;
    }

    private void PlayerOnMediaEnded(object? sender, EventArgs e)
    {
        //SongChanged?.Invoke(null!);
        PlayFinished?.Invoke();
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        TimeChanged?.Invoke(Convert.ToInt64(_player.Position.TotalSeconds));
    }
}