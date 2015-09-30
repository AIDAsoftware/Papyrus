using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Version
{
    [TimestampedMigration(2015, 09, 30, 11, 11)]
    public class RenameProductColToProductId : Migration
    {
        public override void Up()
        {
            Rename.Column("Product").OnTable("ProductVersion").InSchema("dbo").To("ProductId");
        }

        public override void Down()
        {
            Rename.Column("ProductId").OnTable("ProductVersion").InSchema("dbo").To("Product");
        }
    }
}