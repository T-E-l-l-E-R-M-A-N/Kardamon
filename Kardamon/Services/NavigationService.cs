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

    public void GoToIndex()
    {
        var page = _pageFactory.GetIndexPage();
        NavigateTo(page);
    }

    public void NavigateTo(IPage page)
    {
        CurrentPage = page;
        OnNavigated?.Invoke();
    }
}