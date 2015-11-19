using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Document {
    [TimestampedMigration(2015, 11, 19, 9, 23)]
    public class IncreaseTheContentSize : Migration {
        public override void Up() {
            Alter.Column("Content").OnTable("Document").InSchema("dbo").AsString(int.MaxValue).Nullable();
        }

        public override void Down() {
            Alter.Column("Content").OnTable("Document").InSchema("dbo").AsString(3000).Nullable();
        }
    }
}