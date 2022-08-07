using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestEase;
using TicketManagement.UserInterface.Clients.UserApi;
using TicketManagement.UserInterface.Helper;
using TicketManagement.UserInterface.Models;
using TicketManagement.UserInterface.Models.ViewModels;
using TicketManagement.UserInterface.Services;

namespace TicketManagement.UserInterface.Controllers
{
    public class AccountController : Controller
    {
        private readonly ITokenClient _tokenClient;

        private readonly ITokenService _tokenService;

        private readonly ILogger<AccountController> _logger;

        public AccountController(ITokenClient tokenClient,
                                 ITokenService tokenService,
                                 ILogger<AccountController> logger)
        {
            _tokenClient = tokenClient;
            _tokenService = tokenService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new RegisterModel
            {
                FirstName = model.Name,
                Surname = model.Surname,
                Language = model.Language,
                TimeZoneId = model.TimeOffsetId,
                Email = model.Email,
                Password = model.Password,
            };

            string token = null;

            try
            {
                token = await _tokenClient.RegisterAsync(user);
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                string errorMessage = "The user with this email is already registered.";
                _logger.LogInformation(ex, errorMessage);
                ModelState.AddModelError("", errorMessage);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Non-caught register error.");
                return View(model);
            }

            _tokenService.SetToken(token);

            return RedirectToAction("ConfigUser");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loginModel = new LoginModel
            {
                Email = model.Email,
                Password = model.Password,
            };

            string token = null;

            try
            {
                token = await _tokenClient.LoginAsync(loginModel);
            }
            catch (ApiException ex) when (
                ex.StatusCode == HttpStatusCode.BadRequest ||
                ex.StatusCode == HttpStatusCode.NotFound)
            {
                var errorMessage = "Wrong login or password.";
                _logger.LogInformation(ex, errorMessage);
                ModelState.AddModelError("", errorMessage);
                return View(model);
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
            {
                var errorMessage = "User is blocked.";
                _logger.LogInformation(ex, errorMessage);
                ModelState.AddModelError("", errorMessage);
                return View(model);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Non-caught login error.");
                return View(model);
            }

            _tokenService.SetToken(token);

            return RedirectToAction("ConfigUser");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConfigUser()
        {
            string userLanguage = User.GetClaim(nameof(Entities.Identity.User.Language));

            CookieHelper.SetLocal(HttpContext.Response, userLanguage);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _tokenService.ResetToken();

            return RedirectToAction("Index", "Home");
        }
    }
}