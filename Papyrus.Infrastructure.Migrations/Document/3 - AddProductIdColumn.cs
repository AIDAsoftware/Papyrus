using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Document
{
    [TimestampedMigration(2015, 09, 22, 21, 30)]
    public class AddProductIdColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("Documents").InSchema("dbo").AddColumn("ProductId").AsString(50).NotNullable().SetExistingRowsTo("0");

            Delete.PrimaryKey("DocumentsPK").FromTable("Documents").InSchema("dbo");
            Create.PrimaryKey("DocumentsPK").OnTable("Documents").WithSchema("dbo")
                .Columns("Id", "ProductId", "ProductVersionId", "Language");
        }

        public override void Down()
        {
            Delete.PrimaryKey("DocumentsPK").FromTable("Documents").InSchema("dbo");
            Create.PrimaryKey("DocumentsPK").OnTable("Documents").WithSchema("dbo")
                .Columns("Id", "ProductVersionId", "Language");

            Delete.Column("ProductId").FromTable("Documents").InSchema("dbo");
        }
    }
}