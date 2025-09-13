using AngleSharp;
using AngleSharp.Dom;
using Kardamon.ViewModels;
using LibVLCSharp.Shared;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;

namespace Kardamon.Services;

public class WebSearchService
{
    private string _json = PlatformHelper.IsMac || PlatformHelper.IsWindows ? Environment.CurrentDirectory + "/web.json" : Microsoft.Maui.Storage.FileSystem.Current.CacheDirectory + "/web.json";
    public async Task<ReadOnlyCollection<SongModel>> GetPopularAsync()
    {
        try
        {
            var config = Configuration.Default.WithDefaultLoader();
            var address = "https://rus.hitmotop.com/";
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(address);
            var popularList = document.QuerySelector("div.tracks");
            var items = popularList.QuerySelectorAll("li.tracks__item");

            var list = new List<SongModel>();
            foreach (var element in items)
            {
                list.Add(await BuildSongModel(element));
            }
            return list.ToList().AsReadOnly();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ReadOnlyCollection<SongModel>> GetSuggestionsAsync()
    {
        try
        {
            var config = Configuration.Default.WithDefaultLoader();
            var address = "https://rus.hitmotop.com/";
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(address);
            var popularList = document.QuerySelectorAll("div.tracks").LastOrDefault();
            var items = popularList.QuerySelectorAll("li.tracks__item");

            var list = new List<SongModel>();
            foreach (var element in items)
            {
                list.Add(await BuildSongModel(element));
            }
            return list.ToList().AsReadOnly();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task ChangeFavorite(SongModel song)
    {
        
        if (File.Exists(_json))
        {
            var file = await File.ReadAllTextAsync(_json);
            var favorites = JsonConvert.DeserializeObject<List<SongModel>>(file);
            var index = favorites.IndexOf(favorites.FirstOrDefault(x=>x.Id == song.Id));
            if (favorites.Find(x=>x.Id == song.Id) != null)
            {
                song.IsFavorite = false;
                favorites.RemoveAt(index);
            }
            else
            {
                song.IsFavorite = true;
                favorites.Insert(0, song);
            }
        
            File.WriteAllText(_json, JsonConvert.SerializeObject(favorites, Formatting.Indented));
        }
        else
        {
            var list = new List<SongModel>();
            song.IsFavorite = true;
            list.Add(song);
            File.WriteAllText(_json, JsonConvert.SerializeObject(list, Formatting.Indented));
        }
    }

    public async Task<ReadOnlyCollection<SongModel>?> GetFavoritesAsync()
    {
        if (File.Exists(_json))
        {
            var file = await File.ReadAllTextAsync(_json);
            var favorites = JsonConvert.DeserializeObject<List<SongModel>>(file);
            return await Task.FromResult(favorites.AsReadOnly());
        }
        
        return null;
    }

    public async Task<ReadOnlyCollection<SongModel>> SearchAsync(string query)
    {
        try
        {
            var config = Configuration.Default.WithDefaultLoader();
            var address = $"https://rus.hitmotop.com/search?q={query}";
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(address);
            var listEl = document.QuerySelector("ul.tracks__list");
            if (listEl != null)
            {
                var items = listEl.QuerySelectorAll("li.tracks__item");

                var list = new List<SongModel>();
                foreach (var element in items)
                {
                    list.Add(await BuildSongModel(element));
                }
                return list.ToList().AsReadOnly();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return null;
    }
    

    private async Task<SongModel> BuildSongModel(IElement element)
    {
        string file = "";
        
        if(File.Exists(_json))
            file = await File.ReadAllTextAsync(_json);
        var favorites = JsonConvert.DeserializeObject<List<SongModel>>(file);
        
        var track_info_r =  element.QuerySelector(".track__info-r");
        var a = track_info_r.QuerySelector("a");
        var sourceUrl = a.GetAttribute("href");
        var download_span = track_info_r.QuerySelector("span.track__download-btn");
        var id = download_span.GetAttribute("data-track-id");
        var time = track_info_r.QuerySelector("div.track__fulltime").TextContent;
        
        var track__info_l = element.QuerySelector(".track__info-l");
        var title = element.QuerySelector("div.track__title").TextContent.Replace("\n                                                    ", "")
            .Replace("\n                                            ", "");
        var artist = element.QuerySelector("div.track__desc").TextContent;
        var img = element.QuerySelector("div.track__img");
        var imgSource = img.GetAttribute("style").Replace("background-image: url('", "")
            .Replace("');", "");

        var song = new SongModel
        {
            Id = int.Parse(id),
            Album = "hitmo",
            Artist = artist,
            Name = title,
            FilePath = sourceUrl,
            ImagePath = imgSource,
            Time = time,
            IsFavorite = favorites != null && (favorites.Find(x=>x.Id == int.Parse(id)) is not null ? true : false)
        };
        
        return song;
    }
}