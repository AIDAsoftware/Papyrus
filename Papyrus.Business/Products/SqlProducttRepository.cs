namespace Papyrus.Business.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Infrastructure.Core.Database;

    public class SqlProductRepository : ProductRepository
    {
        private readonly DatabaseConnection connection;

        public SqlProductRepository(DatabaseConnection connection) {
            this.connection = connection;
        }

        public async Task<Product> GetProduct(string id)
        {
            const string selectSqlQuery = @"SELECT Id, Name, Description
                                            FROM [Products] WHERE Id = @Id;";
            return (await connection.Query<Product>(selectSqlQuery, new { Id = id })).FirstOrDefault();
        }

        //TODO: devolver IEnumerable ??
        public async Task<List<Product>> GetAllProducts()
        {
            const string selectAllProductsSqlQuery = @"SELECT Id, Name, Description
                                                        FROM Products";
            return (await connection.Query<Product>(selectAllProductsSqlQuery)).ToList();
        }
    }
}