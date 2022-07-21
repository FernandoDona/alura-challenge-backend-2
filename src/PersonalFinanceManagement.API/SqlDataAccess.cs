using Dapper;
using PersonalFinanceManagement.API.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace PersonalFinanceManagement.API;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<int> ExecuteAsync<T>(string storedProcedure, T parameters, string connectionStringKey = "PFMDB")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionStringKey));
        return await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<T>> QueryAsync<T, U>(string storedProcedure, U parameters, string connectionStringKey = "PFMDB")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionStringKey));
        return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<T> QueryFirstOrDefaultAsync<T, U>(string storedProcedure, U parameters, string connectionStringKey = "PFMDB")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionStringKey));
        return await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}
