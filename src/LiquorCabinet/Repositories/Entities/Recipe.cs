﻿using System.Collections.Generic;

namespace LiquorCabinet.Repositories.Entities
{
    internal class Recipe : Component
    {
        public string Instructions { get; set; }
        public Glass Glassware { get; set; }
        public IList<RecipeComponent> Components { get; set; }
    }
}