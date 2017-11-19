using System;

namespace LiquorCabinet.Repositories
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string messsage) : base(messsage) { }
    }
}
