using System.Collections.ObjectModel;
using AngleSharp.Dom;
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
}