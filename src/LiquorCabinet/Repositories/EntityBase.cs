namespace LiquorCabinet.Repositories
{
    public abstract class EntityBase<TId>
    {
        public TId Id { get; set; }
    }
}