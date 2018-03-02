using System.Data;

namespace LiquorCabinet.Repositories
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateLiquorDbConnection();
    }
}
