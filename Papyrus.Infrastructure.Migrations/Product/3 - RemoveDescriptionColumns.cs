using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Product
{
    [TimestampedMigration(2015, 09, 22, 12, 36)]
    public class RemoveDescriptionColumns : Migration
    {
        public override void Up()
        {
            Delete.Column("Description").FromTable("ProductVersion").InSchema("dbo");
        }

        public override void Down()
        {
            Alter.Table("ProductVersion").InSchema("dbo").AddColumn("Description").AsString(300).Nullable();
        }
    }
}