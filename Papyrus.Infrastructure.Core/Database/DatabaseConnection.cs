
namespace Papyrus.Infrastructure.Core.Database {
    using Dapper;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    
    public class DatabaseConnection {

        private static readonly object lockObject = new object();
        private SqlConnection currentConnection;
        private SqlTransaction currenTransaction;
        private readonly string connectionString;
        
        public DatabaseConnection(string connectionString) {
            this.connectionString = connectionString;
        }

        private SqlConnection CurrentConnection {
            get { return currentConnection; }
            set {
                lock (lockObject) {
                    currentConnection = value;
                }
            }
        }

        private SqlTransaction CurrentTransaction {
            get { return currenTransaction; }
            set {
                lock (lockObject) {
                    currenTransaction = value;
                }
            }
        }


        public async Task<DataTable> Query(string query, SqlParameter[] sqlParameters = null) {
            if (IsOpen()) {
                var command = CreateCommand();
                return await ExecuteQueryInCommand(query, command, sqlParameters);
            }

            using (var oneUseConnection = GetNewConnection()) {
                var command = oneUseConnection.CreateCommand();
                return await ExecuteQueryInCommand(query, command, sqlParameters);
            }
        }

        public async Task<IEnumerable<T>> Query<T>(string sql, object param = null, int? commandTimeout = null) {
            if (IsOpen()) {
                return await CurrentConnection.QueryAsync<T>(sql: sql, param: param, transaction: GetTransaction(), commandTimeout: commandTimeout);
            }

            using (var oneUseConnection = GetNewConnection()) {
                return await oneUseConnection.QueryAsync<T>(sql: sql, param: param, commandTimeout: commandTimeout);
            }
        }


        private SqlCommand CreateCommand() {
            if (IsClosed()) {
                throw new DatabaseException("create command requires an open connection");
            }

            var command = CurrentConnection.CreateCommand();
            command.Transaction = GetTransaction();

            return command;

        }

        public async Task<int> Execute(string sql, object param = null) {

            if (IsOpen()) {
                var transaction = GetTransaction();
                return await CurrentConnection.ExecuteAsync(sql: sql, param: param, transaction: transaction);
            }

            using (var oneUseConnection = GetNewConnection()) {
                return await oneUseConnection.ExecuteAsync(sql: sql, param: param);
            }
        }


        public bool IsOpen() {
            return CurrentConnection != null;
        }

        public bool IsClosed() {
            return !IsOpen();
        }

        private SqlTransaction GetTransaction() {
            if (IsClosed()) { throw new DatabaseException("Connection is not established"); }
            return CurrentTransaction ?? (CurrentTransaction = CurrentConnection.BeginTransaction());
        }


        internal void EstablishConnection() {
            if (IsOpen()) return;
            CurrentConnection = GetNewConnection();
        }

        private SqlConnection GetNewConnection() {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        private static async Task<DataTable> ExecuteQueryInCommand(string query, SqlCommand command, SqlParameter[] sqlParameters = null) {
            command.CommandText = query;
            if (sqlParameters != null) command.Parameters.AddRange(sqlParameters);
            using (var dataReader = await command.ExecuteReaderAsync()) {
                var dataTable = new DataTable();
                dataTable.Load(dataReader);
                return dataTable;
            }
        }

        internal void Rollback() {
            if (CurrentTransaction == null) return;

            CurrentTransaction.Rollback();
            CurrentTransaction = null;
        }

        internal void Commit() {
            if (CurrentTransaction == null) return;

            CurrentTransaction.Commit();
            CurrentTransaction = null;
        }

        internal void Close() {
            if (IsClosed()) return;

            CurrentConnection.Close();
            CurrentConnection = null;
            CurrentTransaction = null;
        }

    }
}