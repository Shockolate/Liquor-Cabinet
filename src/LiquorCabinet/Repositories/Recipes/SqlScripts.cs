namespace LiquorCabinet.Repositories.Recipes
{
    internal static class SqlScripts
    {
        internal const string GetAllRecipes = @"
SELECT
    r.RecipeId,
    r.Name AS RecipeName,
    r.Instructions AS RecipeInstructions,
    r.GlasswareId,
    g.Name AS GlasswareName,
    g.Description AS GlassswareDescription,
    g.TypicalSize AS GlasswareTypicalSize,
    rcq.RecipeComponentQuantityId,
    c.ComponentId,
    COALESCE(i.Name, rc.Name) as ComponentName,
    rcq.QuantityPart AS ComponentQuantityPart,
    rcq.QuantityMetric AS ComponentQuantityMetric,
    rcq.QuantityImperial AS ComponentQuantityImperial
FROM
    Recipe r 
JOIN
    Glassware g 
        ON r.GlasswareId = g.GlasswareId 
JOIN
    Recipe_Component_Quantity rcq 
        ON rcq.RecipeId = r.RecipeId 
JOIN
    Component c  
        ON rcq.ComponentId = c.ComponentId 
LEFT JOIN
    Ingredient i 
        ON c.IngredientId = i.IngredientId 
LEFT JOIN
    Recipe rc 
        ON c.RecipeId = rc.RecipeId;";

        internal const string GetRecipe = @"
SELECT
    r.RecipeId,
    r.Name AS RecipeName,
    r.Instructions AS RecipeInstructions,
    r.GlasswareId,
    g.Name AS GlasswareName,
    g.Description AS GlasswareDescription,
    g.TypicalSize AS GlasswareTypicalSize,
    rcq.RecipeComponentQuantityId,
    c.ComponentId,
    COALESCE(i.Name, rc.Name) as ComponentName,
    rcq.QuantityPart AS ComponentQuantityPart,
    rcq.QuantityMetric AS ComponentQuantityMetric,
    rcq.QuantityImperial AS ComponentQuantityImperial
FROM
    Recipe r
JOIN
    Glassware g
        ON r.GlasswareId = g.GlasswareId,
JOIN
    Recipe_Component_Quantity rcq
        ON rcq.RecipeId = r.RecipeId
JOIN
    Component c
        ON rcq.ComponentId = c.ComponentId
LEFT JOIN
    Ingredient i
        ON c.IngredientId = i.IngredientId
LEFT JOIN
    Recipe rc
        ON c.RecipeId = rc.RecipeId
WHERE
    r.RecipeId = @RecipeId";

        internal const string InsertRecipe = @"
INSERT INTO Recipe
    (Name, GlasswareId, Instructions)
VALUES
    (@Name, @GlasswareId, @Instructions);
SELECT SCOPE_IDENTITY();
";

        internal const string InsertRecipeComponent = @"
INSERT INTO Recipe_Component_Quantity
    (RecipeId, ComponentId, QuantityPart, QuantityMetric, QuantityImperial)
VALUES
    (@RecipeId, @ComponentId, @QuantityPart, @QuantityMetric, @QuantityImperial);
";

        internal const string UpdateRecipe = @"
UPDATE
    Recipe
SET
    Name = @Name,
    Instructions = @Instructions,
    GlasswareId = @GlasswareId         
WHERE
    RecipeId = @RecipeId
";
    }
}