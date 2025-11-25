using System.ComponentModel;
using System.Runtime.InteropServices;
using Kardamon.Factory;
using Kardamon.Services;

namespace Kardamon.ViewModels;

public partial class NavigationMenuViewModel : ViewModelBase
{
    private readonly PageFactory  _pageFactory;
    private readonly NavigationService _navigationService;
    private readonly MiniPlayerViewModel _miniPlayerViewModel;
    private readonly SettingsService _settingsService;
    [ObservableProperty] private bool _isOpen;
    [ObservableProperty] ObservableCollection<IPage>? _pages;

    public NavigationMenuViewModel(NavigationService navigationService, PageFactory pageFactory, SettingsService settingsService, MiniPlayerViewModel miniPlayerViewModel)
    {
        _navigationService = navigationService;
        _pageFactory = pageFactory;
        _settingsService = settingsService;
        _miniPlayerViewModel = miniPlayerViewModel;
    }
    
    public void Init()
    {
        //_navigationService.OnNavigated += NavigationServiceOnOnNavigated;
        var pages = _pageFactory.GetGenericPages().OrderBy(x=>x.Name);
        Pages = new ObservableCollection<IPage>(pages);
        _navigationService.NavigateTo(_pageFactory.GetExplorePage());
        _miniPlayerViewModel.PropertyChanged += OnPlayerSongChanged;
        
        IsOpen = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }

    private void OnPlayerSongChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Song")
        {
            if (_miniPlayerViewModel.Song != null)
            {
                if (Pages != null) 
                    Pages.Add(_pageFactory.GetNowPlayingPage());
            }
            else
            {
                if (Pages != null)
                {
                    var nowPlaying = Pages.OfType<NowPlayingPageViewModel>().FirstOrDefault();
                    if (nowPlaying != null)
                    {
                        Pages.Remove(nowPlaying);
                    }
                }
            }
        }
    }

    [RelayCommand]
    private void ToggleIsOpen() => IsOpen = !IsOpen;

    [RelayCommand]
    private void OpenPage(IPage page)
    {
        _navigationService.NavigateTo(page);
        IsOpen = false;
    }

    [RelayCommand]
    private void OpenSearch(string query)
    {
        var page = _pageFactory.GetSearchPage();
        page.SearchText = query;
        page.SearchCommand.Execute(null);
        _navigationService.NavigateTo(page);
    }
}