using System.Data;
using System.Data.SqlClient;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.Repositories
{
    internal class ConnectionFactory
    {
        private static string _liquorDatabaseConnectionString = string.Empty;

        internal static IDbConnection CreateLiquorDbConnection(ILogger logger)
        {
            if (string.IsNullOrEmpty(_liquorDatabaseConnectionString))
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder
                {
                    IntegratedSecurity = false,
                    DataSource = Settings.Settings.Instance.Database.Server,
                    InitialCatalog = Settings.Settings.Instance.Database.Database,
                    UserID = Settings.Settings.Instance.Database.UserId,
                    Password = Settings.Settings.Instance.Database.Password
                };
                _liquorDatabaseConnectionString = connectionStringBuilder.ConnectionString;
            }
            logger.LogDebug(() => _liquorDatabaseConnectionString);
            return new SqlConnection(_liquorDatabaseConnectionString);
        }
    }
}