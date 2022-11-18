using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HsNsH.ApiWorks.JsonTokenAuthApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HsNsH.ApiWorks.JsonTokenAuthApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private static readonly User[] Users = new[] { new User { UserId = Guid.Parse("a03cf65c-edfc-4a23-90a8-112fd957fa5a"), FirstName = "Hasan", FamilyName = "SAHIN", Email = "hsnsh@outlook.com" } };
    private static readonly List<RefreshToken> RefreshTokens = new List<RefreshToken>();

    private readonly ILogger<AccountController> _logger;
    private readonly JWTSettings _jwtSettings;

    public AccountController(ILogger<AccountController> logger, IOptions<JWTSettings> jwtSettings)
    {
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto input)
    {
        try
        {
            // Check email and password
            var userEntity = Users.FirstOrDefault(x => x.Email.Equals(input.Email.ToLower(new CultureInfo("en-US"))));
            if (userEntity == null) return BadRequest("Invalid email or password");
            if (!input.Pass.Equals("1q2w3E*")) return BadRequest("Invalid email or password");

            // Save refresh token
            var refreshToken = GenerateRefreshToken(userEntity.UserId);
            RefreshTokens.Add(refreshToken);

            // Sign your token here...
            var tokenExpireTime = DateTime.UtcNow.AddHours(2);
            var accessToken = GenerateAccessToken(userEntity, tokenExpireTime);

            _logger.LogInformation("User logged in => {Username}", userEntity.Email);
            return Ok(new UserWithTokenDto(userEntity, accessToken, tokenExpireTime, refreshToken.Token));
        }
        catch (Exception) { return BadRequest("Error has occured"); }
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest input)
    {
        try
        {
            var userEntity = GetUserFromAccessToken(input.AccessToken);
            if (userEntity == null || !ValidateRefreshToken(userEntity.UserId, input.RefreshToken))
                return BadRequest("Invalid refresh token");

            // Save new refresh token
            var newRefreshToken = GenerateRefreshToken(userEntity.UserId);
            RefreshTokens.Add(newRefreshToken);

            // Sign your token here...
            var tokenExpireTime = DateTime.UtcNow.AddHours(2);
            var accessToken = GenerateAccessToken(userEntity, tokenExpireTime);

            return Ok(new UserWithTokenDto(userEntity, accessToken, tokenExpireTime, newRefreshToken.Token));
        }
        catch (Exception x)
        {
            return BadRequest("Error has occured");
        }
    }

    private static RefreshToken GenerateRefreshToken(Guid userId)
    {
        var refreshToken = new RefreshToken { Id = Guid.NewGuid(), UserId = userId };

        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            refreshToken.Token = Convert.ToBase64String(randomNumber);
        }

        refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

        return refreshToken;
    }

    private string GenerateAccessToken(User userEntity, DateTime tokenExpireTime)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor { Subject = new ClaimsIdentity(new Claim[] { new Claim("userid", userEntity.UserId.ToString()), new Claim(ClaimTypes.Name, userEntity.Email), new Claim(ClaimTypes.GivenName, userEntity.FirstName), new Claim(ClaimTypes.Surname, userEntity.FamilyName), new Claim(ClaimTypes.Email, userEntity.Email), }), Expires = tokenExpireTime, SigningCredentials = signingCredentials };
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return accessToken;
    }

    private User? GetUserFromAccessToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };

        var principle = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

        if (securityToken is JwtSecurityToken jwtSecurityToken && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            var userIdValue = principle.FindFirst("userid")?.Value;
            if (Guid.TryParse(userIdValue, out var userId))
            {
                return Users.FirstOrDefault(x => x.UserId.Equals(userId));
            }
        }

        return null;
    }

    private static bool ValidateRefreshToken(Guid userId, string rToken)
    {
        var refreshToken = RefreshTokens
            .Where(rt => rt.Token.Equals(rToken))
            .MaxBy(o => o.ExpiryDate);

        return refreshToken != null && refreshToken.UserId == userId && refreshToken.ExpiryDate > DateTime.UtcNow;
    }
}