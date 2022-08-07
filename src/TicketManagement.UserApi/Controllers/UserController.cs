using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketManagement.UserApi.Database.Identity;
using TicketManagement.UserApi.Exceptions;
using TicketManagement.UserApi.Models;
using TicketManagement.UserApi.Services;

namespace TicketManagement.UserApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        private readonly IJwtTokenService _jwtTokenService;

        private readonly ILogger<UsersController> _logger;

        public UsersController(SignInManager<User> signInManager,
                               UserManager<User> userManager,
                               IJwtTokenService jwtTokenService,
                               ILogger<UsersController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                LogModelStateErrors();
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                Surname = model.Surname,
                TimeZoneId = model.TimeZoneId,
                Language = model.Language,
                IsBlocked = false,
                Money = 0,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                var roles = await _userManager.GetRolesAsync(user);
                return Ok(_jwtTokenService.GenerateToken(user, roles));
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Login into the new API.
        /// </summary>
        /// <remarks>
        /// Here is the code sample of usage.
        /// </remarks>
        /// <param name="model">Register data.</param>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                LogModelStateErrors();
                return BadRequest();
            }

            User user = await _userManager.FindByNameAsync(model.Email);

            if (user is null)
            {
                return NotFound();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                return NotFound();
            }

            if (user.IsBlocked)
            {
                return Forbid();
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(_jwtTokenService.GenerateToken(user, roles));
        }

        [HttpGet("validate")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Validate(string token)
        {
            return _jwtTokenService.ValidateToken(token) ? Ok() : Forbid();
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UserModel model)
        {
            User user = await _userManager.FindByIdAsync(model.Id.ToString());

            if (user is null)
            {
                return BadRequest();
            }

            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.Surname = model.Surname;
            user.TimeZoneId = model.TimeZoneId;
            user.Language = model.Language;
            user.Money = model.Money.HasValue ? model.Money.Value : 0;

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(_jwtTokenService.GenerateToken(user, roles));
            }

            return BadRequest();
        }

        [HttpGet("find-by-id/{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FindById(int id)
        {
            User user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("find-by-name/{name}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FindByName(string name)
        {
            User user = await _userManager.FindByNameAsync(name);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut("password-reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordModel password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User user = await _userManager.FindByIdAsync(password.UserId.ToString());

            if (user is null)
            {
                return NotFound();
            }

            if (!await _userManager.CheckPasswordAsync(user, password.OldPassword))
            {
                return BadRequest();
            }

            IdentityResult result =
                await _userManager.ChangePasswordAsync(user, password.OldPassword, password.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("password-validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PasswordValidate([FromBody] PasswordValidateModel password)
        {
            User user = await _userManager.FindByIdAsync(password.UserId.ToString());

            bool valid = await _userManager.CheckPasswordAsync(user, password.Password);

            if (valid)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("get-role-users/{role}")]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRoleUsers(string role)
        {
            if (role == null)
            {
                return BadRequest();
            }

            List<User> users = (await _userManager.GetUsersInRoleAsync(role)).ToList();

            return Ok(users);
        }

        [HttpGet("get-all-users")]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> users = await _userManager.Users.ToListAsync();

            return Ok(users);
        }

        [HttpDelete("ban/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BanUser(int id)
        {
            try
            {
                await SetUserBlock(id, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(BanUser)} exception.");
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("unban/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnbanUser(int id)
        {
            try
            {
                await SetUserBlock(id, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UnbanUser)} exception.");
                return NotFound();
            }

            return NoContent();
        }

        private async Task SetUserBlock(int id, bool block)
        {
            User user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
            {
                throw new UserNotFoundException($"User with id {id} not found.");
            }

            user.IsBlocked = block;

            await _userManager.UpdateAsync(user);
        }

        private void LogModelStateErrors()
        {
            foreach (ModelStateEntry modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    _logger.LogWarning(error.Exception, error.ErrorMessage);
                }
            }
        }
    }
}