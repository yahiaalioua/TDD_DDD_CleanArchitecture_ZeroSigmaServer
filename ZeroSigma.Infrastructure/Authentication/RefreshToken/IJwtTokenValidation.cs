using Microsoft.IdentityModel.Tokens;

namespace ZeroSigma.Infrastructure.Authentication.RefreshToken
{
    public interface IJwtTokenValidation
    {
        TokenValidationParameters Parameters();
        bool ValidateJwtSecurityAlgorith(SecurityToken validatedToken);
    }
}