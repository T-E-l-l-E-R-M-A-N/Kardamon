using System.Collections.Generic;
using Prism.Mvvm;

namespace Kardamon.Core
{
    public class Group : BindableBase, IGroup
    {
        public string Name { get; set; }
        public IEnumerable<object> Items { get; set; }
    }
}