using Prism.Mvvm;

namespace Kardamon.Core
{
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
}