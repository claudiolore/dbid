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
    public class ScarichiIdriciFognariController : ControllerBase
    {
        private readonly IScarichiIdriciFognariService _scarichiIdriciFognariService;

        public ScarichiIdriciFognariController(IScarichiIdriciFognariService scarichiIdriciFognariService)
        {
            _scarichiIdriciFognariService = scarichiIdriciFognariService;
        }

        /// <summary>
        /// Ottiene tutti gli scarichi idrici e fognari
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ScarichiIdriciFognari>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var scarichi = await _scarichiIdriciFognariService.GetAllAsync();
            return Ok(scarichi);
        }

        /// <summary>
        /// Ottiene uno scarico idrico e fognario specifico tramite ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ScarichiIdriciFognari), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var scarico = await _scarichiIdriciFognariService.GetByIdAsync(id);
            if (scarico == null)
                return NotFound();

            return Ok(scarico);
        }

        /// <summary>
        /// Crea un nuovo scarico idrico e fognario
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ScarichiIdriciFognari), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ScarichiIdriciFognari scarico)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _scarichiIdriciFognariService.CreateAsync(scarico);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Aggiorna uno scarico idrico e fognario esistente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ScarichiIdriciFognari), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] ScarichiIdriciFognari scarico)
        {
            if (id != scarico.Id)
                return BadRequest("L'ID nel path non corrisponde all'ID nell'oggetto");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingScarico = await _scarichiIdriciFognariService.GetByIdAsync(id);
            if (existingScarico == null)
                return NotFound();

            var result = await _scarichiIdriciFognariService.UpdateAsync(id, scarico);
            return Ok(result);
        }

        /// <summary>
        /// Elimina uno scarico idrico e fognario
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingScarico = await _scarichiIdriciFognariService.GetByIdAsync(id);
            if (existingScarico == null)
                return NotFound();

            await _scarichiIdriciFognariService.DeleteAsync(id);
            return NoContent();
        }
    }
}