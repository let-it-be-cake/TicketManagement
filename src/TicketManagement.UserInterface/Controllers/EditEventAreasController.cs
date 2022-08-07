using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.UserInterface.Clients.EventApi;
using TicketManagement.UserInterface.Helper;
using TicketManagement.UserInterface.Models;
using TicketManagement.UserInterface.Models.ViewModels;
using TicketManagement.UserInterface.Services;

namespace TicketManagement.UserInterface.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class EditEventAreasController : Controller
    {
        private readonly IEventAreaClient _eventAreaClient;

        private readonly IMapToViewModel _mapHelper;

        private readonly ITokenService _tokenService;

        public EditEventAreasController(IEventAreaClient eventAreaClient,
                                        IMapToViewModel mapHelper,
                                        ITokenService tokenService)
        {
            _eventAreaClient = eventAreaClient;
            _mapHelper = mapHelper;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int from = 0, int howMany = 10)
        {
            var eventAreas = await _eventAreaClient.GetUnregisterAsync(from, howMany, _tokenService.GetToken());

            string timeZoneId = User.GetClaim(nameof(Entities.Identity.User.TimeZoneId));
            TimeSpan offset = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId).BaseUtcOffset;

            return View(_mapHelper.EventAreaModelToViewModel(offset, eventAreas));
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] IEnumerable<EventAreaPriceViewModel> areaPrices)
        {
            if (areaPrices == null)
            {
                return BadRequest();
            }

            var areaPricesModel = new List<EventAreaPriceModel>();

            foreach (var areaPrice in areaPrices)
            {
                if (!areaPrice.Price.HasValue)
                {
                    continue;
                }

                areaPricesModel.Add(new EventAreaPriceModel
                {
                    EventAreaId = areaPrice.EventAreaId,
                    Price = areaPrice.Price.Value,
                });
            }

            await _eventAreaClient.SetPriceAsync(areaPricesModel, _tokenService.GetToken());

            return RedirectToAction("Index");
        }
    }
}
