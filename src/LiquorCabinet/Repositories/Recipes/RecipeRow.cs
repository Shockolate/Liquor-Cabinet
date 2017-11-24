namespace LiquorCabinet.Repositories.Recipes
{
    public class RecipeRow
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string RecipeInstructions { get; set; }
        public int GlasswareId { get; set; }
        public string GlasswareName { get; set; }
        public string GlasswareDescription { get; set; }
        public string GlasswareTypicalSize { get; set; }
        public int RecipeComponentQuantityId { get; set; }
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentQuantityPart { get; set; }
        public double? ComponentQuantityMetric { get; set; }
        public double? ComponentQuantityImperial { get; set; }
    }
}