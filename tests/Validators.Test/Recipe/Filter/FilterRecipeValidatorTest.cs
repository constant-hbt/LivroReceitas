using CommonTestUtilities.Requests;
using FluentAssertions;
using RecipeBook.Application.UseCases.Recipe.Filter;
using Xunit;

namespace Validators.Test.Recipe.Filter;
public class FilterRecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();

        var response = validator.Validate(request);

        response.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Invalid_Cooking_Time()
    {
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((RecipeBook.Communication.Enums.CookingTime)1000);

        var response = validator.Validate(request);

        response.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Error_Invalid_Difficulty()
    {
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.Difficulties.Add((RecipeBook.Communication.Enums.Difficulty)1000);

        var response = validator.Validate(request);

        response.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Error_Invalid_DishTypes()
    {
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.DishTypes.Add((RecipeBook.Communication.Enums.DishType)1000);

        var response = validator.Validate(request);

        response.IsValid.Should().BeFalse();
    }
}
