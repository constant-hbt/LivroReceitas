using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Services.ServiceBus;
public interface IDeleteUserQueue
{
    Task SendMessage(User user);
}
