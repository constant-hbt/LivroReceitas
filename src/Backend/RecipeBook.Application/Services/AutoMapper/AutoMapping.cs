using AutoMapper;
using RecipeBook.Communication.Requests;

namespace RecipeBook.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ConstructUsing(src => new Domain.Entities.User(src.Name, src.Email, string.Empty));
    }
}
