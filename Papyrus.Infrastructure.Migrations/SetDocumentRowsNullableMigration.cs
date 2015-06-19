namespace Papyrus.Infrastructure.Migrations
{
    using FluentMigrator;

    [TimestampedMigration(2015, 6, 19, 15, 24)]
    public class SetDocumentRowsNullableMigration : Migration
    {
        public override void Up()
        {
            Alter.Table("Documents")
                .AlterColumn("Title").AsString(50).Nullable()
                .AlterColumn("Description").AsString(300).Nullable()
                .AlterColumn("Content").AsString(3000).Nullable()
                .AlterColumn("Language").AsString(15).Nullable();
        }

        public override void Down()
        {
            Alter.Table("Documents")
                .AlterColumn("Title").AsString(50).NotNullable()
                .AlterColumn("Description").AsString(300)
                .AlterColumn("Content").AsString(3000)
                .AlterColumn("Language").AsString(15);
        }
    }
}