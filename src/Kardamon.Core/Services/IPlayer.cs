using System.Collections.Generic;

namespace Kardamon.Core
{
    public interface IPlayer
    {
        AudioModel CurrentMedia { get; set; }
        IEnumerable<AudioModel> Queue { get; set; }

        void Init();
        void Load(IEnumerable<AudioModel> audios);
        void Play();
        void Pause();
        void Next();
        void Prev();
        void ClearQueue();
    }
}