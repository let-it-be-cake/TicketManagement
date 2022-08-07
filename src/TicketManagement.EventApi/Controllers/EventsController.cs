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
    [Route("events")]
    [Authorize(Roles = "Admin, Manager")]
    public class EventsController : Controller
    {
        private readonly IEventProxyService _eventProxy;

        public EventsController(IEventProxyService eventProxy)
        {
            _eventProxy = eventProxy;
        }

        /// <summary>
        /// Add new event.
        /// </summary>
        /// <param name="event">Event to add.</param>
        [HttpPost]
        [ProducesResponseType(typeof(Event), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(Event @event)
        {
            try
            {
                await _eventProxy.AddAsync(@event);
                return CreatedAtAction(nameof(Get), new { id = @event.Id }, @event);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get event from id.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var @event = await _eventProxy.ReadAsync(id);
            IActionResult result = @event is null ? NotFound() : Ok(@event);
            return result;
        }

        /// <summary>
        /// Get all events.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<Event>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventProxy.ReadAllAsync();
            return Ok(events);
        }

        /// <summary>
        /// Update event.
        /// </summary>
        /// <param name="event">Event to update. The object with the set id will be updated.</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Event @event)
        {
            try
            {
                await _eventProxy.ChangeAsync(@event);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete event from id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _eventProxy.DeleteAsync(id);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
