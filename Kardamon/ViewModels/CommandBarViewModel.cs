using System.ComponentModel;
using Kardamon.Factory;
using Kardamon.Services;

namespace Kardamon.ViewModels;

public partial class CommandBarViewModel : ViewModelBase
{
    private readonly PageFactory  _pageFactory;
    private readonly NavigationService _navigationService;
    private readonly NavigationMenuViewModel _navigationMenu;
    private readonly SettingsService  _settingsService;

    public CommandBarViewModel(NavigationMenuViewModel navigationMenu, SettingsService settingsService, NavigationService navigationService, PageFactory pageFactory)
    {
        _navigationMenu = navigationMenu;
        _settingsService = settingsService;
        _navigationService = navigationService;
        _pageFactory = pageFactory;
        _navigationMenu.PropertyChanged += NavigationMenuOnPropertyChanged;
        _navigationService.OnNavigated += NavigationServiceOnOnNavigated;
    }

    private void NavigationServiceOnOnNavigated()
    {
        OpenSearchCommand.NotifyCanExecuteChanged();
    }

    private void NavigationMenuOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        
        if (e.PropertyName == nameof(_navigationMenu.IsOpen))
        {
            ToggleIsOpenCommand.NotifyCanExecuteChanged();
        }
    }

    private bool CanToggleIsOpen()
    {
        var isMobile = _settingsService.GetSetting("isMobile");
        if (_navigationMenu.IsOpen && bool.Parse(isMobile))
            return false;
        else
        {
            return true;
        }
    }
    private bool CanOpenSearch()
    {
        return _navigationService.CurrentPage != null && _navigationService.CurrentPage.Type !=  PageType.Search;
    }

    [RelayCommand(CanExecute = "CanToggleIsOpen")]
    private void ToggleIsOpen() => _navigationMenu.IsOpen = !_navigationMenu.IsOpen;

    [RelayCommand(CanExecute = "CanOpenSearch")]
    private void OpenSearch()
    {
        var search = _pageFactory.GetSearchPage();
        _navigationService.NavigateTo(search);
    }
    
}