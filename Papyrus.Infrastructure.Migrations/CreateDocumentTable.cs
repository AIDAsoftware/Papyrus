using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papyrus.Infrastructure.Migrations
{
    using FluentMigrator;

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
