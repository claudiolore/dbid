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
    public class UnitaImmobiliareController : ControllerBase
    {
        private readonly IUnitaImmobiliareService _unitaImmobiliareService;

        public UnitaImmobiliareController(IUnitaImmobiliareService unitaImmobiliareService)
        {
            _unitaImmobiliareService = unitaImmobiliareService;
        }

        /// <summary>
        /// Ottiene tutte le unità immobiliari
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UnitaImmobiliare>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var unitaImmobiliari = await _unitaImmobiliareService.GetAllAsync();
            return Ok(unitaImmobiliari);
        }

        /// <summary>
        /// Ottiene un'unità immobiliare specifica tramite ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UnitaImmobiliare), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var unitaImmobiliare = await _unitaImmobiliareService.GetByIdAsync(id);
            if (unitaImmobiliare == null)
                return NotFound();

            return Ok(unitaImmobiliare);
        }

        /// <summary>
        /// Crea una nuova unità immobiliare
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(UnitaImmobiliare), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] UnitaImmobiliare unitaImmobiliare)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _unitaImmobiliareService.CreateAsync(unitaImmobiliare);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Aggiorna un'unità immobiliare esistente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UnitaImmobiliare), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UnitaImmobiliare unitaImmobiliare)
        {
            if (id != unitaImmobiliare.Id)
                return BadRequest("L'ID nel path non corrisponde all'ID nell'oggetto");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUnitaImmobiliare = await _unitaImmobiliareService.GetByIdAsync(id);
            if (existingUnitaImmobiliare == null)
                return NotFound();

            var result = await _unitaImmobiliareService.UpdateAsync(id, unitaImmobiliare);
            return Ok(result);
        }

        /// <summary>
        /// Elimina un'unità immobiliare
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingUnitaImmobiliare = await _unitaImmobiliareService.GetByIdAsync(id);
            if (existingUnitaImmobiliare == null)
                return NotFound();

            await _unitaImmobiliareService.DeleteAsync(id);
            return NoContent();
        }
    }
}