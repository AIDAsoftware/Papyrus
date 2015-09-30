using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.VersionRange
{
    [TimestampedMigration(2015, 09, 30, 13, 38)]
    public class AddTopicIdColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("VersionRange").InSchema("dbo")
                .AddColumn("TopicId").AsString(50).NotNullable().SetExistingRowsTo("1");
        }

        public override void Down()
        {
            Delete.Column("TopicId").FromTable("VersionRange").InSchema("dbo");
        }
    }
}