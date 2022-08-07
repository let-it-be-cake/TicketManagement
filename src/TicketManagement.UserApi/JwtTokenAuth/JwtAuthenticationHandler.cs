using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TicketManagement.UserApi.Services;

namespace TicketManagement.UserApi.JwtTokenAuth
{
    public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {
        private readonly ILogger<JwtAuthenticationHandler> _logger;
        private readonly IJwtTokenService _tokenService;

        public JwtAuthenticationHandler(
            IOptionsMonitor<JwtAuthenticationOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock,
            IJwtTokenService tokenService,
            ILogger<JwtAuthenticationHandler> logger)
            : base(options, loggerFactory, encoder, clock)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
            }

            var tokenWithType = Request.Headers["Authorization"].ToString();

            try
            {
                _tokenService.ValidateToken(tokenWithType);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unvalid token.");
                return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
            }

            var token = tokenWithType["Bearer ".Length..];

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwtToken.Claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
