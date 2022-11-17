using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    private readonly ILogger<AccountController> _logger;
    private readonly JWTSettings _jwtSettings;

    public AccountController(ILogger<AccountController> logger, IOptions<JWTSettings> jwtSettings)
    {
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest input)
    {
        try
        {
            // Check email and password from store, ex: Inject DBContext and get User 
            if (!input.Email.ToLower(new CultureInfo("en-US")).Equals("hsnsh@outlook.com") || !input.Pass.Equals("1q2w3E*"))
                return BadRequest("Invalid email or password");

            // Sign your token here...
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenExpiresUtcTime = DateTime.UtcNow.AddHours(2);
            var tokenDescriptor = new SecurityTokenDescriptor { Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, input.Email) }), Expires = tokenExpiresUtcTime, SigningCredentials = signingCredentials };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var userToken = tokenHandler.WriteToken(securityToken);

            return Ok(new LoginResponse { AccessToken = userToken, TokenExpireTime = tokenExpiresUtcTime, User = new User { UserId = Guid.NewGuid(), FirstName = "Hasan", FamilyName = "SAHIN", Email = input.Email } });
        }
        catch (Exception) { return BadRequest("Error has occured"); }
    }
}