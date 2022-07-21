namespace PersonalFinanceManagement.API.Interfaces;

public interface ISqlDataAccess
{
    Task<int> ExecuteAsync<T>(string storedProcedure, T parameters, string connectionStringKey = "PFMDB");
    Task<T> QueryFirstOrDefaultAsync<T, U>(string storedProcedure, U parameters, string connectionStringKey = "PFMDB");
    Task<IEnumerable<T>> QueryAsync<T, U>(string storedProcedure, U parameters, string connectionStringKey = "PFMDB");
}
