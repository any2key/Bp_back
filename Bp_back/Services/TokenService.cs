using Bp_back.Context;
using Bp_back.Models.Identity;
using Bp_back.Models.Requests;
using Bp_back.Models.Responses;
using Bp_back.Utils;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bp_back.Services
{
    public class TokenService : ITokenService
    {

        public TokenService(AppDbContext db)
        {
        }
        public Tuple<string, string> GenerateTokens(int userId, string role)
        {
            return BpEx.Run(db =>
            {
                var accessToken = TokenHelper.GenerateAccessToken(userId, role);
                var refreshToken = TokenHelper.GenerateRefreshToken();
                var userRecord = db.Users.Include(o => o.RefreshTokens).FirstOrDefault(e => e.Id == userId);
                if (userRecord == null)
                {
                    return null;
                }
                var salt = PasswordHelper.GetSecureSalt();
                var refreshTokenHashed = PasswordHelper.HashUsingPbkdf2(refreshToken, salt);
                if (userRecord.RefreshTokens != null && userRecord.RefreshTokens.Any())
                {
                    RemoveRefreshToken(userRecord);
                }
                userRecord.RefreshTokens?.Add(new RefreshToken
                {
                    ExpiryDate = DateTime.Now.AddDays(30),
                    Ts = DateTime.Now,
                    UserId = userId,
                    TokenHash = refreshTokenHashed,
                    TokenSalt = Convert.ToBase64String(salt)
                });
                db.SaveChanges();
                var token = new Tuple<string, string>(accessToken, refreshToken);
                return token;
            });
        }
        public bool RemoveRefreshToken(User user)
        {
            return BpEx.Run(db =>
            {
                var userRecord = db.Users.Include(o => o.RefreshTokens).FirstOrDefault(e => e.Id == user.Id);
                if (userRecord == null)
                {
                    return false;
                }
                if (userRecord.RefreshTokens != null && userRecord.RefreshTokens.Any())
                {
                    var currentRefreshToken = userRecord.RefreshTokens.First();
                    db.RefreshTokens.Remove(currentRefreshToken);
                }
                return false;
            });
        }
        public DataResponse<ValidateRefreshTokenResponseData> ValidateRefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            return BpEx.Run(db =>
            {
                try
                {
                    var refreshToken = db.RefreshTokens.FirstOrDefault(o => o.UserId == refreshTokenRequest.UserId);
                    var user = db.Users.FirstOrDefault(e => e.Id == refreshToken.UserId);
                    var response = new DataResponse<int>() { IsOk = true, Data = -1 };
                    if (refreshToken == null)
                    {
                        return new DataResponse<ValidateRefreshTokenResponseData>() { IsOk = false, Message = "Некорректная сессия или пользователь вышел из системы" };
                    }
                    var refreshTokenToValidateHash = PasswordHelper.HashUsingPbkdf2(refreshTokenRequest.RefreshToken, Convert.FromBase64String(refreshToken.TokenSalt));
                    if (refreshToken.TokenHash != refreshTokenToValidateHash)
                    {
                        return new DataResponse<ValidateRefreshTokenResponseData>() { IsOk = false, Message = "Некорректный рефреш токен" };
                    }

                    if (refreshToken.ExpiryDate < DateTime.Now)
                    {
                        return new DataResponse<ValidateRefreshTokenResponseData>() { IsOk = false, Message = "Рефреш токен просрочен" };

                    }

                    return new DataResponse<ValidateRefreshTokenResponseData>()
                    {
                        IsOk = true,
                        Data = new ValidateRefreshTokenResponseData() { UserId = refreshTokenRequest.UserId, Role = user.Role, Login = user.Login }
                    };
                }
                catch (Exception ex)
                {
                    return new DataResponse<ValidateRefreshTokenResponseData>() { IsOk = false, Message = ex.Message };
                }
            });
        }
    }
}
