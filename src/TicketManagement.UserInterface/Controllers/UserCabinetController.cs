using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestEase;
using TicketManagement.Entities.Tables;
using TicketManagement.UserInterface.Clients.PurchaseApi;
using TicketManagement.UserInterface.Clients.UserApi;
using TicketManagement.UserInterface.Helper;
using TicketManagement.UserInterface.Models;
using TicketManagement.UserInterface.Models.ViewModels.UserCabinet;
using TicketManagement.UserInterface.Services;

namespace TicketManagement.UserInterface.Controllers
{
    [Authorize]
    public class UserCabinetController : Controller
    {
        private readonly ITokenService _tokenService;

        private readonly IMapToViewModel _mapHelper;

        private readonly IUserClient _userClient;
        private readonly IPurchaseClient _purchaseClient;

        private readonly ILogger<UserCabinetController> _logger;

        public UserCabinetController(ITokenService tokenService,
                                     IMapToViewModel mapHelper,
                                     IUserClient userClient,
                                     IPurchaseClient purchaseClient,
                                     ILogger<UserCabinetController> logger)
        {
            _tokenService = tokenService;
            _mapHelper = mapHelper;
            _userClient = userClient;
            _purchaseClient = purchaseClient;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userModel = new UserCabinetNoPasswordViewModel
            {
                Name = User.GetClaim(nameof(Entities.Identity.User.FirstName)),
                Surname = User.GetClaim(nameof(Entities.Identity.User.Surname)),
            };

            return View(userModel);
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {
            int userId = int.Parse(User.GetClaim(nameof(Entities.Identity.User.Id)));
            List<Ticket> tickets = await _purchaseClient.GetUserTickets(userId, _tokenService.GetToken());

            string timeZoneId = User.GetClaim(nameof(Entities.Identity.User.TimeZoneId));
            TimeSpan userOffset = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId).BaseUtcOffset;

            return View(await _mapHelper.TicketToViewModelaAsync(userOffset, tickets));
        }

        [HttpGet]
        public IActionResult Edit()
        {
            UserModel user = User.GetUser();
            var userModel = _mapHelper.UserEditViewModel(user);

            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditWithPassword(UserCabinetWithPasswordViewModel userModel)
        {
            UserModel user = User.GetUser();
            if (!ModelState.IsValid)
            {
                return View("Edit", _mapHelper.UserEditViewModel(user));
            }

            var passwordValidateModel = new PasswordValidateModel
            {
                UserId = user.Id,
                Password = userModel.OldPassword,
            };

            try
            {
                await _userClient.PasswordValidate(passwordValidateModel, _tokenService.GetToken());
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                string errorMessage = "Wrong password.";
                _logger.LogInformation(ex, errorMessage);
                ModelState.AddModelError("", errorMessage);
                return View("Edit", _mapHelper.UserEditViewModel(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Non-caught password validate error.");
                return View("Edit", _mapHelper.UserEditViewModel(user));
            }

            user.Email = userModel.Email;
            user.Money = userModel.Money;

            string newToken = null;
            bool passwordsIsNull = string.IsNullOrEmpty(userModel.Password) || string.IsNullOrEmpty(userModel.PasswordConfirm);
            bool passwordConfrim = userModel.Password == userModel.PasswordConfirm;

            if (!passwordConfrim)
            {
                ModelState.AddModelError("", "Passwords don't match");
                return RedirectToAction("Index");
            }

            if (passwordsIsNull)
            {
                newToken = await _userClient.UpdateAsync(user, _tokenService.GetToken());

                _tokenService.SetToken(newToken);

                return RedirectToAction("Index");
            }

            var passwordModel = new PasswordModel
            {
                UserId = user.Id,
                OldPassword = userModel.OldPassword,
                NewPassword = userModel.Password,
            };

            passwordModel.NewPassword = userModel.Password;

            try
            {
                newToken = await _userClient.PasswordReset(passwordModel, _tokenService.GetToken());
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                string errorMessage = "Wrong password.";
                _logger.LogInformation(ex, errorMessage);
                ModelState.AddModelError("", errorMessage);
                return View("Edit", _mapHelper.UserEditViewModel(user));
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                string errorMessage = "User not found.";
                _logger.LogInformation(ex, errorMessage);
                ModelState.AddModelError("", errorMessage);
                return View("Edit", _mapHelper.UserEditViewModel(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Non-caught password reset error.");
                return View(userModel);
            }

            _tokenService.SetToken(newToken);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditNoPassword(UserCabinetNoPasswordViewModel userModel)
        {
            UserModel user = User.GetUser();

            if (!ModelState.IsValid)
            {
                return View("Edit", _mapHelper.UserEditViewModel(user));
            }

            user.FirstName = userModel.Name;
            user.Surname = userModel.Surname;
            user.TimeZoneId = userModel.TimeOffsetId;
            user.Language = userModel.Language;

            await _userClient.UpdateAsync(user, _tokenService.GetToken());

            SetCookie(user.Language);

            return RedirectToAction("Index");
        }

        private void SetCookie(string culture)
            => Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                });
    }
}
