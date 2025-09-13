using Kardamon.Services;

namespace Kardamon.ViewModels;

public partial class SearchPageViewModel : ViewModelBase, IPage
{
    private readonly NotificationService _notificationService;
    private readonly WebSearchService _webSearchService;
    private readonly MiniPlayerViewModel _miniPlayerViewModel;
    private readonly DownloadService _downloadService;
    private readonly LibraryService _libraryService;
    
    [ObservableProperty] private ObservableCollection<SongModel>? _resultsInWeb;
    [ObservableProperty] private ObservableCollection<SongModel>? _resultsOnDevice;
    [ObservableProperty] private string _searchText;
    [ObservableProperty] private string _name;
    public PageType Type => PageType.Search;
    
    public SearchPageViewModel(WebSearchService webSearchService, NotificationService notificationService, MiniPlayerViewModel miniPlayerViewModel, DownloadService downloadService, LibraryService libraryService)
    {
        _webSearchService = webSearchService;
        _notificationService = notificationService;
        _miniPlayerViewModel = miniPlayerViewModel;
        _downloadService = downloadService;
        _libraryService = libraryService;
        Name = "search";
    }
    
    [RelayCommand]
    private async Task Play(SongModel s)
    {
        await _miniPlayerViewModel.PlaySingleAsync(s, true);
    }

    [RelayCommand]
    private async Task Download(SongModel s)
    {
        await _downloadService.SaveAsync(s);
        _notificationService.Send("Music", $"{s.Name} saving on device store...", 3);

    }
    
    [RelayCommand]
    private async Task Search()
    {
        var webResults =  await _webSearchService.SearchAsync(SearchText);
        var libSongs = await _libraryService.GetAllAsync();
        var localResults = libSongs.Where(x => x.Name.ToLower().Contains(SearchText.ToLower())).ToList();
        localResults.AddRange(libSongs.Where(x =>
        {
            return x.Artist.ToLower().Contains(SearchText.ToLower()) && localResults.Find(d => x.Id == d.Id) == null;
        }));
        localResults.AddRange(libSongs.Where(x =>
        {
            return x.Album.ToLower().Contains(SearchText.ToLower()) && localResults.Find(d => x.Id == d.Id) == null;
        }));
        ResultsInWeb =  new ObservableCollection<SongModel>(webResults);
        ResultsOnDevice = new ObservableCollection<SongModel>(localResults);
    }
    [RelayCommand]
    private async Task MarkFavorite(int id)
    {
        var song = ResultsInWeb.FirstOrDefault(x => x.Id == id);
        if(song == null)
            song = ResultsOnDevice.FirstOrDefault(x => x.Id == id);
        if(song.IsDownloaded)
            _libraryService.ChangeFavorite(id);
        else
        {
            _webSearchService.ChangeFavorite(song);
        }
        _notificationService.Send("Music", song.IsFavorite ? $"{song.Name} add to favorites" : $"Removed from favorites {song.Name}", 3);
        
        
    }
}