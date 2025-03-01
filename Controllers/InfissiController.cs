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
    public class InfissiController : ControllerBase
    {
        private readonly IInfissiService _infissiService;

        public InfissiController(IInfissiService infissiService)
        {
            _infissiService = infissiService;
        }

        /// <summary>
        /// Ottiene tutti gli infissi
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Infissi>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var infissi = await _infissiService.GetAllAsync();
            return Ok(infissi);
        }

        /// <summary>
        /// Ottiene un infisso specifico tramite ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Infissi), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var infisso = await _infissiService.GetByIdAsync(id);
            if (infisso == null)
                return NotFound();

            return Ok(infisso);
        }

        /// <summary>
        /// Crea un nuovo infisso
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Infissi), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Infissi infisso)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _infissiService.CreateAsync(infisso);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Aggiorna un infisso esistente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Infissi), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] Infissi infisso)
        {
            if (id != infisso.Id)
                return BadRequest("L'ID nel path non corrisponde all'ID nell'oggetto");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingInfisso = await _infissiService.GetByIdAsync(id);
            if (existingInfisso == null)
                return NotFound();

            var result = await _infissiService.UpdateAsync(id, infisso);
            return Ok(result);
        }

        /// <summary>
        /// Elimina un infisso
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingInfisso = await _infissiService.GetByIdAsync(id);
            if (existingInfisso == null)
                return NotFound();

            await _infissiService.DeleteAsync(id);
            return NoContent();
        }
    }
}