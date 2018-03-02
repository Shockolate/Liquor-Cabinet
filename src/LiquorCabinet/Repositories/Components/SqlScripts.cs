namespace LiquorCabinet.Repositories.Components
{
    internal class SqlScripts
    {
        internal const string UpdateRecipeComponent = @"
UPDATE
    Recipe_Component_Quantity 
SET
    QuantityPart = @QuantityPart,
    QuantityMetric = @QuantityMetric,
    QuantityImperial = @QuantityImperial 
WHERE
    RecipeComponentQuantityId = @RecipeComponentQuantityId
AND
    RecipeId = @RecipeId
AND
    ComponentId = @ComponentId
";

        internal const string GetRecipeComponent = @"
SELECT
    RecipeComponentQuantityId AS Id,
    RecipeId,
    ComponentId,
    QuantityPart,
    QuantityMetric,
    QuantityImperial
FROM
    Recipe_Component_Quantity
WHERE
    RecipeComponentQuantityId = @RecipeComponentQuantityId
";
    }
}