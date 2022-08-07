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
    [Route("layout")]
    public class LayoutController : Controller
    {
        private readonly IProxyService<Layout> _layoutProxy;

        public LayoutController(IProxyService<Layout> layoutProxy)
        {
            _layoutProxy = layoutProxy;
        }

        /// <summary>
        /// Add new layout.
        /// </summary>
        /// <param name="layout">Layout to add.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(Layout @layout)
        {
            try
            {
                await _layoutProxy.AddAsync(@layout);
                return CreatedAtAction(nameof(Get), new { id = @layout.Id }, @layout);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get layout from id.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Layout), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var @layout = await _layoutProxy.ReadAsync(id);
            IActionResult result = @layout is null ? NotFound() : Ok(@layout);
            return result;
        }

        /// <summary>
        /// Get all layouts.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<Layout>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var layouts = await _layoutProxy.ReadAllAsync();
            return Ok(layouts);
        }

        /// <summary>
        /// Update layout.
        /// </summary>
        /// <param name="layout">Layout to update. The object with the set id will be updated.</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Layout @layout)
        {
            try
            {
                await _layoutProxy.ChangeAsync(@layout);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete layout from id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _layoutProxy.DeleteAsync(id);
            return NoContent();
        }
    }
}
