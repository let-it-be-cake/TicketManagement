using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.UserInterface.Clients.EventApi;
using TicketManagement.UserInterface.Clients.UserApi;
using TicketManagement.UserInterface.Helper;
using TicketManagement.UserInterface.Models;
using TicketManagement.UserInterface.Services;

namespace TicketManagement.UserInterface.Controllers
{
    public class TicketOfficeController : Controller
    {
        private readonly IEventGetterClient _eventGetterClient;
        private readonly IEventAreaClient _eventAreaClient;
        private readonly IEventSeatClient _eventSeatClient;

        private readonly IMapToViewModel _mapHelper;

        private readonly ICartTicket _cartTicektService;

        private readonly IPagingValidation _pagingValidation;

        public TicketOfficeController(IEventGetterClient eventGetterClient,
                                      IEventAreaClient eventAreaClient,
                                      IEventSeatClient eventSeatClient,
                                      IMapToViewModel mapHelper,
                                      ICartTicket cartTicektService,
                                      IPagingValidation pagingValidation)
        {
            _eventGetterClient = eventGetterClient;
            _eventAreaClient = eventAreaClient;
            _eventSeatClient = eventSeatClient;
            _mapHelper = mapHelper;
            _cartTicektService = cartTicektService;
            _pagingValidation = pagingValidation;
        }

        public async Task<IActionResult> Index(int from = 0, int howMany = 10)
        {
            howMany = _pagingValidation.MaxElementOnPage(howMany);
            var events = await _eventGetterClient.GetRegisterAsync(from, howMany);

            TimeSpan offset = TimeSpan.Zero;

            if (User.Identity.IsAuthenticated)
            {
                string timeZoneId = User.GetClaim(nameof(Entities.Identity.User.TimeZoneId));
                offset = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId).BaseUtcOffset;
            }

            return View(_mapHelper.EventToViewModel(offset, events));
        }

        [HttpPost]
        public async Task<IActionResult> EventArea(int id)
        {
            var eventAreas = await _eventAreaClient.GetEventAreasAsync(id);
            return View(_mapHelper.EventAreaToViewModel(eventAreas));
        }

        [HttpPost]
        public async Task<IActionResult> EventSeat(int id)
        {
            var eventSeats = await _eventSeatClient.GetFromEventAreaAsync(id);
            return View(_mapHelper.EventSeatToViewModel(eventSeats));
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public void AddToCart(int id)
        {
            _cartTicektService.AddToCart(id);
        }
    }
}
