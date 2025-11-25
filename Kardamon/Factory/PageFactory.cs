using Kardamon.ViewModels;

namespace Kardamon.Factory;

public class PageFactory
{
    public IEnumerable<IPage> GetPages() => IoC.Resolve<IEnumerable<IPage>>();
    public NowPlayingPageViewModel  GetNowPlayingPage() => GetPages().OfType<NowPlayingPageViewModel>().FirstOrDefault();
    public ExplorePageViewModel  GetExplorePage() => GetPages().OfType<ExplorePageViewModel>().FirstOrDefault();
    public SearchPageViewModel GetSearchPage() => GetPages().OfType<SearchPageViewModel>().FirstOrDefault();
    public FavoritesPageViewModel GetFavoritesPage() => GetPages().OfType<FavoritesPageViewModel>().FirstOrDefault();

    public IEnumerable<IPage> GetGenericPages()
    {
        return
        [
            GetExplorePage(),
            GetFavoritesPage()
        ];
    }
}