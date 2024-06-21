using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UseCases.Dashboard;
public interface IGetDashboardUseCase
{
    public Task<ResponseRecipesJson> Execute();
}
