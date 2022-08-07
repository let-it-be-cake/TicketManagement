using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;
using TicketManagement.PurchaseApi.Proxys;

namespace TicketManagement.PurchaseApi.Controllers
{
    [ApiController]
    [Route("ticket")]
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IRepository<EventSeat> _eventSeat;

        public TicketController(ITicketService ticketService,
                                IRepository<EventSeat> eventSeat)
        {
            _ticketService = ticketService;
            _eventSeat = eventSeat;
        }

        [HttpPost("buy/{userId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BuyTicket(int userId, [FromBody] IEnumerable<int> eventSeatsId)
        {
            var dict = new Dictionary<int, List<EventSeat>>();
            foreach (int eventSeatId in eventSeatsId)
            {
                EventSeat eventSeat = await _eventSeat.GetAsync(eventSeatId);
                List<EventSeat> value = null;

                if (!dict.TryGetValue(eventSeat.EventAreaId, out value))
                {
                    dict[eventSeat.EventAreaId] = new List<EventSeat>();
                    value = dict[eventSeat.EventAreaId];
                }

                value.Add(eventSeat);
            }

            try
            {
                foreach (KeyValuePair<int, List<EventSeat>> keyValue in dict)
                {
                    await _ticketService.BuyTicketAsync(userId, keyValue.Value);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet("get-user-tickets/{userId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<Ticket>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserTickets(int userId)
        {
            try
            {
                List<Ticket> eventSeats = await _ticketService.GetUserTicektsAsync(userId);

                return Ok(eventSeats);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
