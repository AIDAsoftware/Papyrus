using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.VersionRange
{
    [TimestampedMigration(2015, 09, 30, 11, 01)]
    public class CreateVersionRangeTable : Migration
    {
        public override void Up()
        {
            Create.Table("VersionRange").InSchema("dbo")
                .WithColumn("VersionRangeId").AsString(50).PrimaryKey()
                .WithColumn("FromVersionId").AsString(50).NotNullable()
                .WithColumn("ToVersionId").AsString(50).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("VersionRange").InSchema("dbo");
        }
    }
}