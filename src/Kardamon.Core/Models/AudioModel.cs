namespace Kardamon.Core
{
    public class AudioModel : ModelBase
    {
        public string Title { get; set; } = null!;
        public string Artist { get; set; } = null!;
        public string Source { get; set; } = null!;

        public AudioModel(int id, string name)
            : base(id, name)
        {

        }
    }
}