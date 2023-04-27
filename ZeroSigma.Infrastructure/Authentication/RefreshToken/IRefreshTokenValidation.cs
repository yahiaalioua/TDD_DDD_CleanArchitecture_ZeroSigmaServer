using Microsoft.IdentityModel.Tokens;

namespace ZeroSigma.Infrastructure.Authentication.RefreshToken
{
    public interface IRefreshTokenValidation
    {
        TokenValidationParameters Parameters();
        bool ValidateJwtSecurityAlgorith(SecurityToken validatedToken);
    }
}