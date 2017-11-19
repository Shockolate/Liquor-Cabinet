namespace LiquorCabinet.Repositories.Entities
{
    internal class Ingredient : EntityBase<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}