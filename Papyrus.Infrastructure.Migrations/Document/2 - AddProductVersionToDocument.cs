using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Document
{
    [TimestampedMigration(2015, 8, 27, 19, 04)]
    public class AddProductVerstionToDocument : Migration
    {
        public override void Up() {
            Alter.Table("Documents").AddColumn("ProductVersionId").AsString(50).NotNullable().PrimaryKey("PK_Documents");
            Alter.Table("Documents").AlterColumn("Language").AsString(15).NotNullable();

            Delete.PrimaryKey("PK_Documents").FromTable("Documents");
            Create.PrimaryKey("DocumentsPK").OnTable("Documents").WithSchema("dbo")
                .Columns("Id", "ProductVersionId", "Language");
        }

        public override void Down() {
            Delete.Column("ProductVersionId").FromTable("Documents").InSchema("dbo");

            Delete.PrimaryKey("DocumentsPK").FromTable("Documents");
            Alter.Table("Documents").AlterColumn("Id").AsString(50).PrimaryKey().NotNullable();
            Alter.Table("Documents").AlterColumn("Language").AsString(15);
        }
    }
}
