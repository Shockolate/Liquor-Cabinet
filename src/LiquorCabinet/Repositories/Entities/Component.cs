namespace LiquorCabinet.Repositories.Entities
{
    internal abstract class Component : EntityBase<int>
    {
        public string Name { get; set; }
    }
}