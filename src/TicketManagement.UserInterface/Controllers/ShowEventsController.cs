using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Entities.Tables;
using TicketManagement.UserInterface.Clients.EventApi;
using TicketManagement.UserInterface.Helper;
using TicketManagement.UserInterface.Services;

namespace TicketManagement.UserInterface.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class ShowEventsController : Controller
    {
        private readonly IEventGetterClient _eventGetterClient;

        private readonly IMapToViewModel _mapHelper;

        private readonly IPagingValidation _pagingValidation;

        private readonly ITokenService _tokenService;

        public ShowEventsController(IEventGetterClient eventGetterClient,
                                    IMapToViewModel mapHelper,
                                    IPagingValidation pagingValidation,
                                    ITokenService tokenService)
        {
            _eventGetterClient = eventGetterClient;
            _mapHelper = mapHelper;
            _pagingValidation = pagingValidation;
            _tokenService = tokenService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Activated(int from = 0, int howMany = 10)
        {
            howMany = _pagingValidation.MaxElementOnPage(howMany);
            List<Event> registerEvents = await _eventGetterClient.GetRegisterAsync(from, howMany);

            string timeZoneId = User.GetClaim(nameof(Entities.Identity.User.TimeZoneId));
            TimeSpan offset = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId).BaseUtcOffset;

            return PartialView("_ShowEventTable", _mapHelper.EventToViewModel(offset, registerEvents));
        }

        [HttpPost]
        public async Task<IActionResult> NotActivated(int from = 0, int howMany = 10)
        {
            howMany = _pagingValidation.MaxElementOnPage(howMany);
            List<Event> unregisterEvents = await _eventGetterClient.GetUnregisterAsync(from, howMany, _tokenService.GetToken());

            string timeZoneId = User.GetClaim(nameof(Entities.Identity.User.TimeZoneId));
            TimeSpan offset = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId).BaseUtcOffset;

            return PartialView("_ShowEventTable", _mapHelper.EventToViewModel(offset, unregisterEvents));
        }
    }
}