using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Entities.Tables;
using TicketManagement.EventApi.Models;
using TicketManagement.EventApi.Proxys;

namespace TicketManagement.EventApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("area")]
    public class AreaController : Controller
    {
        private readonly IEventAreaProxy _eventAreaProxy;

        public AreaController(IEventAreaProxy eventAreaProxy)
        {
            _eventAreaProxy = eventAreaProxy;
        }

        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(EventArea), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventAreaAsync(int id)
        {
            EventArea eventArea = await _eventAreaProxy.GetEventAreaAsync(id);

            return eventArea is null ? NotFound() : Ok(eventArea);
        }

        [HttpGet("get-event-areas/{eventId}")]
        [ProducesResponseType(typeof(List<EventArea>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllFromEventAsync(int eventId)
        {
            List<EventArea> eventAreas = await _eventAreaProxy.GetAllFromEventAsync(eventId);

            return eventAreas.Any() ? Ok(eventAreas) : NotFound();
        }

        [HttpGet("get-unregister/{from}&{howMany}")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(List<EventAreaModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUnregisterModelAsync(int from, int howMany)
        {
            try
            {
                List<EventAreaModel> eventAreaModels = await _eventAreaProxy.GetUnregisterModelAsync(from, howMany);
                return eventAreaModels.Any() ? Ok(eventAreaModels) : NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("set-price")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SetPriceAsync([FromBody] IEnumerable<EventAreaPriceModel> eventAreaPrices)
        {
            await _eventAreaProxy.SetPriceAsync(eventAreaPrices);

            return Ok();
        }
    }
}
