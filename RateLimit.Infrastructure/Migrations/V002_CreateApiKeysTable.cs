using FluentMigrator;

namespace RateLimit.Infrastructure.Migrations;

[Migration(2)]
public class V002_CreateApiKeysTable : Migration
{
    private const string TableName = "TB_API_KEYS";
    private const string UsersTable = "TB_USERS";
    private const string ForeignKeyName = "FK_API_KEYS_USERS";
    
    public override void Up()
    {
        Create.Table(TableName)
            .WithColumn("ID").AsInt32().PrimaryKey().Identity()
            .WithColumn("NAME").AsString(64).NotNullable()
            .WithColumn("KEY").AsString(128).NotNullable()
            .WithColumn("IS_ACTIVE").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CREATED_AT").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("USER_ID").AsInt32().NotNullable();

        Create.ForeignKey(ForeignKeyName)
            .FromTable(TableName).ForeignColumn("USER_ID")
            .ToTable(UsersTable).PrimaryColumn("ID");
    }

    public override void Down()
    {
        Delete.ForeignKey(ForeignKeyName).OnTable(TableName);
        Delete.Table(TableName);
    }
}