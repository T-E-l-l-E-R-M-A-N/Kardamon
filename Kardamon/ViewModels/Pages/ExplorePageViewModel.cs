using System.Net;
using Kardamon.Extensions;
using Kardamon.Factory;
using Kardamon.Services;
using LibVLCSharp.Shared;
using MediaManager;

namespace Kardamon.ViewModels;

public partial class ExplorePageViewModel : ViewModelBase, IPage
{
    private readonly NavigationService _navigationService;
    private readonly WebSearchService _webSearchService;
    private readonly MiniPlayerViewModel _miniPlayerViewModel;
    private readonly DownloadService _downloadService;
    private readonly PageFactory _pageFactory;
    [ObservableProperty] private ObservableCollection<SongModel>? _tops;
    [ObservableProperty] private ObservableCollection<SongModel>? _favorites;
    [ObservableProperty] private SongModel? _selectedSong;
    [ObservableProperty] private string? _searchText;
    [ObservableProperty] private string? _name;
    [ObservableProperty] private bool? _contextMenuIsOpen;
    [ObservableProperty] private SongModel? _contextMenuDataContext;
    public PageType Type => PageType.Explore;

    public ExplorePageViewModel(WebSearchService webSearchService, MiniPlayerViewModel miniPlayerViewModel, DownloadService downloadService, NavigationService navigationService, PageFactory pageFactory)
    {
        _webSearchService = webSearchService;
        _miniPlayerViewModel = miniPlayerViewModel;
        _downloadService = downloadService;
        _navigationService = navigationService;
        _pageFactory = pageFactory;
        Name = "главная";
        Task.Run(async () =>
        {
            await Init();
        });
    }

    public async Task Init()
    {
        var tops = await _webSearchService.GetSuggestionsAsync();
        var favs = await _webSearchService.GetFavoritesAsync();
        if (favs != null) Favorites = new ObservableCollection<SongModel>(favs);
        Tops = new ObservableCollection<SongModel>(tops);
    }

   

    [RelayCommand]
    private async Task Download(SongModel s)
    {
        await _downloadService.SaveAsync(s);
    }

    [RelayCommand]
    private async Task PlayMany(IEnumerable<SongModel> s)
    {
        var list = s.ToList();
        if (Favorites != null && Favorites.Any() && s != Favorites)
        {
            list.InsertRange(0, Favorites);
        }
        
        await _miniPlayerViewModel.EnqueueAndPlayAsync(list);
        _navigationService.GoToNowPlaying();
    }

    [RelayCommand]
    private void OpenSearch(string query)
    {
        var page = _pageFactory.GetSearchPage();
        page.SearchText = query;
        page.SearchCommand.Execute(null);
        _navigationService.NavigateTo(page);
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
    

    [RelayCommand]
    private async Task Shuffle()
    {
        if (Tops != null)
        {
            var list = Tops.ToList();
            if (Favorites != null && Favorites.Any())
            {
                list.InsertRange(0, Favorites);
            }
            if (Favorites != null)
            {
                var shuffle = list.Shuffle();
                await _miniPlayerViewModel.EnqueueAndPlayAsync(shuffle);
            }
        }
        else
        {
            if (Favorites != null && Favorites.Any())
            {
                var shuffle = Favorites.Shuffle();
                await _miniPlayerViewModel.EnqueueAndPlayAsync(shuffle);
            }
        }
        
        _navigationService.GoToNowPlaying();
    }
}