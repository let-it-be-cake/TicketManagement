using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Entities.Tables;
using TicketManagement.UserInterface.Clients.EventApi;
using TicketManagement.UserInterface.Clients.PurchaseApi;
using TicketManagement.UserInterface.Helper;
using TicketManagement.UserInterface.Models.ViewModels;
using TicketManagement.UserInterface.Services;

namespace TicketManagement.UserInterface.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class CartController : Controller
    {
        private readonly IPurchaseClient _purchaseClient;
        private readonly IEventSeatClient _eventSeatClient;
        private readonly IEventAreaClient _eventAreaClient;
        private readonly IEventClient _eventClient;

        private readonly ICartTicket _cartService;

        private readonly ITokenService _tokenService;

        public CartController(IPurchaseClient purchaseClient,
                              IEventSeatClient eventSeatClient,
                              IEventAreaClient eventAreaClient,
                              IEventClient eventClient,
                              ICartTicket cartService,
                              ITokenService tokenService)
        {
            _purchaseClient = purchaseClient;
            _eventSeatClient = eventSeatClient;
            _eventAreaClient = eventAreaClient;
            _eventClient = eventClient;
            _cartService = cartService;
            _tokenService = tokenService;
        }

        public async Task<IActionResult> Index()
        {
            List<int> eventSeatsIds = _cartService.GetAllFromCart();

            string timeZoneId = User.GetClaim(nameof(Entities.Identity.User.TimeZoneId));

            TimeSpan offset = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId).BaseUtcOffset;

            return View(await GenerateCartViewModel(offset, eventSeatsIds));
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Buy()
        {
            var dict = new Dictionary<int, List<int>>();
            var userCart = _cartService.GetAllFromCart();

            foreach (int eventSeatId in userCart)
            {
                EventSeat eventSeat = await _eventSeatClient.GetAsync(eventSeatId);
                List<int> value = null;

                if (!dict.TryGetValue(eventSeat.EventAreaId, out value))
                {
                    dict[eventSeat.EventAreaId] = new List<int>();
                    value = dict[eventSeat.EventAreaId];
                }

                value.Add(eventSeatId);
            }

            try
            {
                foreach (KeyValuePair<int, List<int>> keyValue in dict)
                {
                    int userId = int.Parse(User.GetClaim(nameof(Entities.Identity.User.Id)));

                    await _purchaseClient.BuyTickets(userId, keyValue.Value, _tokenService.GetToken());
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            _cartService.ClearCart();
            return RedirectToAction("Index", "Home");
        }

        private async Task<List<CartViewModel>> GenerateCartViewModel(TimeSpan offset, IEnumerable<int> eventSeatIds)
        {
            var eventSeatsViews = new List<CartViewModel>();

            var eventAreaDict = new Dictionary<int, EventArea>();
            var eventDict = new Dictionary<int, Event>();

            foreach (var eventSeatId in eventSeatIds)
            {
                EventSeat eventSeat = await _eventSeatClient.GetAsync(eventSeatId);

                EventArea eventArea = null;
                Event @event = null;

                if (!eventAreaDict.TryGetValue(eventSeat.EventAreaId, out eventArea))
                {
                    eventAreaDict[eventSeat.EventAreaId] =
                        await _eventAreaClient.GetAsync(eventSeat.EventAreaId, _tokenService.GetToken());
                    eventArea = eventAreaDict[eventSeat.EventAreaId];
                }

                if (!eventDict.TryGetValue(eventArea.EventId, out @event))
                {
                    eventDict[eventArea.EventId] = await _eventClient.GetAsync(eventArea.EventId);
                    @event = eventDict[eventArea.EventId];
                }

                eventSeatsViews.Add(new CartViewModel
                {
                    Id = eventSeat.Id,
                    Name = @event.Name,
                    Description = eventArea.Description,
                    StartEventDate = @event.DateTimeStart + offset,
                    EndEventDate = @event.DateTimeEnd + offset,
                    ImageUrl = @event.ImageUrl,
                    Price = eventArea.Price.Value,
                    Number = eventSeat.Number,
                    Row = eventSeat.Row,
                });
            }

            return eventSeatsViews;
        }
    }
}
