namespace Papyrus.Tests.Infrastructure.Database {
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Data;
    using Papyrus.Infrastructure.Core.Database;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class DatabaseConnectionShould {
        private DatabaseConnection connection;

        [SetUp]
        public void SetUp() {
            var connectionString = ConfigurationManager.ConnectionStrings["Papyrus"].ConnectionString;
            connection = new DatabaseConnection(connectionString);
        }

        [TearDown]
        public void TearDown() {
            
        }

        [Test]
        public async void execute_a_query_returning_raw_data_with_query_parameters() {
            await (new TransactionScopeSqlWithRollback(connection)).Execute(async() => {
                await connection.Execute("create table AnyTableName(column1 varchar(10))");
                await connection.Execute("insert into AnyTableName values ('anyValue')");
                await connection.Execute("insert into AnyTableName values ('otherValue')");

                var result = (await connection.Query("Select column1 from AnyTableName where column1 = @columValue", new[]
                {
                    new SqlParameter("columValue", SqlDbType.NVarChar) {Value = "otherValue"}
                })).AsEnumerable().First();

                result["column1"].Should().Be("otherValue");
            });
        }

        [Test]
        public async void execute_a_query_returning_raw_data_when_there_is_no_current_connection() {
            var result = (await connection.Query("Select 1 as firstColumn")).AsEnumerable().First();
            result["firstColumn"].Should().Be(1);
            connection.IsClosed().Should().BeTrue();
        }

        [Test]
        public async void execute_a_query_returning_raw_data_using_the_current_connection_if_there_is_one() {
            await (new TransactionScopeSqlWithRollback(connection)).Execute(async() => {
                await connection.Execute("create table AnyTableName(column1 varchar(10))");
                await connection.Execute("insert into AnyTableName values ('anyValue')");
                var result = (await connection.Query("Select column1 from AnyTableName")).AsEnumerable().First();
                result["column1"].Should().Be("anyValue");
                connection.IsOpen().Should().BeTrue();
            });
        }

        [Test]
        public async void execute_a_query_using_the_current_connection_if_there_is_one() {
            await (new TransactionScopeSqlWithRollback(connection)).Execute(async() => {
                var result = (await connection.Query<dynamic>("Select 1 as firstColumn")).First();
                Assert.AreEqual(1, result.firstColumn);
                connection.IsOpen().Should().BeTrue();
            });
        }

        [Test]
        public async void execute_a_query_using_a_new_connection_when_there_is_no_current_connection() {
            var result = (await connection.Query<dynamic>("Select 1 as firstColumn")).First();
            Assert.AreEqual(1, result.firstColumn);
            connection.IsClosed().Should().BeTrue();
        }

        [Test]
        public void two_threads_have_two_different_connection() {
            var waitForConnectionClosed = new ManualResetEvent(false);
            var waitForOtherToTakeConnection = new ManualResetEvent(false);

            var t1 = Task.Factory.StartNew(async() => {
                await (new TransactionScopeSqlWithRollback(connection)).Execute(() => 
                    Task.Factory.StartNew(() => { waitForOtherToTakeConnection.WaitOne(); })
                    );
                waitForConnectionClosed.Set();
            });

            var t2 = Task.Factory.StartNew(async() => {
                await (new TransactionScopeSqlWithRollback(connection)).Execute(async() => {
                    waitForOtherToTakeConnection.Set();
                    waitForConnectionClosed.WaitOne();
                    await connection.Execute("Select 1 as firstColumn");
                });
            });

            var hasCompletedWithoutException = true;
            try {
                Task.WaitAll(t1, t2);
            } catch (Exception ex) {
                hasCompletedWithoutException = false;
                Console.WriteLine(ex);
            }
            Assert.IsTrue(hasCompletedWithoutException);
        }
    }
}
