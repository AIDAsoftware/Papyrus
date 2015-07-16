namespace Papyrus.Infrastructure.Core.Database {
    using System;
    using System.Threading.Tasks;

    public interface TransactionScope {
        Task Execute(Func<Task> action);
        Task<T> Execute<T>(Func<Task<T>> action);
    }

    public class TransactionScopeSql : TransactionScope {
        private readonly DatabaseConnection connection;

        public TransactionScopeSql(DatabaseConnection connection) {
            this.connection = connection;
        }

        public virtual async Task Execute(Func<Task> action) {
            try {
                connection.EstablishConnection();
                await action();
                connection.Commit();
            } catch (Exception) {
                connection.Rollback();
                throw;
            } finally {
                connection.Close();
            }
        }

        public virtual async Task<T> Execute<T>(Func<Task<T>> action) {
            try {
                connection.EstablishConnection();
                var result = await action();
                connection.Commit();

                return result;
            } catch (Exception) {
                connection.Rollback();
                throw;
            } finally {
                connection.Close();
            }
        }
    }

    public class TransactionScopeDummy : TransactionScope {

        public async Task Execute(Func<Task> action) {
            await action();
        }

        public async Task<T> Execute<T>(Func<Task<T>> action) {
            return await action();
        }
    }

    public class TransactionScopeSqlWithRollback : TransactionScope {
        private readonly DatabaseConnection connection;

        public TransactionScopeSqlWithRollback(DatabaseConnection connection) {
            this.connection = connection;
        }

        public virtual async Task Execute(Func<Task> action) {
            connection.EstablishConnection();
            await action();
            connection.Rollback();
            connection.Close();
        }

        public virtual async Task<T> Execute<T>(Func<Task<T>> action) {
            connection.EstablishConnection();
            var result = await action();
            connection.Commit();
            return result;
        }
    }


}