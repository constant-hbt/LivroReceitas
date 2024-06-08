using CommonTestUtilities.Requests;
using FluentAssertions;
using RecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.User.Register;

// Herda da classe IClassFixture para criar um servidor de teste,
// com o ponto de início sendo a classe Program do projeto RecipeBook.API, indicando para executar essa API no servidor de testes criado
public class RegisterUserTest : RecipeBookClassFixture
{
    private readonly string _method = "user";

    // Cria um HttpClient e o armazena para ser utilizado nos testes, já que não sabemos qual porta o servidor de testes está rodando
    public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Success()
    {
        var request = new RequestRegisterUserJsonBuilder().Build();

        var response = await DoPost(_method, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // É uma boa prática não desserializar na classe de resposta, pois o frontend muitas vezes não terá acesso a essa classe
        // Assim, convertendo em um JsonDocument e verificando o conteúdo 
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString()
            .Should().NotBeNullOrWhiteSpace()
            .And.Be(request.Name);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = new RequestRegisterUserJsonBuilder().Build();
        request.Name = string.Empty;

        var response = await DoPost(_method, request, culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // É uma boa prática não desserializar na classe de resposta, pois o frontend muitas vezes não terá acesso a essa classe
        // Assim, convertendo em um JsonDocument e verificando o conteúdo 
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesExceptions.ResourceManager.GetString(nameof(ResourceMessagesExceptions.NAME_EMPTY), new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
