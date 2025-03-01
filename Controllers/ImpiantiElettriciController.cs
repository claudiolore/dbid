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
    public class ImpiantiElettriciController : ControllerBase
    {
        private readonly IImpiantiElettriciService _impiantiElettriciService;

        public ImpiantiElettriciController(IImpiantiElettriciService impiantiElettriciService)
        {
            _impiantiElettriciService = impiantiElettriciService;
        }

        /// <summary>
        /// Ottiene tutti gli impianti elettrici
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ImpiantiElettrici>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var impianti = await _impiantiElettriciService.GetAllAsync();
            return Ok(impianti);
        }

        /// <summary>
        /// Ottiene un impianto elettrico specifico tramite ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ImpiantiElettrici), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var impianto = await _impiantiElettriciService.GetByIdAsync(id);
            if (impianto == null)
                return NotFound();

            return Ok(impianto);
        }

        /// <summary>
        /// Crea un nuovo impianto elettrico
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ImpiantiElettrici), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ImpiantiElettrici impianto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _impiantiElettriciService.CreateAsync(impianto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Aggiorna un impianto elettrico esistente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ImpiantiElettrici), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] ImpiantiElettrici impianto)
        {
            if (id != impianto.Id)
                return BadRequest("L'ID nel path non corrisponde all'ID nell'oggetto");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingImpianto = await _impiantiElettriciService.GetByIdAsync(id);
            if (existingImpianto == null)
                return NotFound();

            var result = await _impiantiElettriciService.UpdateAsync(id, impianto);
            return Ok(result);
        }

        /// <summary>
        /// Elimina un impianto elettrico
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingImpianto = await _impiantiElettriciService.GetByIdAsync(id);
            if (existingImpianto == null)
                return NotFound();

            await _impiantiElettriciService.DeleteAsync(id);
            return NoContent();
        }
    }
}