using FluentMigrator;

namespace RateLimit.Infrastructure.Migrations;

[Migration(1)]
public class V001_CreateUsersTable : Migration
{
    private const string TableName = "TB_USERS";
    
    public override void Up()
    {
        Create.Table(TableName)
            .WithColumn("ID").AsInt32().PrimaryKey().Identity()
            .WithColumn("EMAIL").AsString(100).NotNullable()
            .WithColumn("PASSWORD").AsString(60).NotNullable()
            .WithColumn("PLAN").AsInt32().NotNullable().WithDefaultValue(0);
    }

    public override void Down()
    {
        Delete.Table(TableName);
    }
}