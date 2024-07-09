using CommonTestUtilities.Requests;
using FluentAssertions;
using RecipeBook.Application.UseCases.Recipe.Generate;
using RecipeBook.Domain.ValueObjects;
using RecipeBook.Exceptions;
using Xunit;

namespace Validators.Test.Recipe.Generate;
public class GenerateRecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new GenerateRecipeValidator();
        var request = RequestGenerateRecipeJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_More_Maximum_Ingredients()
    {
        var validator = new GenerateRecipeValidator();
        var request = RequestGenerateRecipeJsonBuilder
            .Build(count: RecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE + 1);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.INVALID_NUMBER_INGREDIENTS));
    }

    [Fact]
    public void Error_Duplicated_Ingredient()
    {
        var validator = new GenerateRecipeValidator();
        var request = RequestGenerateRecipeJsonBuilder
            .Build(count: RecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);
        request.Ingredients.Add(request.Ingredients[0]);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.DUPLICATED_INGREDIENTS_IN_LIST));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("      ")]
    [InlineData("")]
    public void Error_Empty_Ingredient(string ingredient)
    {
        var validator = new GenerateRecipeValidator();
        var request = RequestGenerateRecipeJsonBuilder.Build(count: 1);
        request.Ingredients.Add(ingredient);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.INGREDIENT_EMPTY));
    }

    [Fact]
    public void Error_Ingredient_Not_Following_Pattern()
    {
        var validator = new GenerateRecipeValidator();
        var request = RequestGenerateRecipeJsonBuilder.Build(count: RecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);

        request.Ingredients.Add("This is an invalid ingredient because is too long");

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.INGREDIENT_NOT_FOLLOWING_PATTERN));
    }
}
