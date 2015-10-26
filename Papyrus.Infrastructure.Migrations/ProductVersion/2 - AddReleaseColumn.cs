using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.ProductVersion
{
    [TimestampedMigration(2015, 09, 30, 12, 53)]
    public class AddReleaseColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("ProductVersion").InSchema("dbo")
                .AddColumn("Release").AsDate().NotNullable().SetExistingRowsTo("20150930");
        }

        public override void Down()
        {
            Delete.Column("Release").FromTable("ProductVersion").InSchema("dbo");
        }
    }
}