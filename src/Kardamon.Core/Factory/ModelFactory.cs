using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp;

namespace Kardamon.Core
{
    public class ModelFactory
    {
        private IConfiguration _angleSharpConfiguration;
        private IBrowsingContext _browsingContext;

        public void Init()
        {
            _angleSharpConfiguration = Configuration.Default.WithDefaultLoader();
            _browsingContext = BrowsingContext.New(_angleSharpConfiguration);
        }

        public async Task<List<AudioModel>> OnlineGetPopular(int page)
        {
            var document = await _browsingContext.OpenAsync($"https://rus.hitmotop.com/songs/top-today/start/" + 48 * page);
            var ul = document.QuerySelector(".tracks__list");
            var lis = ul.QuerySelectorAll(".tracks__item");
            var list = new List<AudioModel>();
            foreach(var li in lis)
            {
                var track__img = li.QuerySelector(".track__img");
                var track_info_l = li.QuerySelector(".track__info-l");
                var track__title = track_info_l.QuerySelector(".track__title");
                var track__desc = track_info_l.QuerySelector(".track__desc");

                var track__info_r = li.QuerySelector(".track__info-r");
                var a = track__info_r.QuerySelector("a");
                var track__like_btn = track__info_r.QuerySelector(".track__like-btn");
                var track__fulltime = track__info_r.QuerySelector(".track__fulltime");

                int id = Convert.ToInt32(track__like_btn.GetAttribute("data-track-id"));
                string image = track__img.GetAttribute("style").Replace("background-image: url('", "")
                    .Replace("');", "");
                string source = a.GetAttribute("href");
                string title = track__title.TextContent
                    .Replace("\n                                                    ", "")
                    .Replace("\n                                            ", "");
                string artist = track__desc.TextContent;
                string time = TimeSpan.Parse("00:" + track__fulltime.TextContent).ToString("m\\:ss");
                var audioModel = new AudioModel(id, $"{artist} - {title}")
                {
                    Source = source,
                    Title = title,
                    Artist = artist,
                    Time = time,
                    Image = image
                };
                list.Add(audioModel);

            }
            return list;

        }

        public async Task<List<AudioModel>> OnlineGetNew(int page)
        {
            var document = await _browsingContext.OpenAsync($"https://rus.hitmotop.com/songs/new/start/{48 * 0}");
            var ul = document.QuerySelector(".tracks__list");
            var lis = ul.QuerySelectorAll(".tracks__item");
            var list = new List<AudioModel>();
            foreach (var li in lis)
            {
                var track__img = li.QuerySelector(".track__img");
                var track_info_l = li.QuerySelector(".track__info-l");
                var track__title = track_info_l.QuerySelector(".track__title");
                var track__desc = track_info_l.QuerySelector(".track__desc");

                var track__info_r = li.QuerySelector(".track__info-r");
                var a = track__info_r.QuerySelector("a");
                var track__like_btn = track__info_r.QuerySelector(".track__like-btn");
                var track__fulltime = track__info_r.QuerySelector(".track__fulltime");

                int id = Convert.ToInt32(track__like_btn.GetAttribute("data-track-id"));
                string image = track__img.GetAttribute("style").Replace("background-image: url('", "")
                    .Replace("');", "");
                string source = a.GetAttribute("href");
                string title = track__title.TextContent
                    .Replace("\n                                                    ", "")
                    .Replace("\n                                            ", "");
                string artist = track__desc.TextContent;
                string time = TimeSpan.Parse("00:" + track__fulltime.TextContent).ToString("m\\:ss");
                var audioModel = new AudioModel(id, $"{artist} - {title}")
                {
                    Source = source,
                    Title = title,
                    Artist = artist,
                    Time = time,
                    Image = image
                };
                list.Add(audioModel);

            }
            return list;
        }
    }
}