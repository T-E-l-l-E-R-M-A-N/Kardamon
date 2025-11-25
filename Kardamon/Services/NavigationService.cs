using Kardamon.Factory;
using Kardamon.ViewModels;

namespace Kardamon.Services;

public class NavigationService
{
    private readonly PageFactory  _pageFactory;
    public event Action? OnNavigated;
    public IPage? CurrentPage { get; private set; }

    public NavigationService(PageFactory pageFactory)
    {
        _pageFactory = pageFactory;
    }

    public void GoToExplore()
    {
        var page = _pageFactory.GetExplorePage();
        NavigateTo(page);
    }

    public void GoToNowPlaying()
    {
        var page = _pageFactory.GetNowPlayingPage();
        NavigateTo(page);
    }

    public void GoToSearch()
    {
        var page = _pageFactory.GetSearchPage();
        NavigateTo(page);
    }

    public void GoToFavorites()
    {
        var page = _pageFactory.GetFavoritesPage();
        NavigateTo(page);
    }

    public void NavigateTo(IPage page)
    {
        CurrentPage = page;
        OnNavigated?.Invoke();
    }
}