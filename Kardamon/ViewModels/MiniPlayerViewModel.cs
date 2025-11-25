using Avalonia.Threading;
using Kardamon.Factory;
using Kardamon.Services;

namespace Kardamon.ViewModels;

public partial class MiniPlayerViewModel : ViewModelBase
{
    private readonly IPlayback _playbackService;
    private readonly PageFactory _pageFactory;
    private readonly DownloadService _downloadService;
    [ObservableProperty] SongModel _song;
    [ObservableProperty] private long _totalTime;
    [ObservableProperty] private long _currentTime;
    [ObservableProperty] private string _currentTimeString;
    [ObservableProperty] private string _totalTimeString;
    [ObservableProperty] private bool _isPlaying;
    [ObservableProperty] private bool _isActive;
    [ObservableProperty] private ObservableCollection<SongModel> _queue;

    public MiniPlayerViewModel(IPlayback playbackService, DownloadService downloadService, PageFactory pageFactory)
    {
        _playbackService = playbackService;
        _downloadService = downloadService;
        _pageFactory = pageFactory;
        _playbackService.SongChanged += PlaybackServiceOnSongChanged;
        _playbackService.StateChanged += PlaybackServiceOnStateChanged;
        _playbackService.TimeChanged += PlaybackServiceOnTimeChanged;
        _playbackService.PlayFinished += PlaybackServiceOnPlayFinished;
    }

    private void PlaybackServiceOnPlayFinished()
    {
        if (Queue != null && Queue.Any())
        {
            if (forwardCommand != null && ForwardCommand.CanExecute(null)) 
                forwardCommand.Execute(null);
        }
        else
        {
            IsPlaying = false;
            Song = null!;
            TotalTime = 0;
            CurrentTime = 0;
            CurrentTimeString = "00:00";
            TotalTimeString = "00:00";
            IsActive = false;
        }
    }
    
    

    public async Task PlaySingleAsync(SongModel s, bool startsNewQueue = false)
    {
        try
        {
            await Task.Run(() =>
            {
                var file = _downloadService.DownloadForPreviewAsync(s);
                s.FilePath = file;
                Song = s;
                _playbackService.Play(s);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        RewindCommand?.NotifyCanExecuteChanged();
        forwardCommand?.NotifyCanExecuteChanged();
    }

    public async Task EnqueueAndPlayAsync(IEnumerable<SongModel> s)
    {
        Queue = new ObservableCollection<SongModel>(s);
        await PlaySingleAsync(Queue[0]);
    }

    private void PlaybackServiceOnStateChanged(bool obj)
    {
        IsPlaying = obj;
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            RewindCommand?.NotifyCanExecuteChanged();
            forwardCommand?.NotifyCanExecuteChanged();
        });
    }

    private void PlaybackServiceOnTimeChanged(long obj)
    {
        var time = TimeSpan.FromSeconds(obj);
        var duration = TimeSpan.Parse($"00:{Song.Time}");
        var timeString = time.ToString("mm\\:ss");
        var durationString = duration.ToString("mm\\:ss");
        CurrentTime = (long)time.TotalSeconds;
        TotalTime = (long)duration.TotalSeconds;
        CurrentTimeString =  timeString;
        TotalTimeString =   durationString;
    }

    private void PlaybackServiceOnSongChanged(SongModel? obj)
    {
        Song = obj;
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            RewindCommand?.NotifyCanExecuteChanged();
            forwardCommand?.NotifyCanExecuteChanged();
        });
    }

    [RelayCommand]
    private void TogglePlay()
    {
        _playbackService.Pause();
    }

    [RelayCommand]
    private void TogglePlayPause()
    {
        _playbackService.Pause();
    }

    private bool CsnForward() => Queue != null! && Queue.Any() & Queue.IndexOf(Song) < Queue.Count - 1;
    [RelayCommand(CanExecute = "CsnForward")]
    private void Forward()
    {
        if (Queue.Any())
        {
            var nowPl = _pageFactory.GetNowPlayingPage();
            if (Song.Id != Queue.LastOrDefault().Id)
            {
                var index = Queue.IndexOf(Song);
                
                index++;
                var item = Queue.ElementAt(index);
                PlaySingleAsync(item);
            }
            else
            {
                if (nowPl.RepeatMode)
                {
                    var index = 0;
                    var item = Queue.ElementAt(index);
                    PlaySingleAsync(item);
                }
            }
        }
    }
    
    private bool CsnRewind()
    {
        return Queue != null! && Queue.Any() & Queue.IndexOf(Song) > 0;
    }

    [RelayCommand(CanExecute = "CsnRewind")]
    private void Rewind()
    {
        if (Queue.Any())
        {
            if (Song.Id != Queue.FirstOrDefault().Id)
            {
                var index = Queue.IndexOf(Song);
                index--;
                var item = Queue.ElementAt(index);
                PlaySingleAsync(item);
            }
        }
    }

    [RelayCommand]
    private void SeekToStart()
    {
    }

    [RelayCommand]
    private void SeekTo(long d)
    {
        _playbackService.SeekTo(d);
    }
}