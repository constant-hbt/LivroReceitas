using FluentValidation;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Extensions;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.UseCases.User.Update;
public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage(ResourceMessagesExceptions.NAME_EMPTY);
        RuleFor(r => r.Email).NotEmpty().WithMessage(ResourceMessagesExceptions.EMAIL_EMPTY);

        When(r => r.Email.NotEmpty(), () =>
        {
            RuleFor(r => r.Email).EmailAddress().WithMessage(ResourceMessagesExceptions.EMAIL_INVALID);
        });
    }
}
