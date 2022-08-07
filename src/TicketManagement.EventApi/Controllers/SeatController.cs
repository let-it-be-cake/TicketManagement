using System.Collections.Generic;
using System.Linq;
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
    [Route("seat")]
    public class SeatController : Controller
    {
        private readonly IGetEventSeat _eventSeats;

        public SeatController(IGetEventSeat eventSeats)
        {
            _eventSeats = eventSeats;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventSeat), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            EventSeat eventSeat = await _eventSeats.GetAsync(id);

            return eventSeat is null ? NotFound() : Ok(eventSeat);
        }

        [HttpGet("get-area-seats/{eventAreaId}")]
        [ProducesResponseType(typeof(List<EventSeat>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFromEventAreaAsync(int eventAreaId)
        {
            List<EventSeat> eventSeats = await _eventSeats.GetFromEventAreaAsync(eventAreaId);

            return eventSeats.Any() ? Ok(eventSeats) : NotFound();
        }

        [HttpGet("get-ticket-seats/{ticketId}")]
        [ProducesResponseType(typeof(List<EventSeat>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventSeatsFromTicketAsync(int ticketId)
        {
            List<EventSeat> eventSeats = await _eventSeats.GetFromEventAreaAsync(ticketId);

            return eventSeats.Any() ? Ok(eventSeats) : NotFound();
        }
    }
}
