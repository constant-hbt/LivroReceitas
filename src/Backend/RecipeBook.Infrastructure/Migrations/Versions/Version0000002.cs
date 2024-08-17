using FluentMigrator;

namespace RecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_RECIPES, "Create table to save the recipes' information")]
public class Version0000002 : VersionBase
{
    private const string RECIPE_TABLE_NAME = "Recipes";

    public override void Up()
    {
        CreateTable(RECIPE_TABLE_NAME)
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Recipe_User_Id", "Users", "Id")
            .WithColumn("Title").AsString(255).NotNullable()
            .WithColumn("CookingTime").AsInt32().Nullable()
            .WithColumn("Difficulty").AsInt32().Nullable();

        CreateTable("Ingredients")
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Ingredient_Recipe_Id", RECIPE_TABLE_NAME, "Id").OnDelete(System.Data.Rule.Cascade)
            .WithColumn("Item").AsString().NotNullable();

        CreateTable("Instructions")
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Instruction_Recipe_Id", RECIPE_TABLE_NAME, "Id").OnDelete(System.Data.Rule.Cascade)
            .WithColumn("Step").AsInt32().NotNullable()
            .WithColumn("Text").AsString(2000).NotNullable();

        CreateTable("DishTypes")
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_DishType_Recipe_Id", RECIPE_TABLE_NAME, "Id").OnDelete(System.Data.Rule.Cascade)
            .WithColumn("Type").AsInt32().NotNullable();
    }
}
