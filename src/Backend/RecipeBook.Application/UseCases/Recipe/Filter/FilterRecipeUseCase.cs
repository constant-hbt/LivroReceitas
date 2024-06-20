using AutoMapper;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Recipe.Filter;
internal class FilterRecipeUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository repository) : IFilterRecipeUseCase
{
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IRecipeReadOnlyRepository _repository = repository;

    public async Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.User();

        var filters = new Domain.Dtos.FilterRecipesDto
        {
            RecipeTitle_Ingredient = request.RecipeTitle_Ingredient,
            CookingTimes = request.CookingTimes.Distinct().Select(c => (Domain.Enums.CookingTime)c).ToList(),
            Difficulties = request.Difficulties.Distinct().Select(d => (Domain.Enums.Difficulty)d).ToList(),
            DishTypes = request.DishTypes.Distinct().Select(dt => (Domain.Enums.DishType)dt).ToList(),
        };

        var recipes = await _repository.Filter(loggedUser, filters);

        return new ResponseRecipesJson
        {
            Recipes = _mapper.Map<IEnumerable<ResponseShortRecipeJson>>(recipes)
        };
    }

    private static void Validate(RequestFilterRecipeJson request)
    {
        var result = new FilterRecipeValidator().Validate(request);

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
