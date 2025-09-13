using Kardamon.Services;
using Newtonsoft.Json;

namespace Kardamon.ViewModels;

public partial class MyMusicPageViewModel : ViewModelBase, IPage
{
    private readonly NotificationService _notificationService;
    private readonly LibraryService _libraryService;
    private readonly MiniPlayerViewModel _miniPlayerViewModel;
    [ObservableProperty] ObservableCollection<SongModel>  _songs;
    
    [ObservableProperty] private string _name;
    public PageType Type => PageType.MyMusic;

    public MyMusicPageViewModel(LibraryService libraryService, MiniPlayerViewModel miniPlayerViewModel, NotificationService notificationService)
    {
        _libraryService = libraryService;
        _miniPlayerViewModel = miniPlayerViewModel;
        _notificationService = notificationService;
        Name = "music";
        _libraryService.FavoritesChanged += LibraryServiceOnFavoritesChanged;
        ScanFilesCommand.Execute(null);
    }

    private async void LibraryServiceOnFavoritesChanged()
    {
        await ScanFiles();
    }

    [RelayCommand]
    private async Task MarkFavorite(int id)
    {
        var song = Songs.FirstOrDefault(x => x.Id == id);
        var index = Songs.IndexOf(song); 
        _libraryService.ChangeFavorite(id);
        _notificationService.Send("Music", song.IsFavorite ? $"{song.Name} add to favorites" : $"Removed from favorites {song.Name}", 3);
        await ScanFiles();
    }

    [RelayCommand]
    private async Task ScanFiles()
    {
        await _libraryService.ScanAsync();
        var songs = await _libraryService.GetAllAsync();
        Songs =  new ObservableCollection<SongModel>(songs);
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
}