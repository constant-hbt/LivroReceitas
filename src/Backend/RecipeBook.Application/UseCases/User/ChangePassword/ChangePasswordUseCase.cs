using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.Cryptography;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.User.ChangePassword;
public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;

    public ChangePasswordUseCase(ILoggedUser loggedUser, IUserUpdateOnlyRepository repository, IUnitOfWork unitOfWork, IPasswordEncripter passwordEncripter)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.User();

        Validate(request, loggedUser);

        var user = await _repository.GetById(loggedUser.Id);

        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _repository.Update(user);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        if (_passwordEncripter.IsValid(request.Password, loggedUser.Password).IsFalse())
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.Password), ResourceMessagesExceptions.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
