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
    public class IdraulicoAdduzioneController : ControllerBase
    {
        private readonly IIdraulicoAdduzioneService _idraulicoAdduzioneService;

        public IdraulicoAdduzioneController(IIdraulicoAdduzioneService idraulicoAdduzioneService)
        {
            _idraulicoAdduzioneService = idraulicoAdduzioneService;
        }

        /// <summary>
        /// Ottiene tutti gli impianti idraulici di adduzione
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IdraulicoAdduzione>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var impianti = await _idraulicoAdduzioneService.GetAllAsync();
            return Ok(impianti);
        }

        /// <summary>
        /// Ottiene un impianto idraulico di adduzione specifico tramite ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IdraulicoAdduzione), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var impianto = await _idraulicoAdduzioneService.GetByIdAsync(id);
            if (impianto == null)
                return NotFound();

            return Ok(impianto);
        }

        /// <summary>
        /// Crea un nuovo impianto idraulico di adduzione
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(IdraulicoAdduzione), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] IdraulicoAdduzione impianto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _idraulicoAdduzioneService.CreateAsync(impianto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Aggiorna un impianto idraulico di adduzione esistente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IdraulicoAdduzione), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] IdraulicoAdduzione impianto)
        {
            if (id != impianto.Id)
                return BadRequest("L'ID nel path non corrisponde all'ID nell'oggetto");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingImpianto = await _idraulicoAdduzioneService.GetByIdAsync(id);
            if (existingImpianto == null)
                return NotFound();

            var result = await _idraulicoAdduzioneService.UpdateAsync(id, impianto);
            return Ok(result);
        }

        /// <summary>
        /// Elimina un impianto idraulico di adduzione
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingImpianto = await _idraulicoAdduzioneService.GetByIdAsync(id);
            if (existingImpianto == null)
                return NotFound();

            await _idraulicoAdduzioneService.DeleteAsync(id);
            return NoContent();
        }
    }
}