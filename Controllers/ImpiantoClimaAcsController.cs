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
    public class ImpiantoClimaAcsController : ControllerBase
    {
        private readonly IImpiantoClimaAcsService _impiantoClimaAcsService;

        public ImpiantoClimaAcsController(IImpiantoClimaAcsService impiantoClimaAcsService)
        {
            _impiantoClimaAcsService = impiantoClimaAcsService;
        }

        /// <summary>
        /// Ottiene tutti gli impianti di climatizzazione e ACS
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ImpiantoClimaAcs>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var impianti = await _impiantoClimaAcsService.GetAllAsync();
            return Ok(impianti);
        }

        /// <summary>
        /// Ottiene un impianto di climatizzazione e ACS specifico tramite ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ImpiantoClimaAcs), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var impianto = await _impiantoClimaAcsService.GetByIdAsync(id);
            if (impianto == null)
                return NotFound();

            return Ok(impianto);
        }

        /// <summary>
        /// Crea un nuovo impianto di climatizzazione e ACS
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ImpiantoClimaAcs), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ImpiantoClimaAcs impianto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _impiantoClimaAcsService.CreateAsync(impianto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Aggiorna un impianto di climatizzazione e ACS esistente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ImpiantoClimaAcs), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] ImpiantoClimaAcs impianto)
        {
            if (id != impianto.Id)
                return BadRequest("L'ID nel path non corrisponde all'ID nell'oggetto");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingImpianto = await _impiantoClimaAcsService.GetByIdAsync(id);
            if (existingImpianto == null)
                return NotFound();

            var result = await _impiantoClimaAcsService.UpdateAsync(id, impianto);
            return Ok(result);
        }

        /// <summary>
        /// Elimina un impianto di climatizzazione e ACS
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingImpianto = await _impiantoClimaAcsService.GetByIdAsync(id);
            if (existingImpianto == null)
                return NotFound();

            await _impiantoClimaAcsService.DeleteAsync(id);
            return NoContent();
        }
    }
}