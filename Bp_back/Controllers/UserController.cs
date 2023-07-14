using Bp_back.Models.Identity;
using Bp_back.Models.Requests;
using Bp_back.Models.Responses;
using Bp_back.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace Bp_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBaseEx
    {
        private readonly IUserService userService;
        private readonly ITokenService tokenService;
        public UserController(IUserService userService, ITokenService tokenService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            return SafeRun(_ =>
            {

                if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Login) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    return Models.Responses.Response.BadResponse("Некорректный логин и/или пароль");
                }
                var loginResponse = userService.Login(loginRequest);
                if (!loginResponse.IsOk)
                {
                    return Models.Responses.Response.BadResponse(String.IsNullOrEmpty(loginResponse.Message) ? "Unsuccessfully" : loginResponse.Message);

                }
                return loginResponse;

            });
        }


        [HttpPost]
        [Route("refresh_token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {

            return SafeRun(_ =>
            {
                if (refreshTokenRequest == null || string.IsNullOrEmpty(refreshTokenRequest.RefreshToken) || refreshTokenRequest.UserId == 0)
                {
                    throw new CodeException("Отсутствуют данные о рефреш токене");
                }
                var validateRefreshTokenResponse = tokenService.ValidateRefreshToken(refreshTokenRequest);
                if (!validateRefreshTokenResponse.IsOk)
                {
                    throw new CodeException(validateRefreshTokenResponse.Message);
                }
                var tokenResponse = tokenService.GenerateTokens(validateRefreshTokenResponse.Data.UserId, validateRefreshTokenResponse.Data.Role);
                return new DataResponse<TokenResponseData>()
                {
                    Data = new TokenResponseData()
                    {
                        AccessToken = tokenResponse.Item1,
                        RefreshToken = tokenResponse.Item2,
                        UserRole = validateRefreshTokenResponse.Data.Role,
                        UserId = validateRefreshTokenResponse.Data.UserId.ToString(),
                        Login = validateRefreshTokenResponse.Data.Login
                    }
                };
            });
        }


        [HttpPost]
        [Route("createuser")]
        [Authorize]
        public async Task<IActionResult> CreateUser(CreateUserRequest req)
        {
            return SafeRun(_ =>
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList();
                    if (errors.Any())
                    {
                        return Models.Responses.Response.BadResponse(String.Join(",", errors));

                    }
                }

                var signupResponse = userService.Signup(req);
                if (!signupResponse.IsOk)
                {
                    return UnprocessableEntity(signupResponse);
                }
                return Models.Responses.Response.OK;
            });
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            return SafeRun(_ =>
            {
                var logout = userService.Logout(UserID ?? default(int));
                if (!logout.IsOk)
                {
                    return UnprocessableEntity(logout);
                }
                return Models.Responses.Response.OK;
            });
        }


        // Все, что выше нужно вынести в отдельный Authorize контроллер

    }
}
