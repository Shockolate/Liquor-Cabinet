using System;
using System.Collections.Generic;
using System.Text;
using LiquorCabinet.Repositories.Entities;

namespace LiquorCabinet.PathHandlers.v1.recipes
{
    internal class NewRecipe
    {
        public string Name { get; set; }
        public string Instructions { get; set; }
        public int GlasswareId { get; set; }
        public IEnumerable<RecipeComponent> Components { get; set; }
    }
}
