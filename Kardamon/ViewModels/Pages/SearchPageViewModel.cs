using System.ComponentModel;
using Kardamon.Extensions;
using Kardamon.Services;

namespace Kardamon.ViewModels;

public partial class SearchPageViewModel : ViewModelBase, IPage
{
    private readonly NotificationService _notificationService;
    private readonly WebSearchService _webSearchService;
    private readonly MiniPlayerViewModel _miniPlayerViewModel;
    private readonly NavigationService _navigationService;
    private readonly DownloadService _downloadService;
    private readonly LibraryService _libraryService;
    
    [ObservableProperty] private ObservableCollection<SongModel>? _resultsInWeb;
    [ObservableProperty] private ObservableCollection<SongModel>? _resultsOnDevice;
    [ObservableProperty] private string _searchText;
    [ObservableProperty] private string _name;
    [ObservableProperty] private bool? _contextMenuIsOpen;
    [ObservableProperty] private SongModel? _contextMenuDataContext;
    public PageType Type => PageType.Search;
    
    public SearchPageViewModel(WebSearchService webSearchService, NotificationService notificationService, MiniPlayerViewModel miniPlayerViewModel, DownloadService downloadService, LibraryService libraryService, NavigationService navigationService)
    {
        _webSearchService = webSearchService;
        _notificationService = notificationService;
        _miniPlayerViewModel = miniPlayerViewModel;
        _downloadService = downloadService;
        _libraryService = libraryService;
        _navigationService = navigationService;
        Name = "поиск";
        ResultsInWeb = new ObservableCollection<SongModel>();
        ResultsOnDevice = new ObservableCollection<SongModel>();
        PropertyChanged += OnPropertyChanged;
    }

    private async  void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "SearchText")
        {
            await Search();
        }
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
        if (webResults != null) ResultsInWeb = new ObservableCollection<SongModel>(webResults.Where(x=>x.FilePath != null!));
        var favs = await _webSearchService.GetFavoritesAsync();
        ResultsOnDevice = new ObservableCollection<SongModel>(favs.Where(x=>x.Name.ToLower().Contains(SearchText.ToLower()) || x.Artist.Contains(SearchText.ToLower())));
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
        _navigationService.GoToExplore();
        
        
    }

    [RelayCommand]
    private void DismissSearch()
    {
        _navigationService.GoToExplore();
    }
    
    [RelayCommand]
    private async Task PlayMany(IEnumerable<SongModel> s)
    {
        await _miniPlayerViewModel.EnqueueAndPlayAsync(s);
        _navigationService.GoToNowPlaying();
    }

    [RelayCommand]
    private void ResetSearch()
    {
        ResultsInWeb?.Clear();
        SearchText = string.Empty;
        ResultsInWeb = null!;
    }
    
    [RelayCommand]
    private async Task Shuffle()
    {
        if (ResultsInWeb != null && ResultsInWeb.Any())
        {
            var shuffle = ResultsInWeb.Shuffle();
            await _miniPlayerViewModel.EnqueueAndPlayAsync(shuffle);
        }
        _navigationService.GoToNowPlaying();
    }
    
    [RelayCommand]
    private void OpenContextMenu(SongModel s)
    {
        ContextMenuIsOpen = true;
        ContextMenuDataContext = s;
    } 
    [RelayCommand]
    private void DismissContextMenu()
    {
        ContextMenuIsOpen = false;
        ContextMenuDataContext = null!;
    }
}
