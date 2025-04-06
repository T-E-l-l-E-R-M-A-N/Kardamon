using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Kardamon.Core.ViewModels.Pages;
using Prism.Commands;
using Prism.Mvvm;

namespace Kardamon.Core.ViewModels.Windows
{
    public class SelectionHost : BindableBase
    {
        public object SelectedItem { get; set; }
    }
    public class MainWindowViewModel : BindableBase
    {
        private readonly PageFactory _pageFactory;
        private readonly ModelFactory _modelFactory;
        public IPlayer PlayerService { get; set; }
        public SelectionHost SelectionHost { get; set; }
        public ObservableCollection<IPage> Pages { get; set; }
        public IPage CurrentPage { get; set; }
        public bool IsLoading { get; set; }
        public MainWindowViewModel(PageFactory pageFactory, ModelFactory modelFactory, IPlayer player, SelectionHost selectionHost)
        {
            _pageFactory = pageFactory;
            _modelFactory = modelFactory;
            PlayerService = player;
            SelectionHost = selectionHost;
        }

        public DelegateCommand<AudioModel> PlayCommand => new DelegateCommand<AudioModel>(a =>
        {
            PlayerService.Load(new AudioModel[] { a });
            PlayerService.CurrentMedia = a;
            PlayerService.Play();
        });

        public DelegateCommand<IEnumerable<AudioModel>> PlayAllCommand => new DelegateCommand<IEnumerable<AudioModel>>(
            (l) =>
            {
                PlayerService.Load(l);
                PlayerService.CurrentMedia = PlayerService.Queue.FirstOrDefault()!;
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