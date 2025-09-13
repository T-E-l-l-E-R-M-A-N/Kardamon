using System.Net;
using Kardamon.Factory;
using Kardamon.ViewModels;
using LibVLCSharp.Shared;

namespace Kardamon.Services;

public class DownloadService
{
    private readonly NotificationService  _notificationService;
    private readonly NavigationService  _navigationService;
    private readonly LibraryService   _libraryService;
    private readonly PageFactory  _pageFactory;
    private readonly string _downloads = PlatformHelper.IsMac || PlatformHelper.IsWindows ? Environment.CurrentDirectory + "/" : Microsoft.Maui.Storage.FileSystem.Current.CacheDirectory + "/";

    public DownloadService(NotificationService notificationService, LibraryService libraryService, PageFactory pageFactory, NavigationService navigationService)
    {
        _notificationService = notificationService;
        _libraryService = libraryService;
        _pageFactory = pageFactory;
        _navigationService = navigationService;
    }
    
    public async Task<string> DownloadForPreviewAsync(SongModel song)
    {
        using var wc = new WebClient();
        var newFileName = _downloads + $"{song.Id}.mp3";
        if (!File.Exists(newFileName))
        {
            await Task.Run(() =>
            {
                _notificationService.Send("Music", "Buffering...", 3);
                wc.DownloadFile(song.FilePath, newFileName);
                song.FilePath = newFileName;
            });
        }
        
        return newFileName;
    }

    public async Task SaveAsync(SongModel song)
    {
        using var wc = new WebClient();
        var newFileName = _downloads + $"{song.Id}.mp3";
        if (!File.Exists(newFileName))
        {
            await Task.Run(async () =>
            {
                wc.DownloadFile(song.FilePath, newFileName);
                song.FilePath = newFileName;
                _notificationService.Send("Music", "Add to my music", 3);
                song.IsDownloaded = true;
                _libraryService.AddToLibrary(song);
                var lib = _pageFactory.GetMyMusicPage();
                await lib.ScanFilesCommand.ExecuteAsync(null!);
                _navigationService.NavigateTo(lib);
            });
        }
        else
        {
            song.IsDownloaded = true;
            _libraryService.AddToLibrary(song);
            var lib = _pageFactory.GetMyMusicPage();
            await lib.ScanFilesCommand.ExecuteAsync(null!);
            _navigationService.NavigateTo(lib);
        }
    }
}