using FluentValidation;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesExceptions.NAME_EMPTY).Length(2, 50);
        RuleFor(user => user.Email).NotEmpty().MaximumLength(50).EmailAddress();
        RuleFor(user => user.Password).NotEmpty().MinimumLength(6);
    }
}
