using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Document
{
    [TimestampedMigration(2015, 09, 23, 11, 46)]
    public class RenameIdColumnToTopicId : Migration
    {
        public override void Up()
        {
            Rename.Column("Id").OnTable("Documents").InSchema("dbo").To("TopicId");
        }

        public override void Down()
        {
            Rename.Column("TopicId").OnTable("Documents").InSchema("dbo").To("Id");
        }
    }
}