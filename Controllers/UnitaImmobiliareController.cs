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

        /// <summary>
        /// Associa un infisso a un'unità immobiliare
        /// </summary>
        [HttpPost("{unitaImmobiliareId}/infissi/{infissiId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddInfissiToUnitaImmobiliare(Guid unitaImmobiliareId, Guid infissiId)
        {
            var result = await _unitaImmobiliareService.AddInfissiToUnitaImmobiliareAsync(unitaImmobiliareId, infissiId);

            if (!result)
                return NotFound("Unità immobiliare o infisso non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un infisso da un'unità immobiliare
        /// </summary>
        [HttpDelete("{unitaImmobiliareId}/infissi/{infissiId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveInfissiFromUnitaImmobiliare(Guid unitaImmobiliareId, Guid infissiId)
        {
            var result = await _unitaImmobiliareService.RemoveInfissiFromUnitaImmobiliareAsync(unitaImmobiliareId, infissiId);

            if (!result)
                return NotFound("Unità immobiliare o infisso non trovati");

            return Ok();
        }

        /// <summary>
        /// Associa un impianto idraulico di adduzione a un'unità immobiliare
        /// </summary>
        [HttpPost("{unitaImmobiliareId}/idraulicoadduzione/{idraulicoAdduzioneId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddIdraulicoAdduzioneToUnitaImmobiliare(Guid unitaImmobiliareId, Guid idraulicoAdduzioneId)
        {
            var result = await _unitaImmobiliareService.AddIdraulicoAdduzioneToUnitaImmobiliareAsync(unitaImmobiliareId, idraulicoAdduzioneId);

            if (!result)
                return NotFound("Unità immobiliare o impianto idraulico di adduzione non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un impianto idraulico di adduzione da un'unità immobiliare
        /// </summary>
        [HttpDelete("{unitaImmobiliareId}/idraulicoadduzione/{idraulicoAdduzioneId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveIdraulicoAdduzioneFromUnitaImmobiliare(Guid unitaImmobiliareId, Guid idraulicoAdduzioneId)
        {
            var result = await _unitaImmobiliareService.RemoveIdraulicoAdduzioneFromUnitaImmobiliareAsync(unitaImmobiliareId, idraulicoAdduzioneId);

            if (!result)
                return NotFound("Unità immobiliare o impianto idraulico di adduzione non trovati");

            return Ok();
        }

        /// <summary>
        /// Associa un impianto di scarichi idrici e fognari a un'unità immobiliare
        /// </summary>
        [HttpPost("{unitaImmobiliareId}/scarichiidricifognari/{scarichiIdriciFognariId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddScarichiIdriciFognariToUnitaImmobiliare(Guid unitaImmobiliareId, Guid scarichiIdriciFognariId)
        {
            var result = await _unitaImmobiliareService.AddScarichiIdriciFognariToUnitaImmobiliareAsync(unitaImmobiliareId, scarichiIdriciFognariId);

            if (!result)
                return NotFound("Unità immobiliare o impianto di scarichi idrici e fognari non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un impianto di scarichi idrici e fognari da un'unità immobiliare
        /// </summary>
        [HttpDelete("{unitaImmobiliareId}/scarichiidricifognari/{scarichiIdriciFognariId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveScarichiIdriciFognariFromUnitaImmobiliare(Guid unitaImmobiliareId, Guid scarichiIdriciFognariId)
        {
            var result = await _unitaImmobiliareService.RemoveScarichiIdriciFognariFromUnitaImmobiliareAsync(unitaImmobiliareId, scarichiIdriciFognariId);

            if (!result)
                return NotFound("Unità immobiliare o impianto di scarichi idrici e fognari non trovati");

            return Ok();
        }

        /// <summary>
        /// Associa un impianto elettrico a un'unità immobiliare
        /// </summary>
        [HttpPost("{unitaImmobiliareId}/impiantielettrici/{impiantiElettriciId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddImpiantiElettriciToUnitaImmobiliare(Guid unitaImmobiliareId, Guid impiantiElettriciId)
        {
            var result = await _unitaImmobiliareService.AddImpiantiElettriciToUnitaImmobiliareAsync(unitaImmobiliareId, impiantiElettriciId);

            if (!result)
                return NotFound("Unità immobiliare o impianto elettrico non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un impianto elettrico da un'unità immobiliare
        /// </summary>
        [HttpDelete("{unitaImmobiliareId}/impiantielettrici/{impiantiElettriciId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveImpiantiElettriciFromUnitaImmobiliare(Guid unitaImmobiliareId, Guid impiantiElettriciId)
        {
            var result = await _unitaImmobiliareService.RemoveImpiantiElettriciFromUnitaImmobiliareAsync(unitaImmobiliareId, impiantiElettriciId);

            if (!result)
                return NotFound("Unità immobiliare o impianto elettrico non trovati");

            return Ok();
        }

        /// <summary>
        /// Associa un impianto clima e acs a un'unità immobiliare
        /// </summary>
        [HttpPost("{unitaImmobiliareId}/impiantoclimaacs/{impiantoClimaAcsId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddImpiantoClimaAcsToUnitaImmobiliare(Guid unitaImmobiliareId, Guid impiantoClimaAcsId)
        {
            var result = await _unitaImmobiliareService.AddImpiantoClimaAcsToUnitaImmobiliareAsync(unitaImmobiliareId, impiantoClimaAcsId);

            if (!result)
                return NotFound("Unità immobiliare o impianto clima e acs non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un impianto clima e acs da un'unità immobiliare
        /// </summary>
        [HttpDelete("{unitaImmobiliareId}/impiantoclimaacs/{impiantoClimaAcsId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveImpiantoClimaAcsFromUnitaImmobiliare(Guid unitaImmobiliareId, Guid impiantoClimaAcsId)
        {
            var result = await _unitaImmobiliareService.RemoveImpiantoClimaAcsFromUnitaImmobiliareAsync(unitaImmobiliareId, impiantoClimaAcsId);

            if (!result)
                return NotFound("Unità immobiliare o impianto clima e acs non trovati");

            return Ok();
        }

        /// <summary>
        /// Associa un altro impianto a un'unità immobiliare
        /// </summary>
        [HttpPost("{unitaImmobiliareId}/altriimpianti/{altriImpiantiId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddAltriImpiantiToUnitaImmobiliare(Guid unitaImmobiliareId, Guid altriImpiantiId)
        {
            var result = await _unitaImmobiliareService.AddAltriImpiantiToUnitaImmobiliareAsync(unitaImmobiliareId, altriImpiantiId);

            if (!result)
                return NotFound("Unità immobiliare o altro impianto non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un altro impianto da un'unità immobiliare
        /// </summary>
        [HttpDelete("{unitaImmobiliareId}/altriimpianti/{altriImpiantiId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveAltriImpiantiFromUnitaImmobiliare(Guid unitaImmobiliareId, Guid altriImpiantiId)
        {
            var result = await _unitaImmobiliareService.RemoveAltriImpiantiFromUnitaImmobiliareAsync(unitaImmobiliareId, altriImpiantiId);

            if (!result)
                return NotFound("Unità immobiliare o altro impianto non trovati");

            return Ok();
        }

        /// <summary>
        /// Associa un documento generale a un'unità immobiliare
        /// </summary>
        [HttpPost("{unitaImmobiliareId}/documentigenerali/{documentiGeneraliId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddDocumentiGeneraliToUnitaImmobiliare(Guid unitaImmobiliareId, Guid documentiGeneraliId)
        {
            var result = await _unitaImmobiliareService.AddDocumentiGeneraliToUnitaImmobiliareAsync(unitaImmobiliareId, documentiGeneraliId);

            if (!result)
                return NotFound("Unità immobiliare o documento generale non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un documento generale da un'unità immobiliare
        /// </summary>
        [HttpDelete("{unitaImmobiliareId}/documentigenerali/{documentiGeneraliId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveDocumentiGeneraliFromUnitaImmobiliare(Guid unitaImmobiliareId, Guid documentiGeneraliId)
        {
            var result = await _unitaImmobiliareService.RemoveDocumentiGeneraliFromUnitaImmobiliareAsync(unitaImmobiliareId, documentiGeneraliId);

            if (!result)
                return NotFound("Unità immobiliare o documento generale non trovati");

            return Ok();
        }
    }
}