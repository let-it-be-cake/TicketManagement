using Microsoft.Extensions.DependencyInjection;
using TicketManagement.UserInterface.Helper;
using TicketManagement.UserInterface.Services;

namespace TicketManagement.UserInterface
{
    public static class ServiceDependencyExtension
    {
        public static void WebUIDependency(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<ICartTicket, CartTicketService>();
            services.AddScoped<IPagingValidation, PagingValidationService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IMapToViewModel, ViewModelMapper>();
        }
    }
}
