using Kardamon.Extensions;
using Kardamon.Services;

namespace Kardamon.ViewModels;

public partial class FavoritesPageViewModel: ViewModelBase, IPage
{
    private readonly WebSearchService _webSearchService;
    private readonly MiniPlayerViewModel _miniPlayerViewModel;
    private readonly DownloadService _downloadService;
    private readonly NavigationService _navigationService;

    
    [ObservableProperty] private string? _name;
    
    [ObservableProperty] private ObservableCollection<SongModel>? _favorites;
    public PageType Type => PageType.Favorites;

    public FavoritesPageViewModel(WebSearchService webSearchService, MiniPlayerViewModel miniPlayerViewModel, DownloadService downloadService, NavigationService navigationService)
    {
        _webSearchService = webSearchService;
        _miniPlayerViewModel = miniPlayerViewModel;
        _downloadService = downloadService;
        _navigationService = navigationService;
        Name = "избранное";
        Task.Run(async () =>
        {
            await Init();
        });
    }
    
    public async Task Init()
    {
        var favs = await _webSearchService.GetFavoritesAsync();
        if (favs != null) Favorites = new ObservableCollection<SongModel>(favs);
    }
    [RelayCommand]
    private async Task PlayMany(IEnumerable<SongModel> s)
    {
        var list = s.ToList();
        if (Favorites != null && Favorites.Any())
        {
            list.InsertRange(0, Favorites);
        }
        
        await _miniPlayerViewModel.EnqueueAndPlayAsync(list);
        _navigationService.GoToNowPlaying();
    }
    [RelayCommand]
    private async Task MarkFavorite(int id)
    {
        var song = Favorites.FirstOrDefault(x => x.Id == id);
        await _webSearchService.ChangeFavorite(song);
        await Init();


    }

    [RelayCommand]
    private async Task Shuffle()
    {
        if (Favorites != null && Favorites.Any())
        {
            var shuffle = Favorites.Shuffle();
            await _miniPlayerViewModel.EnqueueAndPlayAsync(shuffle);
        }
        
        _navigationService.GoToNowPlaying();
    }
}