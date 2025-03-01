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
    public class EdificioController : ControllerBase
    {
        private readonly IEdificioService _edificioService;

        public EdificioController(IEdificioService edificioService)
        {
            _edificioService = edificioService;
        }

        /// <summary>
        /// Ottiene tutti gli edifici
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Edificio>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var edifici = await _edificioService.GetAllAsync();
            return Ok(edifici);
        }

        /// <summary>
        /// Ottiene un edificio specifico tramite ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Edificio), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var edificio = await _edificioService.GetByIdAsync(id);
            if (edificio == null)
                return NotFound();

            return Ok(edificio);
        }

        /// <summary>
        /// Crea un nuovo edificio
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Edificio), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Edificio edificio)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _edificioService.CreateAsync(edificio);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Aggiorna un edificio esistente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Edificio), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] Edificio edificio)
        {
            if (id != edificio.Id)
                return BadRequest("L'ID nel path non corrisponde all'ID nell'oggetto");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingEdificio = await _edificioService.GetByIdAsync(id);
            if (existingEdificio == null)
                return NotFound();

            var result = await _edificioService.UpdateAsync(id, edificio);
            return Ok(result);
        }

        /// <summary>
        /// Elimina un edificio
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingEdificio = await _edificioService.GetByIdAsync(id);
            if (existingEdificio == null)
                return NotFound();

            await _edificioService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Ottiene le unità immobiliari associate a un edificio
        /// </summary>
        [HttpGet("{id}/unitaimmobiliari")]
        [ProducesResponseType(typeof(IEnumerable<UnitaImmobiliare>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUnitaImmobiliariByEdificioId(Guid id)
        {
            var existingEdificio = await _edificioService.GetByIdAsync(id);
            if (existingEdificio == null)
                return NotFound();

            var unitaImmobiliari = await _edificioService.GetUnitaImmobiliariByEdificioIdAsync(id);
            return Ok(unitaImmobiliari);
        }
    }
} 