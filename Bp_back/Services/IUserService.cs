using Bp_back.Models.Identity;
using Bp_back.Models.Requests;
using Bp_back.Models.Responses;

namespace Bp_back.Services
{
    public interface IUserService
    {
        DataResponse<TokenResponseData> Login(LoginRequest loginRequest);
        Response Signup(CreateUserRequest signupRequest);
        Response Logout(int userId);
    }
}
