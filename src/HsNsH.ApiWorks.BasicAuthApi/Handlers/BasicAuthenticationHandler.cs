using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace HsNsH.ApiWorks.BasicAuthApi.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options
        , ILoggerFactory logger
        , UrlEncoder encoder
        , ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization header not found"));
        }

        try
        {
            var authHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

            var bytes = Convert.FromBase64String(authHeaderValue.Parameter!);
            var credentials = Encoding.UTF8.GetString(bytes).Split(":");

            var email = credentials[0];
            var pass = credentials[1];

            // Check email and password from store, ex: Inject DBContext and get User 
            if (!email.ToLower(new CultureInfo("en-US")).Equals("hsnsh@outlook.com") || !pass.Equals("1q2w3E*"))
                return Task.FromResult(AuthenticateResult.Fail("Invalid email and password"));

            var claims = new[] { new Claim(ClaimTypes.Name, email) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch (Exception) { return Task.FromResult(AuthenticateResult.Fail("Error has occured")); }
    }
}