using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestEase;
using TicketManagement.UserInterface.Clients.UserApi;
using TicketManagement.UserInterface.Models;
using TicketManagement.UserInterface.Models.Maps;
using TicketManagement.UserInterface.Models.ViewModels;
using TicketManagement.UserInterface.Services;

namespace TicketManagement.UserInterface.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersManageController : Controller
    {
        private readonly IUserClient _userClient;
        private readonly ITokenClient _tokenClient;
        private readonly ITokenService _tokenService;

        private readonly ILogger<UsersManageController> _logger;

        public UsersManageController(IUserClient userClient,
                                     ITokenClient tokenClient,
                                     ITokenService tokenService,
                                     ILogger<UsersManageController> logger)
        {
            _userClient = userClient;
            _tokenClient = tokenClient;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = new List<UserViewModel>();
            List<int> adminsId = (await _userClient.GetRoleUsers("Admin", _tokenService.GetToken()))
                .Select(o => o.Id)
                .ToList();

            foreach (var user in await _userClient.GetAllUsers(_tokenService.GetToken()))
            {
                if (adminsId.Contains(user.Id))
                {
                    continue;
                }

                users.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Surname = user.Surname,
                    Language = user.Language,
                    Money = user.Money,
                    TimeZoneId = user.TimeZoneId,
                    IsBlocked = user.IsBlocked,
                });
            }

            return View(users);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            RegisterModel user = new RegisterModel
            {
                FirstName = model.Name,
                Surname = model.Surnmae,
                Language = model.Language,
                TimeZoneId = model.TimeOffsetId,
                Email = model.Email,
                Password = model.Password,
            };

            try
            {
                await _tokenClient.RegisterAsync(user);
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
                _logger.LogError(ex, "Non-caught create user error.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userClient.FindByIdAsync(id, _tokenService.GetToken());
            if (user == null)
            {
                return NotFound();
            }

            var model = new AdminEditViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Money = user.Money,
                Name = user.FirstName,
                Surname = user.Surname,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new UserModel
            {
                Id = model.Id,
                Email = model.Email,
                Money = model.Money,
                FirstName = model.Name,
                Surname = model.Surname,
            };

            try
            {
                await _userClient.UpdateAsync(user, _tokenService.GetToken());
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
                _logger.LogError(ex, "Non-caught edit user error.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Block(int id)
        {
            try
            {
                await _userClient.BanAsync(id, _tokenService.GetToken());
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                string errorMessage = "The user is not found.";
                _logger.LogInformation(ex, errorMessage);
                ModelState.AddModelError("", errorMessage);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Non-caught block user error.");
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Unblock(int id)
        {
            try
            {
                await _userClient.UnbanAsync(id, _tokenService.GetToken());
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                string errorMessage = "The user is not found.";
                _logger.LogInformation(ex, errorMessage);
                ModelState.AddModelError("", errorMessage);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Non-caught block user error.");
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}