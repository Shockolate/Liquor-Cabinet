﻿namespace LiquorCabinet.Repositories.Entities
{
    internal class Glass : EntityBase<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TypicalSize { get; set; }
    }
}
