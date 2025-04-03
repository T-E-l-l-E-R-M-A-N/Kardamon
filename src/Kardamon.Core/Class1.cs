using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using LibVLCSharp.Shared;
using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Prism.Mvvm;

namespace Kardamon.Core
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly PageFactory _pageFactory;
        private readonly ModelFactory _modelFactory;
        public IPlayer PlayerService { get; set; }
        public ObservableCollection<IPage> Pages { get; set; }
        public IPage CurrentPage { get; set; }
        public bool IsLoading { get; set; }
        public MainWindowViewModel(PageFactory pageFactory, ModelFactory modelFactory, IPlayer player)
        {
            _pageFactory = pageFactory;
            _modelFactory = modelFactory;
            PlayerService = player;
        }

        public DelegateCommand<AudioModel> PlayCommand => new DelegateCommand<AudioModel>(a =>
        {
            PlayerService.Load(new AudioModel[] { a });
            PlayerService.CurrentMedia = a;
            PlayerService.Play();
        });

        public void Init()
        {
            _modelFactory.Init();
            PlayerService.Init();
            var pages = _pageFactory.GetPageViewModels();
            Pages = new ObservableCollection<IPage>(pages);
            CurrentPage = Pages[0];
            (CurrentPage as HomePageViewModel).Init();
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

    public interface IGroup
    {
        string Name { get; set; }
        IEnumerable<object> Items { get; set; }
    }

    public class Group : BindableBase, IGroup
    {
        public string Name { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public interface IPlayer
    {
        AudioModel CurrentMedia { get; set; }
        IEnumerable<AudioModel> Queue { get; set; }

        void Init();
        void Load(IEnumerable<AudioModel> audios);
        void Play();
        void Pause();
        void Next();
        void Prev();
        void ClearQueue();
    }
    public class UniversalPlayerService : BindableBase, IPlayer
    {
        private LibVLC _libVlc;
        private MediaPlayer _player;
        public bool Buffering { get; set; }
        public AudioModel CurrentMedia { get; set; }
        public IEnumerable<AudioModel> Queue { get; set; }
        public void Init()
        {
            _libVlc = new LibVLC();
            _player = new MediaPlayer(_libVlc);
        }
        public void ClearQueue()
        {
            throw new NotImplementedException();
        }


        public void Load(IEnumerable<AudioModel> audios)
        {
            Queue = new ObservableCollection<AudioModel>(audios);
        }

        public void Next()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            _player.Pause();
        }

        public void Play()
        {
            if (CurrentMedia == null)
                CurrentMedia = Queue.FirstOrDefault()!;

            if(CurrentMedia.Source.StartsWith("https://") ||
                CurrentMedia.Source.StartsWith("http://"))
            {
                if (!File.Exists(Environment.CurrentDirectory + "./cache/" + CurrentMedia.Id.ToString()))
                {
                    using (var wc = new WebClient())
                    {
                        if (!Directory.Exists(Environment.CurrentDirectory + "./cache"))
                            Directory.CreateDirectory(Environment.CurrentDirectory + "./cache");

                        wc.DownloadFileCompleted += Downloaded;
                        Buffering = true;
                        wc.DownloadFileAsync(new Uri(CurrentMedia.Source), Environment.CurrentDirectory + "./cache/" + CurrentMedia.Id);
                    }
                }
                else
                {
                    _player.Media = new Media(_libVlc, Environment.CurrentDirectory + "./cache/" + CurrentMedia.Id);
                    _player.Play();
                }
            }
        }

        private void Downloaded(object? sender, AsyncCompletedEventArgs e)
        {
            (sender as WebClient).DownloadFileCompleted -= Downloaded;
            _player.Media = new Media(_libVlc, Environment.CurrentDirectory + "./cache/" + CurrentMedia.Id);
            _player.Play();
            Buffering = false;

        }

        public void Prev()
        {
            throw new NotImplementedException();
        }
    }
    public class HomePageViewModel : PageViewModelBase
    {
        private readonly ModelFactory _modelFactory;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public ObservableCollection<IGroup> Groups { get; set; }
        public IGroup Group { get; set; }

        public HomePageViewModel(ModelFactory modelFactory, MainWindowViewModel mainWindowViewModel) : base("Home", PageType.Home)
        {
            _modelFactory = modelFactory;
            _mainWindowViewModel = mainWindowViewModel;
        }

        public async void Init()
        {
            var popularGroup = new Group() { Name = "Popular" };
            var newGroup = new Group() { Name = "New tracks" };

            _mainWindowViewModel.IsLoading = true;

            var popular = await _modelFactory.OnlineGetPopular(0);
            var newMusic = await _modelFactory.OnlineGetNew(0);

            popularGroup.Items = new ObservableCollection<AudioModel>(popular);
            newGroup.Items = new ObservableCollection<AudioModel>(newMusic);

            Groups = new ObservableCollection<IGroup>(new IGroup[] { popularGroup, newGroup });

            Group = Groups[0];

            _mainWindowViewModel.IsLoading = false;
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
            var document = await _browsingContext.OpenAsync($"https://rus.hitmotop.com/songs/top-today/start/" + 48 * page);
            var ul = document.QuerySelector(".tracks__list");
            var lis = ul.QuerySelectorAll(".tracks__item");
            var list = new List<AudioModel>();
            foreach(var li in lis)
            {
                var track_info_l = li.QuerySelector(".track__info-l");
                var track__title = track_info_l.QuerySelector(".track__title");
                var track__desc = track_info_l.QuerySelector(".track__desc");

                var track__info_r = li.QuerySelector(".track__info-r");
                var a = track__info_r.QuerySelector("a");
                var track__like_btn = track__info_r.QuerySelector(".track__like-btn");

                int id = Convert.ToInt32(track__like_btn.GetAttribute("data-track-id"));
                string source = a.GetAttribute("href");
                string title = track__title.TextContent
                    .Replace("\n                                                    ", "")
                    .Replace("\n                                            ", "");
                string artist = track__desc.TextContent;
                var audioModel = new AudioModel(id, $"{artist} - {title}")
                {
                    Source = source,
                    Title = title,
                    Artist = artist
                };
                list.Add(audioModel);

            }
            return list;

        }

        public async Task<List<AudioModel>> OnlineGetNew(int page)
        {
            var document = await _browsingContext.OpenAsync($"https://rus.hitmotop.com/songs/new/start/{48 * 0}");
            var ul = document.QuerySelector(".tracks__list");
            var lis = ul.QuerySelectorAll(".tracks__item");
            var list = new List<AudioModel>();
            foreach (var li in lis)
            {
                var track_info_l = li.QuerySelector(".track__info-l");
                var track__title = track_info_l.QuerySelector(".track__title");
                var track__desc = track_info_l.QuerySelector(".track__desc");

                var track__info_r = li.QuerySelector(".track__info-r");
                var a = track__info_r.QuerySelector("a");
                var track__like_btn = track__info_r.QuerySelector(".track__like-btn");

                int id = Convert.ToInt32(track__like_btn.GetAttribute("data-track-id"));
                string source = a.GetAttribute("href");
                string title = track__title.TextContent
                    .Replace("\n                                                    ", "")
                    .Replace("\n                                            ", "");
                string artist = track__desc.TextContent;
                var audioModel = new AudioModel(id, $"{artist} - {title}")
                {
                    Source = source,
                    Title = title,
                    Artist = artist
                };
                list.Add(audioModel);

            }
            return list;
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
            : base(id, name)
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

            services.AddScoped<ModelFactory>();
            services.AddScoped<PageFactory>();
            services.AddScoped<IPlayer, UniversalPlayerService>();

            services.AddScoped<MainWindowViewModel>();
            services.AddScoped<IPage, HomePageViewModel>();
            services.AddScoped<IPage, SearchPageViewModel>();

            _provider = services.BuildServiceProvider();
        }

        public static T Resolve<T>() => _provider.GetRequiredService<T>();
    }

    public interface IPage
    {
        string Name { get; }
        PageType Type { get; }
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
}