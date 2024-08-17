using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.User.Update;
public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserUseCase(ILoggedUser loggedUser, IUserUpdateOnlyRepository repository, IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.User();

        await Validate(request, loggedUser.Email);

        var user = await _repository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _repository.Update(user);

        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var result = new UpdateUserValidator().Validate(request);

        if (currentEmail.Equals(request.Email).IsFalse())
        {
            var userExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (userExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.Email), ResourceMessagesExceptions.EMAIL_ALREADY_REGISTERED));
        }

        if (result.IsValid.IsFalse())
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
