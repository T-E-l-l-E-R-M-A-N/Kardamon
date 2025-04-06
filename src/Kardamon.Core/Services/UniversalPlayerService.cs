using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using LibVLCSharp.Shared;
using Prism.Mvvm;

namespace Kardamon.Core
{
    public class UniversalPlayerService : BindableBase, IPlayer
    {
        private LibVLC _libVlc;
        private MediaPlayer _player;
        public bool Buffering { get; set; }
        public AudioModel CurrentMedia { get; set; }
        public IEnumerable<AudioModel> Queue { get; set; }
        public double CurrentTime { get; set; }
        public string CurrentTimeString { get; set; }
        public double TotalTime { get; set; }
        public string TotalTimeString { get; set; }

        public void Init()
        {
            _libVlc = new LibVLC();
            _player = new MediaPlayer(_libVlc);
            _player.PositionChanged += PlayerOnPositionChanged;
        }

        private void PlayerOnPositionChanged(object? sender, MediaPlayerPositionChangedEventArgs e)
        {
            var dur = TimeSpan.FromMilliseconds(_player.Media.Duration);
            var time = TimeSpan.FromMilliseconds(_player.Time);

            CurrentTime = time.TotalSeconds;
            TotalTime = dur.TotalSeconds;

            CurrentTimeString = time.ToString("m\\:ss");
            TotalTimeString = dur.ToString("m\\:ss");
        }

        public void ClearQueue()
        {
            throw new NotImplementedException();
        }


        public void Load(IEnumerable<AudioModel> audios)
        {
            Queue = new ObservableCollection<AudioModel>(audios);
        }

        public void Next()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            _player.Pause();
        }

        public void Play()
        {
            if (CurrentMedia == null)
                CurrentMedia = Queue.FirstOrDefault()!;

            if(CurrentMedia.Source.StartsWith("https://") ||
                CurrentMedia.Source.StartsWith("http://"))
            {
                if (!File.Exists(Environment.CurrentDirectory + "./cache/" + CurrentMedia.Id.ToString()))
                {
                    using (var wc = new WebClient())
                    {
                        if (!Directory.Exists(Environment.CurrentDirectory + "./cache"))
                            Directory.CreateDirectory(Environment.CurrentDirectory + "./cache");

                        wc.DownloadFileCompleted += Downloaded;
                        Buffering = true;
                        wc.DownloadFileAsync(new Uri(CurrentMedia.Source), Environment.CurrentDirectory + "./cache/" + CurrentMedia.Id);
                    }
                }
                else
                {
                    _player.Media = new Media(_libVlc, Environment.CurrentDirectory + "./cache/" + CurrentMedia.Id);
                    _player.Play();
                }
            }
        }

        private void Downloaded(object? sender, AsyncCompletedEventArgs e)
        {
            (sender as WebClient).DownloadFileCompleted -= Downloaded;
            _player.Media = new Media(_libVlc, Environment.CurrentDirectory + "./cache/" + CurrentMedia.Id);
            _player.Play();
            Buffering = false;

        }

        public void Prev()
        {
            throw new NotImplementedException();
        }
    }
}