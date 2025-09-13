using System.Net;
using Kardamon.Factory;
using Kardamon.Services;
using LibVLCSharp.Shared;
using Plugin.LocalNotification;
using Plugin.LocalNotification.iOSOption;

namespace Kardamon.ViewModels;

public partial class ExplorePageViewModel : ViewModelBase, IPage
{
    private readonly NotificationService _notificationService;
    private readonly WebSearchService _webSearchService;
    private readonly MiniPlayerViewModel _miniPlayerViewModel;
    private readonly DownloadService _downloadService;
    [ObservableProperty] private ObservableCollection<SongModel>? _tops;
    [ObservableProperty] private SongModel? _selectedSong;
    [ObservableProperty] private string? _searchText;
    [ObservableProperty] private string? _name;
    public PageType Type => PageType.Explore;

    public ExplorePageViewModel(WebSearchService webSearchService, NotificationService notificationService, MiniPlayerViewModel miniPlayerViewModel, DownloadService downloadService)
    {
        _webSearchService = webSearchService;
        _notificationService = notificationService;
        _miniPlayerViewModel = miniPlayerViewModel;
        _downloadService = downloadService;
        Name = "explore";
        Init();
    }

    public async Task Init()
    {
        var tops = await _webSearchService.GetSuggestionsAsync();
        Tops = new ObservableCollection<SongModel>(tops);
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
    }

    [RelayCommand]
    private async Task PlayMany(IEnumerable<SongModel> s)
    {
        await _miniPlayerViewModel.EnqueueAndPlayAsync(s);
    }
    
   

    
}