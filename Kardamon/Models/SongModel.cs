namespace Kardamon.ViewModels;

public  partial class SongModel : ViewModelBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string Time  { get; set; }
    public string FilePath { get; set; }
    public string ImagePath { get; set; }
    [ObservableProperty] private bool _isDownloaded;
    [ObservableProperty] private bool _isFavorite;
}