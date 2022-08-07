using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Repositories.EntityFramework;
using TicketManagement.UserInterface;

namespace TicketManagement.IntegrationTests.ApiTesting
{
    public class WebAppTesting : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<TicketManagementContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var serviceProvider = new ServiceCollection()
                    .BuildServiceProvider();

                services.AddDbContext<TicketManagementContext>(options =>
                {
                    options.UseInternalServiceProvider(serviceProvider);
                });
            });
        }
    }
}