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

        public async Task Save(Product product)
        {
            const string insertSqlQuery = @"INSERT Products(Id, Name, Description) 
                                            VALUES (@Id, @Name, @Description);";
            await connection.Execute(insertSqlQuery, new {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description
            });
        }

        public async Task<Product> GetProduct(string id)
        {
            const string selectSqlQuery = @"SELECT Id, Name, Description
                                            FROM [Products] WHERE Id = @Id;";
            return (await connection.Query<Product>(selectSqlQuery, new { Id = id })).FirstOrDefault();
        }

        public async Task Update(Product document)
        {
            const string updateSqlQuery = @"UPDATE Products " + 
                                            "SET Name = @Name, " + 
                                            "Description = @Description " + 
                                            "WHERE Id = @Id;";
            var affectedRows = await connection.Execute(updateSqlQuery, new
            {
                Id = document.Id,
                Name = document.Name,
                Description = document.Description
            });
        }

        public async Task Delete(string documentId)
        {
            const string deleteSqlQuery = @"DELETE FROM Products WHERE Id = @Id";
            var affectedRows = await connection.Execute(deleteSqlQuery, new
            {
                Id = documentId,
            });
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