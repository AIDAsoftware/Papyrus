using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Document
{
    [TimestampedMigration(2015, 6, 19, 11, 04)]
    public class CreateDocumentTable : Migration
    {
        public override void Up()
        {
            Create.Table("Documents")
                .WithColumn("Id")
                .AsString(50)
                .PrimaryKey().NotNullable()
                .WithColumn("Title")
                .AsString()
                .NotNullable()
                .WithColumn("Description")
                .AsString(300)
                .WithColumn("Content")
                .AsString(3000)
                .WithColumn("Language")
                .AsString(15);
        }

        public override void Down()
        {
            Delete.Table("Documents");
        }
    }
}
