namespace LiquorCabinet.Repositories.Glasses
{
    internal static class SqlScripts
    {
        internal const string GetListGlassware = @"
SELECT 
    GlasswareId AS Id,
    Name,
    Description,
    TypicalSize FROM Glassware
";

        internal const string InsertGlassware = @"
INSERT INTO Glassware
    (Name, Description, TypicalSize)
VALUES
    (@Name, @Description, @TypicalSize);

SELECT SCOPE_IDENTITY();";
    }
}