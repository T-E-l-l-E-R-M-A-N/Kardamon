using Kardamon.Models;
using LibVLCSharp.Shared;
using LiteDB;

namespace Kardamon.Services;

public class SettingsService
{
    private string _db = PlatformHelper.IsMac || PlatformHelper.IsWindows ? Environment.CurrentDirectory + "/setts.db" : Microsoft.Maui.Storage.FileSystem.Current.CacheDirectory + "/setts.db";

    public void ChangeSetting(string name, string value)
    {
        try
        {
            using var db = new LiteDatabase(_db);
            var settings = db.GetCollection<SettingModel>("settings");
            var item = settings.FindOne(x => x.Name == name);
            if (item == null)
            {
                var setting = new SettingModel() { Name = name, Value = value, Id = Random.Shared.Next() };
                settings.Upsert(setting);
            }
            else
            {
                settings.Delete(item.Id);
                item.Value = value;
                settings.Insert(item);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public string GetSetting(string name)
    {
        try
        {
            using var db = new LiteDatabase(_db);
            var settings = db.GetCollection<SettingModel>("settings");
            var item = settings.FindOne(x => x.Name == name);
            if (item != null) return item.Value;
            return null!;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return null!;
    }

}