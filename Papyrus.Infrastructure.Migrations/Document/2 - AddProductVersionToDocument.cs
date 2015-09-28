using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Document
{
    [TimestampedMigration(2015, 8, 27, 19, 04)]
    public class AddProductVerstionToDocument : Migration
    {
        public override void Up() {
            Alter.Table("Documents").AddColumn("ProductVersionId").AsString(50).NotNullable().SetExistingRowsTo("1");
            Alter.Table("Documents").AlterColumn("Language").AsString(15).NotNullable();

            Delete.PrimaryKey("PK_Documents").FromTable("Documents");
            Create.PrimaryKey("DocumentsPK").OnTable("Documents").WithSchema("dbo")
                .Columns("Id", "ProductVersionId", "Language");
        }

        public override void Down() {
            Delete.PrimaryKey("DocumentsPK").FromTable("Documents");
            Delete.Column("ProductVersionId").FromTable("Documents").InSchema("dbo");

            Alter.Table("Documents").AlterColumn("Language").AsString(15).Nullable();
            Create.PrimaryKey("PK_Documents").OnTable("Documents").WithSchema("dbo").Columns("Id");
        }
    }
}
