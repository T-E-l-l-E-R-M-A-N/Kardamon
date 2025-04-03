using System.Collections.Generic;

namespace Kardamon.Core
{
    public interface IGroup
    {
        string Name { get; set; }
        IEnumerable<object> Items { get; set; }
    }
}