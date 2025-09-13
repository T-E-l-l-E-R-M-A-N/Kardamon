using Kardamon.ViewModels;
using LibVLCSharp.Shared;
using MediaManager;
using MediaManager.Media;
using MediaManager.Playback;

namespace Kardamon.Services;

public interface IPlayback
{
    event Action<long> TimeChanged;
    event Action<bool> StateChanged;
    event Action<SongModel>? SongChanged;
    event Action? PlayFinished;
    bool IsPaused { get; set; }
    void Play(SongModel s);
    void Pause();
    void SeekTo(long newTime);
    void Init();
}