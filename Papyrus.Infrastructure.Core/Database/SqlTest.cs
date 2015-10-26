using System;

namespace Papyrus.Infrastructure.Core.Database {
    using System.Configuration;
    using NUnit.Framework;

    public abstract class SqlTest {
        protected DatabaseConnection dbConnection;
        [SetUp]
        public void SetUp() {
            var connectionString = ConfigurationManager.ConnectionStrings["Papyrus"].ConnectionString;
            dbConnection = new DatabaseConnection(connectionString);
            dbConnection.EstablishConnection();
        }

        [TearDown]
        public void teardown() {
            dbConnection.Rollback();
            dbConnection.Close();
        }
    }
}