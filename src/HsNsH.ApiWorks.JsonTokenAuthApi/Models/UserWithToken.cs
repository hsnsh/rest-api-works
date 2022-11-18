namespace HsNsH.ApiWorks.JsonTokenAuthApi.Models;

public class UserWithTokenDto : User
{
    public string AccessToken { get; }
    public DateTime TokenExpireTime { get; }

    public string RefreshToken { get; }

    public UserWithTokenDto(User user, string accessToken, DateTime tokenExpireTime, string refreshToken)
    {
        UserId = user.UserId;
        FirstName = user.FirstName;
        FamilyName = user.FamilyName;
        Email = user.Email;

        AccessToken = accessToken;
        TokenExpireTime = tokenExpireTime;
        RefreshToken = refreshToken;
    }
}