using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Api.IntegrationTest
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        public static string SCHEME = "CustomScheme";
        public static string TOKEN = "1234567890";

        public TestAuthenticationHandler(IOptionsMonitor<TestAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].FirstOrDefault();
                if (token != TOKEN)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid token."));
                }
            }
            catch (InvalidOperationException)
            {
                return Task.FromResult(AuthenticateResult.Fail(""));
            }

            var claimsPrincipal = new ClaimsPrincipal(Options.Identity);
            var authenticationTicket = new AuthenticationTicket(claimsPrincipal, this.Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }
    }

    public static class TestAuthenticationExtension
    {
        public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }

    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public virtual ClaimsIdentity Identity { get; } = new ClaimsIdentity(
            new Claim[]
            {
                new Claim(ClaimTypes.Name, "test_user")
            },
            TestAuthenticationHandler.SCHEME
        );
    }
}