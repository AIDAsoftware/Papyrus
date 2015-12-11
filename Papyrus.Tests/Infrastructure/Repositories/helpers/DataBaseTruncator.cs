using System.Threading.Tasks;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Tests.Infrastructure.Repositories.Helpers
{
    public class DataBaseTruncator
    {
        private readonly DatabaseConnection dbConnection;

        public DataBaseTruncator(DatabaseConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task TruncateDataBase()
        {
            await dbConnection.Execute("TRUNCATE TABLE Topic;");
            await dbConnection.Execute("TRUNCATE TABLE Product;");
            await dbConnection.Execute("TRUNCATE TABLE ProductVersion;");
            await dbConnection.Execute("TRUNCATE TABLE VersionRange;");
            await dbConnection.Execute("TRUNCATE TABLE Document;");
        }
    }
}