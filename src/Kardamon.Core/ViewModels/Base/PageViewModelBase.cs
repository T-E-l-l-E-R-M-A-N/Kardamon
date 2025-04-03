using Prism.Mvvm;

namespace Kardamon.Core
{
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
}