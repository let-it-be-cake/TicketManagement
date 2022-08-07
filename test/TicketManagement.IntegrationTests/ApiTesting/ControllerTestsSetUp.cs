using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;
using TicketManagement.EventApi;
using TicketManagement.PurchaseApi;
using TicketManagement.UserApi;
using TicketManagement.UserApi.Models;
using TicketManagement.UserInterface;
using TicketManagement.VenueApi;

namespace TicketManagement.IntegrationTests.ApiTesting
{
    public class ControllerTestsSetUp
    {
        public string AdminToken { get; set; }

        public string UserApiAddress { get; set; }

        public string EventApiAddress { get; set; }

        public string PurchaseApiAddress { get; set; }

        public string VenueApiAddress { get; set; }

        public HttpClient Client { get; private set; }

        public IServiceProvider Provider { get; private set; }

        public IWebHostBuilder WebUi { get; set; }

        public TestServer WebUiServer { get; set; }

        public IWebHost UserApi { get; set; }

        public IWebHost EventApi { get; set; }

        public IWebHost PurchaseApi { get; set; }

        public IWebHost VenueApi { get; set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUpBase()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            UserApiAddress = config["UserApiAddress"];
            EventApiAddress = config["EventApiAddress"];
            PurchaseApiAddress = config["PurchaseApiAddress"];
            VenueApiAddress = config["VenueApiAddress"];

            UserApi = WebHost.CreateDefaultBuilder().UseConfiguration(config).UseStartup<UserApi.Startup>().Start(UserApiAddress);
            EventApi = WebHost.CreateDefaultBuilder().UseConfiguration(config).UseUrls().UseStartup<EventApi.Startup>().Start(EventApiAddress);
            PurchaseApi = WebHost.CreateDefaultBuilder().UseConfiguration(config).UseStartup<PurchaseApi.Startup>().Start(PurchaseApiAddress);
            VenueApi = WebHost.CreateDefaultBuilder().UseConfiguration(config).UseStartup<VenueApi.Startup>().Start(VenueApiAddress);

            var httpClient = new HttpClient();
            var loginForm = new LoginModel
            {
                Email = config["AdminLogin"],
                Password = config["AdminPassword"],
            };

            HttpResponseMessage response = await httpClient.PostAsync(UserApiAddress + "/users/login",
                new StringContent(JsonConvert.SerializeObject(loginForm), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            AdminToken = await response.Content.ReadAsStringAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDownBase()
        {
            await UserApi.StopAsync();
            await EventApi.StopAsync();
            await PurchaseApi.StopAsync();
            await VenueApi.StopAsync();
        }

        [SetUp]
        public void SetUpBase()
        {
            WebUi = WebHost.CreateDefaultBuilder().UseStartup<UserInterface.Startup>().UseUrls("https://localhost:5003");
            WebUiServer = new TestServer(WebUi);

            Client = WebUiServer.CreateClient();
            Provider = WebUiServer.Host.Services.CreateScope().ServiceProvider;
        }
    }
}
