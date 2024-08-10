using FluentMigrator;

namespace RecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_REFRESH_TOKEN, "Add collumn on recipe table to save images")]
public class Version0000004 : VersionBase
{
    public override void Up()
    {
        CreateTable("RefreshTokens")
            .WithColumn("Value").AsString().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_RefreshTokens_User_Id", "Users", "Id");
    }
}
