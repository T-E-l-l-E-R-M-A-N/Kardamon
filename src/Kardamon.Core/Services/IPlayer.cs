using System.Collections.Generic;

namespace Kardamon.Core
{
    public interface IPlayer
    {
        AudioModel CurrentMedia { get; set; }
        IEnumerable<AudioModel> Queue { get; set; }
        double CurrentTime { get; set; }
        string CurrentTimeString { get; set; }
        double TotalTime { get; set; }
        string TotalTimeString { get; set; }

        void Init();
        void Load(IEnumerable<AudioModel> audios);
        void Play();
        void Pause();
        void Next();
        void Prev();
        void ClearQueue();
    }
}