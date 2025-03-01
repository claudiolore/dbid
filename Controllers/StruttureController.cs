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
    public class StruttureController : ControllerBase
    {
        private readonly IStruttureService _struttureService;

        public StruttureController(IStruttureService struttureService)
        {
            _struttureService = struttureService;
        }

        /// <summary>
        /// Ottiene tutte le strutture
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Strutture>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var strutture = await _struttureService.GetAllAsync();
            return Ok(strutture);
        }

        /// <summary>
        /// Ottiene una struttura specifica tramite ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Strutture), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var struttura = await _struttureService.GetByIdAsync(id);
            if (struttura == null)
                return NotFound();

            return Ok(struttura);
        }

        /// <summary>
        /// Crea una nuova struttura
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Strutture), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Strutture struttura)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _struttureService.CreateAsync(struttura);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Aggiorna una struttura esistente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Strutture), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] Strutture struttura)
        {
            if (id != struttura.Id)
                return BadRequest("L'ID nel path non corrisponde all'ID nell'oggetto");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingStruttura = await _struttureService.GetByIdAsync(id);
            if (existingStruttura == null)
                return NotFound();

            var result = await _struttureService.UpdateAsync(id, struttura);
            return Ok(result);
        }

        /// <summary>
        /// Elimina una struttura
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingStruttura = await _struttureService.GetByIdAsync(id);
            if (existingStruttura == null)
                return NotFound();

            await _struttureService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Ottiene le segnalazioni di problemi associate a una struttura
        /// </summary>
        [HttpGet("{id}/segnalazioni")]
        [ProducesResponseType(typeof(IEnumerable<SegnalazioneProblema>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSegnalazioniByStrutturaId(Guid id)
        {
            var existingStruttura = await _struttureService.GetByIdAsync(id);
            if (existingStruttura == null)
                return NotFound();

            var segnalazioni = await _struttureService.GetSegnalazioniByStrutturaIdAsync(id);
            return Ok(segnalazioni);
        }
    }
}