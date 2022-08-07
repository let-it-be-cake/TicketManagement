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
    [Route("seat")]
    public class SeatController : Controller
    {
        private readonly IProxyService<Seat> _seatProxy;

        public SeatController(IProxyService<Seat> seatProxy)
        {
            _seatProxy = seatProxy;
        }

        /// <summary>
        /// Add new seat.
        /// </summary>
        /// <param name="seat">Seat to add.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(Seat @seat)
        {
            try
            {
                await _seatProxy.AddAsync(@seat);
                return CreatedAtAction(nameof(Get), new { id = @seat.Id }, @seat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get seat from id.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Seat), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var @seat = await _seatProxy.ReadAsync(id);
            IActionResult result = @seat is null ? NotFound() : Ok(@seat);
            return result;
        }

        /// <summary>
        /// Get all seats.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<Seat>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var seats = await _seatProxy.ReadAllAsync();
            return Ok(seats);
        }

        /// <summary>
        /// Update seat.
        /// </summary>
        /// <param name="seat">Seat to update. The object with the set id will be updated.</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Seat @seat)
        {
            try
            {
                await _seatProxy.ChangeAsync(@seat);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete seat from id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _seatProxy.DeleteAsync(id);
            return NoContent();
        }
    }
}
