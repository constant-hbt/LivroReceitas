using AutoMapper;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ConstructUsing(src => new Domain.Entities.User(src.Name, src.Email, string.Empty));
    }

    private void DomainToResponse()
    {
        CreateMap<Domain.Entities.User, ResponseUserProfileJson>();
    }
}
