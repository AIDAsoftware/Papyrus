using FluentMigrator;

namespace Papyrus.Infrastructure.Migrations.Document
{
    [TimestampedMigration(2015, 09, 30, 11, 55)]
    public class AdaptDocumentToBeContainedByVersionRange : Migration
    {
        private string oldTableName = "Documents";
        private string schemaName = "dbo";
        private string newTableName = "Document";

        public override void Up()
        {
            Rename.Column("TopicId").OnTable(oldTableName).InSchema(schemaName).To("DocumentId");
            Rename.Column("ProductId").OnTable(oldTableName).InSchema(schemaName).To("VersionRangeId");

            Delete.PrimaryKey("DocumentsPK").FromTable(oldTableName).InSchema(schemaName);
            Create.PrimaryKey("DocumentsPK").OnTable(oldTableName).WithSchema(schemaName)
                .Columns("DocumentId");
            Delete.Column("ProductVersionId").FromTable(oldTableName).InSchema(schemaName);

            Rename.Table(oldTableName).InSchema(schemaName).To(newTableName);
        }

        public override void Down()
        {
            Rename.Column("DocumentId").OnTable(newTableName).InSchema(schemaName).To("TopicId");
            Rename.Column("VersionRangeId").OnTable(newTableName).InSchema(schemaName).To("ProductId");
            Alter.Table(newTableName).InSchema(schemaName)
                .AddColumn("ProductVersionId").AsString(50).NotNullable().SetExistingRowsTo(1);

            Delete.PrimaryKey("DocumentsPK").FromTable(newTableName).InSchema(schemaName);
            Create.PrimaryKey("DocumentsPK").OnTable(newTableName).WithSchema(schemaName)
                .Columns("TopicId", "Language", "ProductVersionId", "ProductId");

            Rename.Table(newTableName).InSchema(schemaName).To(oldTableName);
        }
    }
}