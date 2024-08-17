using FluentValidation;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.UseCases.Recipe;
public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(r => r.Title).NotEmpty().WithMessage(ResourceMessagesExceptions.RECIPE_TITLE_EMPTY);
        RuleFor(r => r.CookingTime).IsInEnum().WithMessage(ResourceMessagesExceptions.COOKING_TIME_NOT_SUPPORTED);
        RuleFor(r => r.Difficulty).IsInEnum().WithMessage(ResourceMessagesExceptions.DIFFICULTY_LEVEL_NOT_SUPPORTED);
        RuleFor(r => r.Ingredients).NotEmpty().WithMessage(ResourceMessagesExceptions.AT_LEAST_ONE_INGREDIENT);
        RuleFor(r => r.Instructions).NotEmpty().WithMessage(ResourceMessagesExceptions.AT_LEAST_ONE_INSTRUCTION);

        RuleForEach(r => r.DishTypes).IsInEnum().WithMessage(ResourceMessagesExceptions.DISH_TYPE_NOT_SUPPORTED);
        RuleForEach(r => r.Ingredients).NotEmpty().WithMessage(ResourceMessagesExceptions.INGREDIENT_EMPTY);

        RuleForEach(r => r.Instructions).ChildRules(instructionRule =>
        {
            instructionRule.RuleFor(instruction => instruction.Step)
                .GreaterThan(0).WithMessage(ResourceMessagesExceptions.LESS_OR_EQUAL_ZERO_INSTRUCTION_STEP);

            instructionRule.RuleFor(instruction => instruction.Text)
                .NotEmpty().WithMessage(ResourceMessagesExceptions.INSTRUCTION_TEXT_EMPTY)
                .MaximumLength(2000).WithMessage(ResourceMessagesExceptions.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS);
        });

        RuleFor(r => r.Instructions)
            .Must(instructions => instructions.Select(i => i.Step).Distinct().Count() == instructions.Count).WithMessage(ResourceMessagesExceptions.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER);
    }
}
