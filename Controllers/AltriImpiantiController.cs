using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AltriImpiantiController : ControllerBase
    {
        private readonly IAltriImpiantiService _altriImpiantiService;

        public AltriImpiantiController(IAltriImpiantiService altriImpiantiService)
        {
            _altriImpiantiService = altriImpiantiService;
        }

        /// <summary>
        /// Ottiene tutti gli altri impianti
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AltriImpianti>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var impianti = await _altriImpiantiService.GetAllAsync();
            return Ok(impianti);
        }

        /// <summary>
        /// Ottiene un altro impianto specifico tramite ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AltriImpianti), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var impianto = await _altriImpiantiService.GetByIdAsync(id);
            if (impianto == null)
                return NotFound();

            return Ok(impianto);
        }

        /// <summary>
        /// Crea un nuovo altro impianto
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AltriImpianti), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] AltriImpianti impianto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _altriImpiantiService.CreateAsync(impianto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Aggiorna un altro impianto esistente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AltriImpianti), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] AltriImpianti impianto)
        {
            if (id != impianto.Id)
                return BadRequest("L'ID nel path non corrisponde all'ID nell'oggetto");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingImpianto = await _altriImpiantiService.GetByIdAsync(id);
            if (existingImpianto == null)
                return NotFound();

            var result = await _altriImpiantiService.UpdateAsync(id, impianto);
            return Ok(result);
        }

        /// <summary>
        /// Elimina un altro impianto
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingImpianto = await _altriImpiantiService.GetByIdAsync(id);
            if (existingImpianto == null)
                return NotFound();

            await _altriImpiantiService.DeleteAsync(id);
            return NoContent();
        }
    }
} 