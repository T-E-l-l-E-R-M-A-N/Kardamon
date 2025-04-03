namespace Kardamon.Core
{
    public class CollectionModel : ModelBase
    {
        public int ArtistId { get; set; }
        public CollectionType CollectionType { get; }
        public CollectionModel(CollectionType collectionType, int id, string name)
            : base(id, name)
        {
            CollectionType = collectionType;
        }
    }
}