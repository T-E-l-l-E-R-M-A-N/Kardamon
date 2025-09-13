using Kardamon.ViewModels;

namespace Kardamon.Factory;

public class PageFactory
{
    public IEnumerable<IPage> GetPages() => IoC.Resolve<IEnumerable<IPage>>();
    
    public IndexPageViewModel GetIndexPage() => GetPages().OfType<IndexPageViewModel>().FirstOrDefault();
    public MyMusicPageViewModel GetMyMusicPage() => GetPages().OfType<MyMusicPageViewModel>().FirstOrDefault();
    public NowPlayingPageViewModel  GetNowPlayingPage() => GetPages().OfType<NowPlayingPageViewModel>().FirstOrDefault();
    public ExplorePageViewModel  GetExplorePage() => GetPages().OfType<ExplorePageViewModel>().FirstOrDefault();
    public SearchPageViewModel GetSearchPage() => GetPages().OfType<SearchPageViewModel>().FirstOrDefault();
}