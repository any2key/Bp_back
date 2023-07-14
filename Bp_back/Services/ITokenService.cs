using Bp_back.Models.Identity;
using Bp_back.Models.Requests;
using Bp_back.Models.Responses;

namespace Bp_back.Services
{
    public interface ITokenService
    {
        Tuple<string, string> GenerateTokens(int userId, string role);
        DataResponse<ValidateRefreshTokenResponseData> ValidateRefreshToken(RefreshTokenRequest refreshTokenRequest);
        bool RemoveRefreshToken(User user);
    }
}
