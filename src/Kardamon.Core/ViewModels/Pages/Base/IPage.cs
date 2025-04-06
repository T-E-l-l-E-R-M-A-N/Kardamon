namespace Kardamon.Core.ViewModels.Pages
{
    public interface IPage
    {
        string Name { get; }
        PageType Type { get; }
    }
}