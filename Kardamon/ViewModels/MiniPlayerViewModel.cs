using Kardamon.Services;

namespace Kardamon.ViewModels;

public partial class MiniPlayerViewModel : ViewModelBase
{
    private readonly IPlayback _playbackService;
    private readonly DownloadService _downloadService;
    [ObservableProperty] SongModel _song;
    [ObservableProperty] private long _totalTime;
    [ObservableProperty] private long _currentTime;
    [ObservableProperty] private string _currentTimeString;
    [ObservableProperty] private string _totalTimeString;
    [ObservableProperty] private bool _isPlaying;
    [ObservableProperty] private bool _isActive;
    [ObservableProperty] private ObservableCollection<SongModel> _queue;

    public MiniPlayerViewModel(IPlayback playbackService, DownloadService downloadService)
    {
        _playbackService = playbackService;
        _downloadService = downloadService;
        _playbackService.SongChanged += PlaybackServiceOnSongChanged;
        _playbackService.StateChanged += PlaybackServiceOnStateChanged;
        _playbackService.TimeChanged += PlaybackServiceOnTimeChanged;
        _playbackService.PlayFinished += PlaybackServiceOnPlayFinished;
    }

    private void PlaybackServiceOnPlayFinished()
    {
        if (Queue.Any())
        {
            if (forwardCommand != null) 
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
        if (!s.IsDownloaded)
        {
            var file = await _downloadService.DownloadForPreviewAsync(s);
            s.FilePath = file;
        }

        if (startsNewQueue)
        {
            Queue =  new ObservableCollection<SongModel>(new List<SongModel>() {s});
        }

        Song = s;
        _playbackService.Play(s);
    }

    public async Task EnqueueAndPlayAsync(IEnumerable<SongModel> s)
    {
        Queue = new ObservableCollection<SongModel>(s);
        await PlaySingleAsync(Queue[0]);
    }

    private void PlaybackServiceOnStateChanged(bool obj)
    {
        IsPlaying = obj;
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
    }

    [RelayCommand]
    private void TogglePlay()
    {
        _playbackService.Pause();
    }
    
    [RelayCommand]
    private void Forward()
    {
        if (Queue.Any())
        {
            if (Song.Id != Queue.LastOrDefault().Id)
            {
                var index = Queue.IndexOf(Song);
                index++;
                var item = Queue.ElementAt(index);
                PlaySingleAsync(item);
            }
        }
    }
    
    [RelayCommand]
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