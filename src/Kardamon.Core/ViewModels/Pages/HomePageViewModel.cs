using System.Collections.ObjectModel;
using Kardamon.Core.ViewModels.Pages;
using Kardamon.Core.ViewModels.Windows;

namespace Kardamon.Core
{
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
}