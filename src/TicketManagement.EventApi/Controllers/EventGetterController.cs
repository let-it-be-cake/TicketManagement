using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Entities.Tables;
using TicketManagement.EventApi.Proxys;

namespace TicketManagement.EventApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("event-getter")]
    public class EventGetterController : Controller
    {
        private readonly IGetEvents _getEvent;

        public EventGetterController(IGetEvents getEvent)
        {
            _getEvent = getEvent;
        }

        /// <summary>
        /// Get event from id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEvent(int id)
        {
            Event @event = await _getEvent.GetEventAsync(id);
            IActionResult result = @event is null ? NotFound() : Ok(@event);
            return result;
        }

        /// <summary>
        /// Get registered events.
        /// </summary>
        /// <param name="from">What number should I get events from.</param>
        /// <param name="howMany">By which number to receive events.</param>
        [HttpGet("register/{from}&{howMany}")]
        [ProducesResponseType(typeof(List<Event>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRegisterEvents(int from, int howMany)
        {
            try
            {
                List<Event> events = await _getEvent.GetRegisterEventsAsync(from, howMany);
                return Ok(events);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get unregistered events.
        /// </summary>
        /// <param name="from">What number should I get events from.</param>
        /// <param name="howMany">By which number to receive events.</param>
        [HttpGet("unregister/{from}&{howMany}")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(List<Event>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUnregisterEvents(int from, int howMany)
        {
            try
            {
                List<Event> events = await _getEvent.GetUnregisterEventsAsync(from, howMany);
                return Ok(events);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
