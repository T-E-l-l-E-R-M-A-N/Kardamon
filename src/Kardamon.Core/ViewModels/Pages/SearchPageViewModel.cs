﻿using System.Collections.ObjectModel;
using Kardamon.Core.ViewModels.Pages;
using Prism.Commands;

namespace Kardamon.Core
{
    public class SearchPageViewModel : PageViewModelBase
    {
        public ObservableCollection<ResultItemModel> Results { get; set; }
        public DelegateCommand<string> SearchText { get; set; }
        public SearchPageViewModel() : base("Search", PageType.Search)
        {
        }
    }
}