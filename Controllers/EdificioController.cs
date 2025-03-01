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

        /// <summary>
        /// Associa una struttura a un edificio
        /// </summary>
        [HttpPost("{edificioId}/strutture/{strutturaId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddStrutturaToEdificio(Guid edificioId, Guid strutturaId)
        {
            var result = await _edificioService.AddStrutturaToEdificioAsync(edificioId, strutturaId);

            if (!result)
                return NotFound("Edificio o struttura non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di una struttura da un edificio
        /// </summary>
        [HttpDelete("{edificioId}/strutture/{strutturaId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveStrutturaFromEdificio(Guid edificioId, Guid strutturaId)
        {
            var result = await _edificioService.RemoveStrutturaFromEdificioAsync(edificioId, strutturaId);

            if (!result)
                return NotFound("Edificio o struttura non trovati");

            return Ok();
        }

        /// <summary>
        /// Associa un infisso a un edificio
        /// </summary>
        [HttpPost("{edificioId}/infissi/{infissiId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddInfissiToEdificio(Guid edificioId, Guid infissiId)
        {
            var result = await _edificioService.AddInfissiToEdificioAsync(edificioId, infissiId);

            if (!result)
                return NotFound("Edificio o infisso non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un infisso da un edificio
        /// </summary>
        [HttpDelete("{edificioId}/infissi/{infissiId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveInfissiFromEdificio(Guid edificioId, Guid infissiId)
        {
            var result = await _edificioService.RemoveInfissiFromEdificioAsync(edificioId, infissiId);

            if (!result)
                return NotFound("Edificio o infisso non trovati");

            return Ok();
        }

        /// <summary>
        /// Associa un impianto idraulico di adduzione a un edificio
        /// </summary>
        [HttpPost("{edificioId}/idraulicoadduzione/{idraulicoAdduzioneId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddIdraulicoAdduzioneToEdificio(Guid edificioId, Guid idraulicoAdduzioneId)
        {
            var result = await _edificioService.AddIdraulicoAdduzioneToEdificioAsync(edificioId, idraulicoAdduzioneId);

            if (!result)
                return NotFound("Edificio o impianto idraulico di adduzione non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un impianto idraulico di adduzione da un edificio
        /// </summary>
        [HttpDelete("{edificioId}/idraulicoadduzione/{idraulicoAdduzioneId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveIdraulicoAdduzioneFromEdificio(Guid edificioId, Guid idraulicoAdduzioneId)
        {
            var result = await _edificioService.RemoveIdraulicoAdduzioneFromEdificioAsync(edificioId, idraulicoAdduzioneId);

            if (!result)
                return NotFound("Edificio o impianto idraulico di adduzione non trovati");

            return Ok();
        }

        /// <summary>
        /// Associa un impianto di scarichi idrici e fognari a un edificio
        /// </summary>
        [HttpPost("{edificioId}/scarichiidricifognari/{scarichiIdriciFognariId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddScarichiIdriciFognariToEdificio(Guid edificioId, Guid scarichiIdriciFognariId)
        {
            var result = await _edificioService.AddScarichiIdriciFognariToEdificioAsync(edificioId, scarichiIdriciFognariId);

            if (!result)
                return NotFound("Edificio o impianto di scarichi idrici e fognari non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un impianto di scarichi idrici e fognari da un edificio
        /// </summary>
        [HttpDelete("{edificioId}/scarichiidricifognari/{scarichiIdriciFognariId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveScarichiIdriciFognariFromEdificio(Guid edificioId, Guid scarichiIdriciFognariId)
        {
            var result = await _edificioService.RemoveScarichiIdriciFognariFromEdificioAsync(edificioId, scarichiIdriciFognariId);

            if (!result)
                return NotFound("Edificio o impianto di scarichi idrici e fognari non trovati");

            return Ok();
        }

        /// <summary>
        /// Associa un impianto elettrico a un edificio
        /// </summary>
        [HttpPost("{edificioId}/impiantielettrici/{impiantiElettriciId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddImpiantiElettriciToEdificio(Guid edificioId, Guid impiantiElettriciId)
        {
            var result = await _edificioService.AddImpiantiElettriciToEdificioAsync(edificioId, impiantiElettriciId);

            if (!result)
                return NotFound("Edificio o impianto elettrico non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un impianto elettrico da un edificio
        /// </summary>
        [HttpDelete("{edificioId}/impiantielettrici/{impiantiElettriciId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveImpiantiElettriciFromEdificio(Guid edificioId, Guid impiantiElettriciId)
        {
            var result = await _edificioService.RemoveImpiantiElettriciFromEdificioAsync(edificioId, impiantiElettriciId);

            if (!result)
                return NotFound("Edificio o impianto elettrico non trovati");

            return Ok();
        }

        /// <summary>
        /// Associa un impianto clima e acs a un edificio
        /// </summary>
        [HttpPost("{edificioId}/impiantoclimaacs/{impiantoClimaAcsId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddImpiantoClimaAcsToEdificio(Guid edificioId, Guid impiantoClimaAcsId)
        {
            var result = await _edificioService.AddImpiantoClimaAcsToEdificioAsync(edificioId, impiantoClimaAcsId);

            if (!result)
                return NotFound("Edificio o impianto clima e acs non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un impianto clima e acs da un edificio
        /// </summary>
        [HttpDelete("{edificioId}/impiantoclimaacs/{impiantoClimaAcsId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveImpiantoClimaAcsFromEdificio(Guid edificioId, Guid impiantoClimaAcsId)
        {
            var result = await _edificioService.RemoveImpiantoClimaAcsFromEdificioAsync(edificioId, impiantoClimaAcsId);

            if (!result)
                return NotFound("Edificio o impianto clima e acs non trovati");

            return Ok();
        }

        /// <summary>
        /// Associa un altro impianto a un edificio
        /// </summary>
        [HttpPost("{edificioId}/altriimpianti/{altriImpiantiId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddAltriImpiantiToEdificio(Guid edificioId, Guid altriImpiantiId)
        {
            var result = await _edificioService.AddAltriImpiantiToEdificioAsync(edificioId, altriImpiantiId);

            if (!result)
                return NotFound("Edificio o altro impianto non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un altro impianto da un edificio
        /// </summary>
        [HttpDelete("{edificioId}/altriimpianti/{altriImpiantiId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveAltriImpiantiFromEdificio(Guid edificioId, Guid altriImpiantiId)
        {
            var result = await _edificioService.RemoveAltriImpiantiFromEdificioAsync(edificioId, altriImpiantiId);

            if (!result)
                return NotFound("Edificio o altro impianto non trovati");

            return Ok();
        }

        /// <summary>
        /// Associa un documento generale a un edificio
        /// </summary>
        [HttpPost("{edificioId}/documentigenerali/{documentiGeneraliId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddDocumentiGeneraliToEdificio(Guid edificioId, Guid documentiGeneraliId)
        {
            var result = await _edificioService.AddDocumentiGeneraliToEdificioAsync(edificioId, documentiGeneraliId);

            if (!result)
                return NotFound("Edificio o documento generale non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un documento generale da un edificio
        /// </summary>
        [HttpDelete("{edificioId}/documentigenerali/{documentiGeneraliId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveDocumentiGeneraliFromEdificio(Guid edificioId, Guid documentiGeneraliId)
        {
            var result = await _edificioService.RemoveDocumentiGeneraliFromEdificioAsync(edificioId, documentiGeneraliId);

            if (!result)
                return NotFound("Edificio o documento generale non trovati");

            return Ok();
        }
    }
}