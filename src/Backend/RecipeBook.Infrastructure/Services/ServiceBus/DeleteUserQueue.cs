using Azure.Messaging.ServiceBus;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Services.ServiceBus;

namespace RecipeBook.Infrastructure.Services.ServiceBus;
public class DeleteUserQueue : IDeleteUserQueue
{
    private readonly ServiceBusSender _serviceBusSender;

    public DeleteUserQueue(ServiceBusSender serviceBusSender)
    {
        _serviceBusSender = serviceBusSender;
    }

    public async Task SendMessage(User user)
    {
        await _serviceBusSender.SendMessageAsync(new ServiceBusMessage(user.UserIdentifier.ToString()));
    }
}
