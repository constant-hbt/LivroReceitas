using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Infrastructure.Security.Tokens.Access.Generator;

namespace CommonTestUtilities.Tokens;
public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() 
        => new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey: "q2Nt[xl|;8U1h6(es,£E+a8lAbpS?uzH");
}
