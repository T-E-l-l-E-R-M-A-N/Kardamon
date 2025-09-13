using System.Net;
using Kardamon.Factory;
using Kardamon.Services;
using LibVLCSharp.Shared;
using Newtonsoft.Json;

namespace Kardamon.ViewModels;

public partial class IndexPageViewModel : ViewModelBase, IPage
{
    private readonly LibraryService  _libraryService;
    private readonly WebSearchService _webSearchService;
    private readonly MiniPlayerViewModel _miniPlayerViewModel;

    [ObservableProperty] private ObservableCollection<SongModel>? _favorites;
    [ObservableProperty] private ObservableCollection<SongModel>? _popular;
    
    [ObservableProperty] private string _name;
    public PageType Type => PageType.Index;

    public IndexPageViewModel(LibraryService libraryService, WebSearchService webSearchService, MiniPlayerViewModel miniPlayerViewModel)
    {
        _libraryService = libraryService;
        _webSearchService = webSearchService;
        _miniPlayerViewModel = miniPlayerViewModel;
        Name = "listen now";
        _libraryService.FavoritesChanged += LibraryServiceOnFavoritesChanged;
        Init();
    }

    private async void LibraryServiceOnFavoritesChanged()
    {
        try
        {
            var favorites = await _libraryService.GetFavoritesAsync();
            var onlineFavorites = await _webSearchService.GetFavoritesAsync();
            var list = new List<SongModel>();
            list.AddRange(favorites);
            list.AddRange(onlineFavorites);
            Favorites = new ObservableCollection<SongModel>(list);
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }
    
    [RelayCommand]
    private async Task Play(SongModel s)
    {
        await _miniPlayerViewModel.PlaySingleAsync(s, true);
    }

    [RelayCommand]
    private async Task PlayMany(IEnumerable<SongModel> s)
    {
        await _miniPlayerViewModel.EnqueueAndPlayAsync(s);
    }

    public async Task Init()
    {
        var popular = await _webSearchService.GetPopularAsync();
        Popular = new ObservableCollection<SongModel>(popular);
        
        //var favorites = await _libraryService.GetFavoritesAsync();
        var favorites = await _libraryService.GetFavoritesAsync();
        var onlineFavorites = await _webSearchService.GetFavoritesAsync();
        
        
        var list = new List<SongModel>();
        list.AddRange(favorites);
        list.AddRange(onlineFavorites);
        Favorites = new ObservableCollection<SongModel>(list);
    }
}