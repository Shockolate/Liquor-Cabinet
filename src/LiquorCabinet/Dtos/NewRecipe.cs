using System.Collections.Generic;
using LiquorCabinet.Models;

namespace LiquorCabinet.Dtos
{
    public class NewRecipe
    {
        public string Name { get; set; }
        public string Instructions { get; set; }
        public int GlasswareId { get; set; }
        public IEnumerable<RecipeComponent> Components { get; set; }
    }
}