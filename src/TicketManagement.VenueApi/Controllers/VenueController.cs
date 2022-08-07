using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Entities.Tables;
using TicketManagement.VenueApi.Proxys;

namespace TicketManagement.VenueApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin, VenueManager")]
    [Route("venue")]
    public class VenueController : Controller
    {
        private readonly IProxyService<Venue> _venueProxy;

        public VenueController(IProxyService<Venue> venueProxy)
        {
            _venueProxy = venueProxy;
        }

        /// <summary>
        /// Add new venue.
        /// </summary>
        /// <param name="venue">Venue to add.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(Venue @venue)
        {
            try
            {
                await _venueProxy.AddAsync(@venue);
                return CreatedAtAction(nameof(Get), new { id = @venue.Id }, @venue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get venue from id.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Venue), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var @venue = await _venueProxy.ReadAsync(id);
            IActionResult result = @venue is null ? NotFound() : Ok(@venue);
            return result;
        }

        /// <summary>
        /// Get all venues.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<Venue>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var venues = await _venueProxy.ReadAllAsync();
            return Ok(venues);
        }

        /// <summary>
        /// Update venue.
        /// </summary>
        /// <param name="venue">Venue to update. The object with the set id will be updated.</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Venue @venue)
        {
            try
            {
                await _venueProxy.ChangeAsync(@venue);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete venue from id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _venueProxy.DeleteAsync(id);
            return NoContent();
        }
    }
}
