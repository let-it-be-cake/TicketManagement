using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TicketManagement.EventApi.Clients;

namespace TicketManagement.EventApi.JwtTokenAuth
{
    public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {
        private readonly ILogger<JwtAuthenticationHandler> _logger;
        private readonly ITokenClient _tokenClient;

        public JwtAuthenticationHandler(
            IOptionsMonitor<JwtAuthenticationOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock,
            ITokenClient tokenClient,
            ILogger<JwtAuthenticationHandler> logger)
            : base(options, loggerFactory, encoder, clock)
        {
            _tokenClient = tokenClient;
            _logger = logger;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var tokenWithType = Request.Headers["Authorization"].ToString();

            try
            {
                await _tokenClient.ValidateTokenAsync(tokenWithType);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unvalid token.");
                return AuthenticateResult.Fail("Unauthorized");
            }

            var token = tokenWithType["Bearer ".Length..];

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwtToken.Claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
