namespace HsNsH.ApiWorks.JsonTokenAuthApi.Models;

public class LoginRequest
{
    public string Email { get; set; }
    public string Pass { get; set; }
}

public class LoginResponse
{
    public string AccessToken { get; set; }
    public DateTime TokenExpireTime { get; set; }
    public User User { get; set; }
}