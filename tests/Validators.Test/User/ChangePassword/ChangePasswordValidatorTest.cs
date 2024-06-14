using CommonTestUtilities.Requests;
using FluentAssertions;
using RecipeBook.Application.UseCases.User.ChangePassword;
using RecipeBook.Exceptions;
using Xunit;

namespace Validators.Test.User.ChangePassword;
public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var result = new ChangePasswordValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_New_Password_Empty()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;

        var result = new ChangePasswordValidator().Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.PASSWORD_EMPTY));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_New_Password_Invalid(int passwordLength)
    {
        var request = RequestChangePasswordJsonBuilder.Build(passwordLength);

        var result = new ChangePasswordValidator().Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.PASSWORD_INVALID));
    }
}
