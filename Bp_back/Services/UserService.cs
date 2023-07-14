using Bp_back.Models.Identity;
using Bp_back.Models.Requests;
using Bp_back.Models.Responses;
using Bp_back.Utils;

namespace Bp_back.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService tokenService;
        public UserService(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }
        public DataResponse<TokenResponseData> Login(LoginRequest loginRequest)
        {
            return BpEx.Run(db =>
            {
                var user = db.Users.SingleOrDefault(user => user.Active && user.Login == loginRequest.Login);
                if (user == null)
                {
                    return new DataResponse<TokenResponseData>() { IsOk = false, Message = "Login not found" };
                }
                var passwordHash = PasswordHelper.HashUsingPbkdf2(loginRequest.Password, Convert.FromBase64String(user.PasswordSalt));
                if (user.Password != passwordHash)
                {
                    return new DataResponse<TokenResponseData>() { IsOk = false, Message = "Invalid Password" };
                }
                var token = tokenService.GenerateTokens(user.Id, user.Role);
                return new DataResponse<TokenResponseData>()
                {
                    IsOk = true,
                    Data = new TokenResponseData()
                    {
                        AccessToken = token.Item1,
                        RefreshToken = token.Item2,
                        Login = user.Login,
                        UserId = user.Id.ToString(),
                        UserRole = user.Role
                    }
                };
            });

        }
        public Response Logout(int userId)
        {
            return BpEx.Run(db =>
            {
                var refreshToken = db.RefreshTokens.FirstOrDefault(o => o.UserId == userId);
                if (refreshToken == null)
                {
                    return Response.OK;
                }
                db.RefreshTokens.Remove(refreshToken);
                var saveResponse = db.SaveChanges();
                if (saveResponse >= 0)
                {
                    return Response.OK;
                }
                return Response.BadResponse("Unable to logout user");
            });
        }
        public Response Signup(CreateUserRequest req)
        {
            return BpEx.Run(db =>
            {
                var existingUser = db.Users.SingleOrDefault(user => user.Login == req.Login);
                if (existingUser != null)
                {

                    return Response.BadResponse("User already exists with the same email");
                }


                var salt = PasswordHelper.GetSecureSalt();
                var passwordHash = PasswordHelper.HashUsingPbkdf2(req.Password, salt);
                var user = new User
                {
                    Email = req.Email,
                    Password = passwordHash,
                    Login = req.Login,
                    PasswordSalt = Convert.ToBase64String(salt),
                    Active = true,
                    Role = req.Role
                };
                db.Users.Add(user);
                var saveResponse = db.SaveChanges();
                if (saveResponse >= 0)
                {
                    return Response.OK;
                }

                return Response.BadResponse("Unable to save the user");
            });
        }
    }
}
