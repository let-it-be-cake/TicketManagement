using Microsoft.AspNetCore.Http;
using TicketManagement.UserInterface.Token;

namespace TicketManagement.UserInterface.Services
{
    internal class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetToken()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[JwtCookie.Name].ToString();
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(JwtCookie.Name, token);
        }

        public void ResetToken()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(JwtCookie.Name);
        }
    }
}
