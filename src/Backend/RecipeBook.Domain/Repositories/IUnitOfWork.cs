namespace RecipeBook.Domain.Repositories;
public interface IUnitOfWork
{
    public Task<int> Commit();
}
