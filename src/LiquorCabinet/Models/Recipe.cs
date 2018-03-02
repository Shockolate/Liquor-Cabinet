using System.Collections.Generic;

namespace LiquorCabinet.Models
{
    public class Recipe : Component
    {
        public string Instructions { get; set; }
        public Glass Glassware { get; set; }
        public IList<RecipeComponent> Components { get; set; }
    }
}