using CommonTestUtilities.Dtos;
using CommonTestUtilities.OpenAI;
using CommonTestUtilities.Requests;
using FluentAssertions;
using RecipeBook.Application.UseCases.Recipe.Generate;
using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.ValueObjects;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Xunit;

namespace UseCases.Test.Recipe.Generate;
public class GenerateRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var dto = GeneratedRecipeDtoBuilder.Build();
        var request = RequestGenerateRecipeJsonBuilder.Build();
        var useCase = CreateUseCase(dto);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Title.Should().Be(dto.Title);
        result.CookingTime.Should().Be((RecipeBook.Communication.Enums.CookingTime)dto.CookingTime);
        result.Difficulty.Should().Be(RecipeBook.Communication.Enums.Difficulty.Low);
    }

    [Fact]
    public async Task Error_Duplicated_Ingredients()
    {
        var dto = GeneratedRecipeDtoBuilder.Build();

        var request = RequestGenerateRecipeJsonBuilder.Build(RecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);
        request.Ingredients.Add(request.Ingredients[0]);

        var useCase = CreateUseCase(dto);

        var act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 &&
                e.ErrorMessages.Contains(ResourceMessagesExceptions.DUPLICATED_INGREDIENTS_IN_LIST));
    }

    private static GenerateRecipeUseCase CreateUseCase(GeneratedRecipeDto dto)
    {
        var generateRecipeAPI = GenerateRecipeAIBuilder.Build(dto);

        return new GenerateRecipeUseCase(generateRecipeAPI);
    }
}
