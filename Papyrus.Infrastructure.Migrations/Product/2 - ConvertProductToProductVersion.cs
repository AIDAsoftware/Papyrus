using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Product {
    [TimestampedMigration(2015, 09, 21, 23, 46)]
    public class ConvertProductToProductVersion : Migration {

        public override void Up() {
            var productVersionTableName = "ProductVersion";
            var schemaName = "dbo";
            Rename.Table("Products").InSchema(schemaName).To(productVersionTableName);
            Rename.Column("Name").OnTable(productVersionTableName).InSchema(schemaName).To("ProductName");
            Rename.Column("Id").OnTable(productVersionTableName).InSchema(schemaName).To("ProductId");
            Alter.Table(productVersionTableName).InSchema(schemaName)
                .AddColumn("VersionId")
                .AsString(15)
                .NotNullable().SetExistingRowsTo("1");
            Alter.Table(productVersionTableName).InSchema(schemaName)
                .AddColumn("VersionName")
                .AsString(50)
                .NotNullable().SetExistingRowsTo("1");

            Delete.PrimaryKey("PK_Products").FromTable(productVersionTableName).InSchema(schemaName);
            Create.PrimaryKey("ProductsPK").OnTable(productVersionTableName).WithSchema(schemaName)
                .Columns("ProductId", "VersionId");
        }

        public override void Down() {
            var productVersionTableName = "ProductVersion";
            var schemaName = "dbo";

            Delete.PrimaryKey("ProductsPK").FromTable(productVersionTableName).InSchema(schemaName);

            Rename.Column("ProductName").OnTable(productVersionTableName).InSchema(schemaName).To("Name");
            Rename.Column("ProductId").OnTable(productVersionTableName).InSchema(schemaName).To("Id");
            Create.PrimaryKey("PK_Products").OnTable(productVersionTableName).WithSchema(schemaName)
                .Columns("Id");

            Delete.Column("VersionId").FromTable(productVersionTableName).InSchema(schemaName);
            Delete.Column("VersionName").FromTable(productVersionTableName).InSchema(schemaName);
            Rename.Table(productVersionTableName).InSchema(schemaName).To("Products");
        }
    }
}