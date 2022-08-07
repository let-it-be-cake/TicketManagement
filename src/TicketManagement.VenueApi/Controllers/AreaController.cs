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
    [Route("area")]
    public class AreaController : Controller
    {
        private readonly IProxyService<Area> _areaProxy;

        public AreaController(IProxyService<Area> areaProxy)
        {
            _areaProxy = areaProxy;
        }

        /// <summary>
        /// Add new area.
        /// </summary>
        /// <param name="area">Area to add.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(Area @area)
        {
            try
            {
                await _areaProxy.AddAsync(@area);
                return CreatedAtAction(nameof(Get), new { id = @area.Id }, @area);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get area from id.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Area), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var @area = await _areaProxy.ReadAsync(id);
            IActionResult result = @area is null ? NotFound() : Ok(@area);
            return result;
        }

        /// <summary>
        /// Get all areas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<Area>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var areas = await _areaProxy.ReadAllAsync();
            return Ok(areas);
        }

        /// <summary>
        /// Update area.
        /// </summary>
        /// <param name="area">Area to update. The object with the set id will be updated.</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Area @area)
        {
            try
            {
                await _areaProxy.ChangeAsync(@area);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete area from id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _areaProxy.DeleteAsync(id);
            return NoContent();
        }
    }
}
