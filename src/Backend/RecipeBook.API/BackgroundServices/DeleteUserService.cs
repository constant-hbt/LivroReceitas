
using Azure.Messaging.ServiceBus;
using RecipeBook.Application.UseCases.User.Delete.Delete;

namespace RecipeBook.API.BackgroundServices;

public class DeleteUserService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ServiceBusProcessor _serviceBusProcessor;

    public DeleteUserService(IServiceProvider services, ServiceBusProcessor serviceBusProcessor)
    {
        _services = services;
        _serviceBusProcessor = serviceBusProcessor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _serviceBusProcessor.ProcessMessageAsync += ProcessMessageAsync;

        _serviceBusProcessor.ProcessErrorAsync += ExceptionReceivedHandler;

        await _serviceBusProcessor.StartProcessingAsync(stoppingToken);
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs eventArgs)
    {
        var message = eventArgs.Message.Body.ToString();

        var userIdentifier = Guid.Parse(message);

        var scope = _services.CreateScope();

        var deleteUserUseCase = scope.ServiceProvider.GetRequiredService<IDeleteUserAccountUseCase>();

        await deleteUserUseCase.Execute(userIdentifier);
    }

    private Task ExceptionReceivedHandler(ProcessErrorEventArgs _) => Task.CompletedTask;

    ~DeleteUserService() => Dispose();

    public override void Dispose() 
    {
        base.Dispose();

        GC.SuppressFinalize(this);
    }
}
