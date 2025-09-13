using Kardamon.ViewModels;
using LibVLCSharp.Shared;
using LiteDB;
using Newtonsoft.Json;
using File = TagLib.File;

namespace Kardamon.Services;

public class LibraryService
{
    private string _db = PlatformHelper.IsMac || PlatformHelper.IsWindows ? Environment.CurrentDirectory + "/data.db" : Microsoft.Maui.Storage.FileSystem.Current.CacheDirectory + "/data.db";
    public event Action FavoritesChanged;
    
    public async Task ScanAsync()
    {
        try
        {
            using var db = new LiteDatabase(_db);
            var songs = db.GetCollection<SongModel>("songs");

            var files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "*.mp3",
                SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var tagFile = File.Create(file);
                var tag = tagFile.Tag;
                if (songs.FindOne(x => x.FilePath == file) != null)
                {
                    continue;
                }

                await Task.Run(() =>
                {
                    var song = new SongModel()
                    {
                        Id = Random.Shared.Next(),
                        Name = tag.Title,
                        IsDownloaded = true,
                        Album = tag.Album ?? "Unknown album",
                        Artist = tag.FirstAlbumArtist ?? "Unknown artist",
                        Time = tagFile.Properties.Duration.ToString("mm\\:ss"),
                        FilePath = file,
                    };
                    songs.Upsert(song);
                });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task<ReadOnlyCollection<SongModel>> GetFavoritesAsync()
    {
        try
        {
            using var db = new LiteDatabase(_db);
            var songs = db.GetCollection<SongModel>("songs");
            return await Task.FromResult(songs.Find(x => x.IsFavorite).ToList().AsReadOnly());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return null;
    }
    public async Task<ReadOnlyCollection<SongModel>> GetAllAsync()
    {
        try
        {
            using var db = new LiteDatabase(_db);
            var songs = db.GetCollection<SongModel>("songs");
            return await Task.FromResult(songs.FindAll().ToList().AsReadOnly());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return null;
    }

    public void ChangeFavorite(int id)
    {
        using var db = new LiteDatabase(_db);
        var songs = db.GetCollection<SongModel>("songs");
        if (songs.FindById(id) is SongModel song)
        {
            song.IsFavorite = !song.IsFavorite;
        
            songs.Upsert(song);
            FavoritesChanged?.Invoke();
        }

    }

    public void AddToLibrary(SongModel s)
    {
        using var db = new LiteDatabase(_db);
        var songs = db.GetCollection<SongModel>("songs");
        songs.Upsert(s);
    }
    
}