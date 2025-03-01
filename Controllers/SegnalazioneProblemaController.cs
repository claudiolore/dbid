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
    public class SegnalazioneProblemaController : ControllerBase
    {
        private readonly ISegnalazioneProblemaService _segnalazioneProblemaService;

        public SegnalazioneProblemaController(ISegnalazioneProblemaService segnalazioneProblemaService)
        {
            _segnalazioneProblemaService = segnalazioneProblemaService;
        }

        /// <summary>
        /// Ottiene tutte le segnalazioni di problemi
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SegnalazioneProblema>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var segnalazioni = await _segnalazioneProblemaService.GetAllAsync();
            return Ok(segnalazioni);
        }

        /// <summary>
        /// Ottiene una segnalazione problema specifica tramite ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SegnalazioneProblema), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var segnalazione = await _segnalazioneProblemaService.GetByIdAsync(id);
            if (segnalazione == null)
                return NotFound();

            return Ok(segnalazione);
        }

        /// <summary>
        /// Ottiene le segnalazioni relative a una struttura
        /// </summary>
        [HttpGet("struttura/{strutturaId}")]
        [ProducesResponseType(typeof(IEnumerable<SegnalazioneProblema>), 200)]
        public async Task<IActionResult> GetByStrutturaId(Guid strutturaId)
        {
            var segnalazioni = await _segnalazioneProblemaService.GetByStrutturaIdAsync(strutturaId);
            return Ok(segnalazioni);
        }

        /// <summary>
        /// Crea una nuova segnalazione problema
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SegnalazioneProblema), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] SegnalazioneProblema segnalazione)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _segnalazioneProblemaService.CreateAsync(segnalazione);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Aggiorna una segnalazione problema esistente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SegnalazioneProblema), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] SegnalazioneProblema segnalazione)
        {
            if (id != segnalazione.Id)
                return BadRequest("L'ID nel path non corrisponde all'ID nell'oggetto");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingSegnalazione = await _segnalazioneProblemaService.GetByIdAsync(id);
            if (existingSegnalazione == null)
                return NotFound();

            var result = await _segnalazioneProblemaService.UpdateAsync(id, segnalazione);
            return Ok(result);
        }

        /// <summary>
        /// Elimina una segnalazione problema
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingSegnalazione = await _segnalazioneProblemaService.GetByIdAsync(id);
            if (existingSegnalazione == null)
                return NotFound();

            await _segnalazioneProblemaService.DeleteAsync(id);
            return NoContent();
        }
    }
} 