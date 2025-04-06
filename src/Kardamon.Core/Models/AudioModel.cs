namespace Kardamon.Core
{
    public class AudioModel : ModelBase
    {
        public string Title { get; set; } = null!;
        public string Artist { get; set; } = null!;
        public string Source { get; set; } = null!;
        public string Time { get; set; }
        public string Image { get; set; }

        public AudioModel(int id, string name)
            : base(id, name)
        {

        }
    }
}