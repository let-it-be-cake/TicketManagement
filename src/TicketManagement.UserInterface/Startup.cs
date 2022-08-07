using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using TicketManagement.UserInterface.Helper;
using TicketManagement.UserInterface.JwtTokenAuth;
using TicketManagement.UserInterface.Services;

namespace TicketManagement.UserInterface
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Config serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var cultures = new CultureInfo[]
                    {
                        new CultureInfo("en"),
                        new CultureInfo("ru"),
                        new CultureInfo("be"),
                    };
                    options.DefaultRequestCulture = new RequestCulture("en");
                    options.SupportedCultures = cultures;
                    options.SupportedUICultures = cultures;
                });

            services.WebUIDependency(Configuration.GetConnectionString("DefaultConnection"));

            // User api rest ease config.
            var userApi = Configuration["UserApiAddress"];
            services.RestEaseUserApiConfig(userApi);

            // Event api rest ease config.
            var eventApi = Configuration["EventApiAddress"];
            services.RestEaseEventApiConfig(eventApi);

            // Purchase api rest ease config.
            var purchaseApi = Configuration["PurchaseApiAddress"];
            services.RestEasePurchaseApiConfig(purchaseApi);

            // Venue api rest ease config.
            var venueApi = Configuration["VenueApiAddress"];
            services.RestEaseVenueApiConfig(venueApi);

            services.AddAuthentication(JwtAutheticationConstants.SchemeName)
                .AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>(JwtAutheticationConstants.SchemeName, null);

            services.AddScoped<ICartTicket, CartTicketService>();
            services.AddScoped<IPagingValidation, PagingValidationService>();
            services.AddScoped<IMapToViewModel, ViewModelMapper>();
            services.AddScoped<IThirdPartyEventHelper, LocalSaveThirdPartyEvents>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRequestLocalization(
                app.ApplicationServices
                    .GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}