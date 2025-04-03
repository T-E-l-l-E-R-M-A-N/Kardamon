using System.Collections.Generic;
using System.Linq;

namespace Kardamon.Core
{
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
}