
using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Product
{
    [TimestampedMigration(2015, 8, 27, 9, 52)]
    public class CreateProductTable : Migration
    {
        public override void Up() {
            Create.Table("Products")
                .WithColumn("Id").AsString(50).PrimaryKey().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Description").AsString(300).Nullable();
        }

        public override void Down()
        {
            Delete.Table("Products");
        }
    }
}
