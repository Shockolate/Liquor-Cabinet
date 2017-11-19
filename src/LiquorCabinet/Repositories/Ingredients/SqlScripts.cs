namespace LiquorCabinet.Repositories.Ingredients
{
    internal class SqlScripts
    {
        internal const string GetListIngredient = @"SELECT IngredientId AS Id, Name, Description FROM Glassware";
        internal const string InsertIngredient = @"INSERT INTO Ingredient (Name, Description) VALUES (@Name, @Description); SELECT SCOPE_IDENTITY();";
        internal const string GetIngredient = @"SELECT IngredientId AS Id, Name, Description FROM Ingredient WHERE IngredientId = @IngredientId";
        internal const string UpdateIngredient = @"UPDATE Ingredient SET Name = @Name, Description = @Description WHERE IngredientId = @IngredientId";
        internal const string DeleteIngredient = @"DELETE FROM Ingredient WHERE IngredientId = @IngredientId";
    }
}