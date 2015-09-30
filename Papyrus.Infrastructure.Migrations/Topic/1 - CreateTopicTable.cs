using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Topic
{
    [TimestampedMigration(2015, 09, 30, 10, 49)]
    public class CreateTopicTable : Migration
    {
        public override void Up()
        {
            Create.Table("Topic").InSchema("dbo")
                .WithColumn("TopicId").AsString(50).PrimaryKey()
                .WithColumn("ProductId").AsString(50).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Topic").InSchema("dbo");
        }
    }
}