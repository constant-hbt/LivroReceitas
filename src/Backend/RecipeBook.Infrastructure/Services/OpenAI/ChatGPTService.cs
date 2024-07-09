using OpenAI_API;
using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Services.OpenAI;
using OpenAI_API.Chat;
using RecipeBook.Domain.Extensions;

namespace RecipeBook.Infrastructure.Services.OpenAI;
public class ChatGPTService : IGenerateRecipeAI
{
    private const string CHAT_MODEL = "gpt-4o";

    private readonly IOpenAIAPI _openAIAPI;

    public ChatGPTService(IOpenAIAPI openAIAPI)
    {
        _openAIAPI = openAIAPI;
    }

    public async Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
    {
        // O modelo padrão caso não passe um específico, no momento atual, é o gpt-3.5-turbo
        var conversation = _openAIAPI.Chat.CreateConversation(new ChatRequest { Model = CHAT_MODEL });

        conversation.AppendSystemMessage(ResourceOpenApi.STARTING_GENERATE_RECIPE);

        conversation.AppendUserInput(string.Join(';', ingredients));

        var response = await conversation.GetResponseFromChatbotAsync();

        var responseParagraphList = response
            .Split("\n")
            .Where(response => response.NotEmpty())
            .Select(item => item.Replace("[", "").Replace("]", ""))
            .ToList();

        var step = 1;

        return new GeneratedRecipeDto
        {
            Title = responseParagraphList[0],
            CookingTime = (Domain.Enums.CookingTime)Enum.Parse(typeof(Domain.Enums.CookingTime), responseParagraphList[1]),
            Ingredients = responseParagraphList[2].Split(";"),
            Instructions = responseParagraphList[3].Split("@").Select(instruction => new GeneratedInstructionDto
            {
                Text = instruction.Trim(),
                Step = step++
            }).ToList(),
        };
    }
}
