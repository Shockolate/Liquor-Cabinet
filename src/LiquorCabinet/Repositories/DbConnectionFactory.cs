using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LiquorCabinet.Repositories
{
    internal class DbConnectionFactory : IDbConnectionFactory
    {
        private static string _liquorDatabaseConnectionString = string.Empty;

        internal DbConnectionFactory(IConfiguration configuration)
        {
            _liquorDatabaseConnectionString = configuration.GetConnectionString("LiquorDB");
        }

        public IDbConnection CreateLiquorDbConnection()
        {
            if (string.IsNullOrEmpty(_liquorDatabaseConnectionString))
            {
                throw new ApplicationException("Connection string cannot be empty!");
            }
            return new SqlConnection(_liquorDatabaseConnectionString);
        }
    }
}