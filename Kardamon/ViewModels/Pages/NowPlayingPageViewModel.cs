using Kardamon.Extensions;
using Kardamon.Services;

namespace Kardamon.ViewModels;

public partial class NowPlayingPageViewModel : ViewModelBase, IPage
{
    private readonly WebSearchService  _webSearchService;
    private readonly LibraryService  _libraryService;
    private readonly NavigationService  _navigationService;
    [ObservableProperty]private MiniPlayerViewModel  _miniPlayer;
    [ObservableProperty] private string _name;
    [ObservableProperty] private bool _repeatMode;
    public PageType Type => PageType.NowPlaying;
    [ObservableProperty] bool _listIsVisible;

    public NowPlayingPageViewModel(MiniPlayerViewModel miniPlayer, NavigationService navigationService, WebSearchService webSearchService)
    {
        _miniPlayer = miniPlayer;
        _navigationService = navigationService;
        _webSearchService = webSearchService;
        Name = "текущий";
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
    [RelayCommand]
    private async Task Play(SongModel s)
    {
        await MiniPlayer.PlaySingleAsync(s, false);
    }

    [RelayCommand]
    private void Return()
    {
        _navigationService.GoToExplore();
    }

    [RelayCommand]
    private void ReturnToExplore()
    {
        _navigationService.GoToExplore();
    }

    [RelayCommand]
    private void Shuffle()
    {
        var list = MiniPlayer.Queue;
        MiniPlayer.Queue = new ObservableCollection<SongModel>(list.Shuffle());
    }

    [RelayCommand]
    private void Repeat()
    {
        RepeatMode = !RepeatMode;
    }
    [RelayCommand]
    private async Task MarkFavorite(int id)
    {
        try
        {
            var song = MiniPlayer.Queue.FirstOrDefault(x => x.Id == id);
            if (song == null)
                return;
            _webSearchService.ChangeFavorite(song);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }
}