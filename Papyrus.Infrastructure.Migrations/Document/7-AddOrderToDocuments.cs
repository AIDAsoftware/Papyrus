using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Document {
    [TimestampedMigration(2016, 07, 23, 17, 56)]
    public class AddOrderToDocuments : Migration {
        public override void Up() {
            Alter.Table("Document").InSchema("dbo").AddColumn("Order").AsInt32().NotNullable().SetExistingRowsTo(int.MaxValue);
        }

        public override void Down() {
            Delete.Column("Order").FromTable("Document").InSchema("dbo");
        }
    }
}