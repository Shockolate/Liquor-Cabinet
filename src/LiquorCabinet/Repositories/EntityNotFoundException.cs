using System;

namespace LiquorCabinet.Repositories
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string messsage) : base(messsage) { }

        public EntityNotFoundException(string type, int id) : this($"{type}:{id} Not Found") { }
    }
}