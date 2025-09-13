using Kardamon.Services;

namespace Kardamon.ViewModels;

public partial class NowPlayingPageViewModel : ViewModelBase, IPage
{
    private readonly WebSearchService  _webSearchService;
    private readonly LibraryService  _libraryService;
    [ObservableProperty]private MiniPlayerViewModel  _miniPlayer;
    [ObservableProperty] private string _name;
    public PageType Type => PageType.NowPlaying;
    [ObservableProperty] bool _listIsVisible;

    public NowPlayingPageViewModel(MiniPlayerViewModel miniPlayer)
    {
        _miniPlayer = miniPlayer;
        Name = "playing";
    }

    [RelayCommand]
    private void ToggleListVisibility()
    {
        ListIsVisible = !ListIsVisible;
    }

    [RelayCommand]
    private async Task ToggleFavorite(SongModel song)
    {
        if (song.IsDownloaded)
        {
            _libraryService.ChangeFavorite(song.Id);
        }
        else
        {
            await _webSearchService.ChangeFavorite(song);
        }
    }
}