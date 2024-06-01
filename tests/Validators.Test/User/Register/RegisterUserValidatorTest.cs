using CommonTestUtilities.Requests;
using FluentAssertions;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Exceptions;
using Xunit;

namespace Validators.Test.User.Register;
public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterUserValidator();
        var request = new RequestRegisterUserJsonBuilder().Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var validator = new RegisterUserValidator();
        var request = new RequestRegisterUserJsonBuilder().Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.NAME_EMPTY));
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var validator = new RegisterUserValidator();
        var request = new RequestRegisterUserJsonBuilder().Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidator();
        var request = new RequestRegisterUserJsonBuilder().Build();
        request.Email = "email.com";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.EMAIL_INVALID));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLength)
    {
        var validator = new RegisterUserValidator();
        var request = new RequestRegisterUserJsonBuilder(passwordLength).Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.PASSWORD_INVALID));
    }
}
