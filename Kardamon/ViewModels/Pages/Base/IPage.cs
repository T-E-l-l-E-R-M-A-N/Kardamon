namespace Kardamon.ViewModels;

public interface IPage
{
    string Name { get; set; }
    PageType Type { get; }
}