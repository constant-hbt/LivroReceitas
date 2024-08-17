using FluentValidation;
using RecipeBook.Application.SharedValidators;
using RecipeBook.Communication.Requests;

namespace RecipeBook.Application.UseCases.User.ChangePassword;
public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(cp => cp.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}
