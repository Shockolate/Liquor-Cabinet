using LiquorCabinet.Repositories;

namespace LiquorCabinet.Models
{
    public abstract class Component : EntityBase<int>
    {
        public string Name { get; set; }
    }
}