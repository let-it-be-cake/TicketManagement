using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TicketManagement.UserApi.Database.Identity;
using TicketManagement.UserApi.Settings;

namespace TicketManagement.UserApi.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private const string TokenType = "Bearer ";
        private readonly JwtTokenSettings _settings;

        public JwtTokenService(IOptions<JwtTokenSettings> options)
        {
            _settings = options.Value;
        }

        public string GenerateToken(User user, IList<string> roles)
        {
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            var userClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(nameof(user.Id), user.Id.ToString()),
                new Claim(nameof(user.Email), user.Email),
                new Claim(nameof(user.FirstName), user.FirstName),
                new Claim(nameof(user.Surname), user.Surname),
                new Claim(nameof(user.TimeZoneId), user.TimeZoneId),
                new Claim(nameof(user.Language), user.Language),
                new Claim(nameof(user.Money), user.Money.ToString()),
            };
            userClaims.AddRange(roleClaims);
            var jwt = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                expires: DateTime.Now.AddMinutes(_settings.LifeTime),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey)),
                    SecurityAlgorithms.HmacSha256),
                claims: userClaims);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            string token = TokenType + encodedJwt;

            return token;
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!token.StartsWith(TokenType))
            {
                return false;
            }

            token = token[TokenType.Length..];

            try
            {
                tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _settings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _settings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey)),
                    ValidateLifetime = false,
                },
                out var _);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
