using FluentMigrator;
using RecipeBook.Domain.Entities;

namespace RecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.IMAGES_FOR_RECIPES, "Add collumn on recipe table to save images")]
public class Version0000003 : VersionBase
{
    public override void Up()
    {
        Alter.Table("Recipes").AddColumn(nameof(Recipe.ImageIdentifier)).AsString().Nullable();
    }
}
