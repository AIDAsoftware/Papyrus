using System.Collections.Specialized;
using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Product
{
    [TimestampedMigration(2015, 09, 22, 15, 41)]
    public class SeparateProductVersionTable : Migration
    {
        private string schemaName = "dbo";
        private string ProductVersionTableName = "ProductVersion";

        public override void Up()
        {
            Delete.PrimaryKey("ProductsPK").FromTable(ProductVersionTableName).InSchema(schemaName);
            Create.PrimaryKey("ProductsPK").OnTable(ProductVersionTableName).WithSchema(schemaName)
                .Columns("ProductId");
            Delete.Column("VersionId").FromTable(ProductVersionTableName).InSchema(schemaName);
            Delete.Column("VersionName").FromTable(ProductVersionTableName).InSchema(schemaName);
            Rename.Table(ProductVersionTableName).InSchema(schemaName).To("Product");

            Create.Table(ProductVersionTableName).InSchema(schemaName)
                .WithColumn("VersionId").AsString(50).PrimaryKey()
                .WithColumn("VersionName").AsString(50).NotNullable()
                .WithColumn("Product").AsString(50).NotNullable();
        }

        public override void Down()
        {
            Delete.Table(ProductVersionTableName).InSchema(schemaName);

            Alter.Table("Product").InSchema(schemaName)
                .AddColumn("VersionId").AsString(50).NotNullable()
                .AddColumn("VersionName").AsString(50).NotNullable();

            Delete.PrimaryKey("ProductsPK").FromTable("Product").InSchema(schemaName);
            Create.PrimaryKey("ProductsPK").OnTable("Product").WithSchema(schemaName)
                .Columns("ProductId", "VersionId");
            Rename.Table("Product").InSchema(schemaName).To(ProductVersionTableName);
        }
    }
}