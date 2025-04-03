using System.Collections.ObjectModel;
using AngleSharp;
using Microsoft.Extensions.DependencyInjection;

namespace Kardamon.Core;

public class MainWindowViewModel : BindableBase
{
    private readonly PageFactory _pageFactory;
    public ObservableCollection<IPage> Pages { get; set; }
    public IPage CurrentPage { get; set; }
    public bool IsLoading { get; set; }
    public MainWindowViewModel(PageFactory pageFactory)
    {
        _pageFactory = pageFactory;
    }

    public void Init()
    {
        var pages = _pageFactory.GetPageViewModels();
        Pages = new ObservableCollection<IPage>(pages);
        CurrentPage = Pages[0];
    }
}

public class SearchPageViewModel : PageViewModelBase
{
    public ObservableCollection<ResultItemModel> Results { get; set; }
    public DelegateCommand<string> SearchText { get; set; }
    public SearchPageViewModel() : base("Search", PageType.Search)
    {
    }
}

public class ModelFactory
{
    private IConfiguration _angleSharpConfiguration;
    private IBrowsingContext _browsingContext;

    public void Init()
    {
        _angleSharpConfiguration = Configuration.Default.WithDefaultLoader();
        _browsingContext = BrowsingContext.New(_angleSharpConfiguration);
    }
    
    public async Task<List<AudioModel>> OnlineGetPopular(int page)
    {
        var document = await _browsingContext.OpenAsync($"https://rus.hitmotop.com/songs/top-today/start/{48*0}");
        var ul = document.QuerySelector(".tracks__list");
        
    }

    public async Task<List<AudioModel>>  OnlineGetNew(int page)
    {
        return null!;
    }

    public async Task<List<AudioModel>>  OnlineGetTopRating(int page)
    {
        return null!;
    }
}

public class ResultItemModel 
{
    public IModel Model { get; set; }
    public ResultType Type { get; }
}

public enum ResultType
{
    Artist,
    Collection,
    Audio
}
public class PageFactory
{
    public IPage GetPageViewModel(PageType type)
    {
        return GetPageViewModels().FirstOrDefault(x => x.Type == type)!;
    }

    public IList<IPage> GetPageViewModels()
    {
        return IoC.Resolve<IEnumerable<IPage>>().ToList();
    }
}
public interface IModel
{
    int Id { get; }
    string Name { get; }
}

public abstract class ModelBase : BindableBase, IModel
{
    public int Id { get; }
    public string Name { get; } 
    protected ModelBase(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
public class AudioModel : ModelBase
{
    public string Title { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public string Source { get; set; } = null!;

    public AudioModel(int id, string name)
        : base(id, name)
    {
        
    }
}

public class ArtistModel : ModelBase
{
    public ArtistModel(int id, string name)
        : base(id, name)
    {
        
    }
}

public class CollectionModel : ModelBase
{
    public int ArtistId { get; set; }
    public CollectionType CollectionType { get; }
    public CollectionModel(CollectionType collectionType, int id, string name)
        : base(id,name)
    {
        CollectionType = collectionType;
    }
}

public enum CollectionType
{
    Collection,
    Album,
    Compilation
}

public abstract class PageViewModelBase : BindableBase, IPage
{
    public string Name { get; }
    public PageType Type { get; }
    
    protected PageViewModelBase(string name, PageType type)
    {
        Name = name;
        Type = type;
    }
}



public static class IoC
{
    private static IServiceProvider _provider;

    public static void Build()
    {
        var services = new ServiceCollection();

        _provider = services.BuildServiceProvider();
    }

    public static T Resolve<T>() => _provider.GetRequiredService<T>();
}

public interface IPage
{
    string Name { get; }
    PageType Type { get;}
}

public enum PageType
{
    Home,
    Library,
    Favorites,
    Downloads,
    Search,
    NowPlaying,
    Settings
}