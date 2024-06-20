using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Services.AutoMapper;
using RecipeBook.Application.UseCases.Login.DoLogin;
using RecipeBook.Application.UseCases.Recipe;
using RecipeBook.Application.UseCases.Recipe.Filter;
using RecipeBook.Application.UseCases.User.ChangePassword;
using RecipeBook.Application.UseCases.User.Profile;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Application.UseCases.User.Update;
using Sqids;

namespace RecipeBook.Application;
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfigurationManager configuration)
    {
        AddAutoMapper(services, configuration);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services, IConfigurationManager configuration)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
        });

        services.AddScoped(options => new AutoMapper.MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping(sqids));
        }).CreateMapper());
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
        services.AddScoped<IFilterRecipeUseCase, FilterRecipeUseCase>();
    }
}
