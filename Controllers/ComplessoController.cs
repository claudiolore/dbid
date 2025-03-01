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
    public class ComplessoController : ControllerBase
    {
        private readonly IComplessoService _complessoService;

        public ComplessoController(IComplessoService complessoService)
        {
            _complessoService = complessoService;
        }

        /// <summary>
        /// Ottiene tutti i complessi
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Complesso>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var complessi = await _complessoService.GetAllAsync();
            return Ok(complessi);
        }

        /// <summary>
        /// Ottiene un complesso specifico tramite ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Complesso), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var complesso = await _complessoService.GetByIdAsync(id);
            if (complesso == null)
                return NotFound();

            return Ok(complesso);
        }

        /// <summary>
        /// Crea un nuovo complesso
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Complesso), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Complesso complesso)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _complessoService.CreateAsync(complesso);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Aggiorna un complesso esistente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Complesso), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] Complesso complesso)
        {
            if (id != complesso.Id)
                return BadRequest("L'ID nel path non corrisponde all'ID nell'oggetto");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingComplesso = await _complessoService.GetByIdAsync(id);
            if (existingComplesso == null)
                return NotFound();

            var result = await _complessoService.UpdateAsync(id, complesso);
            return Ok(result);
        }

        /// <summary>
        /// Elimina un complesso
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingComplesso = await _complessoService.GetByIdAsync(id);
            if (existingComplesso == null)
                return NotFound();

            await _complessoService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Ottiene gli edifici associati a un complesso
        /// </summary>
        [HttpGet("{id}/edifici")]
        [ProducesResponseType(typeof(IEnumerable<Edificio>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEdificiByComplessoId(Guid id)
        {
            var existingComplesso = await _complessoService.GetByIdAsync(id);
            if (existingComplesso == null)
                return NotFound();

            var edifici = await _complessoService.GetEdificiByComplessoIdAsync(id);
            return Ok(edifici);
        }

        /// <summary>
        /// Associa una struttura a un complesso
        /// </summary>
        [HttpPost("{complessoId}/strutture/{strutturaId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddStrutturaToComplesso(Guid complessoId, Guid strutturaId)
        {
            var result = await _complessoService.AddStrutturaToComplessoAsync(complessoId, strutturaId);

            if (!result)
                return NotFound("Complesso o struttura non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di una struttura da un complesso
        /// </summary>
        [HttpDelete("{complessoId}/strutture/{strutturaId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveStrutturaFromComplesso(Guid complessoId, Guid strutturaId)
        {
            var result = await _complessoService.RemoveStrutturaFromComplessoAsync(complessoId, strutturaId);

            if (!result)
                return NotFound("Complesso o struttura non trovati");

            return Ok();
        }

        // IdraulicoAdduzione
        /// <summary>
        /// Associa un impianto idraulico di adduzione a un complesso
        /// </summary>
        [HttpPost("{complessoId}/idraulicoadduzione/{idraulicoAdduzioneId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddIdraulicoAdduzioneToComplesso(Guid complessoId, Guid idraulicoAdduzioneId)
        {
            var result = await _complessoService.AddIdraulicoAdduzioneToComplessoAsync(complessoId, idraulicoAdduzioneId);

            if (!result)
                return NotFound("Complesso o impianto idraulico di adduzione non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un impianto idraulico di adduzione da un complesso
        /// </summary>
        [HttpDelete("{complessoId}/idraulicoadduzione/{idraulicoAdduzioneId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveIdraulicoAdduzioneFromComplesso(Guid complessoId, Guid idraulicoAdduzioneId)
        {
            var result = await _complessoService.RemoveIdraulicoAdduzioneFromComplessoAsync(complessoId, idraulicoAdduzioneId);

            if (!result)
                return NotFound("Complesso o impianto idraulico di adduzione non trovati");

            return Ok();
        }

        // ScarichiIdriciFognari
        /// <summary>
        /// Associa un impianto di scarichi idrici e fognari a un complesso
        /// </summary>
        [HttpPost("{complessoId}/scarichiidricifognari/{scarichiIdriciFognariId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddScarichiIdriciFognariToComplesso(Guid complessoId, Guid scarichiIdriciFognariId)
        {
            var result = await _complessoService.AddScarichiIdriciFognariToComplessoAsync(complessoId, scarichiIdriciFognariId);

            if (!result)
                return NotFound("Complesso o impianto di scarichi idrici e fognari non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un impianto di scarichi idrici e fognari da un complesso
        /// </summary>
        [HttpDelete("{complessoId}/scarichiidricifognari/{scarichiIdriciFognariId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveScarichiIdriciFognariFromComplesso(Guid complessoId, Guid scarichiIdriciFognariId)
        {
            var result = await _complessoService.RemoveScarichiIdriciFognariFromComplessoAsync(complessoId, scarichiIdriciFognariId);

            if (!result)
                return NotFound("Complesso o impianto di scarichi idrici e fognari non trovati");

            return Ok();
        }

        // ImpiantiElettrici
        /// <summary>
        /// Associa un impianto elettrico a un complesso
        /// </summary>
        [HttpPost("{complessoId}/impiantielettrici/{impiantiElettriciId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddImpiantiElettriciToComplesso(Guid complessoId, Guid impiantiElettriciId)
        {
            var result = await _complessoService.AddImpiantiElettriciToComplessoAsync(complessoId, impiantiElettriciId);

            if (!result)
                return NotFound("Complesso o impianto elettrico non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un impianto elettrico da un complesso
        /// </summary>
        [HttpDelete("{complessoId}/impiantielettrici/{impiantiElettriciId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveImpiantiElettriciFromComplesso(Guid complessoId, Guid impiantiElettriciId)
        {
            var result = await _complessoService.RemoveImpiantiElettriciFromComplessoAsync(complessoId, impiantiElettriciId);

            if (!result)
                return NotFound("Complesso o impianto elettrico non trovati");

            return Ok();
        }

        // ImpiantoClimaAcs
        /// <summary>
        /// Associa un impianto di climatizzazione e ACS a un complesso
        /// </summary>
        [HttpPost("{complessoId}/impiantoclimaacs/{impiantoClimaAcsId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddImpiantoClimaAcsToComplesso(Guid complessoId, Guid impiantoClimaAcsId)
        {
            var result = await _complessoService.AddImpiantoClimaAcsToComplessoAsync(complessoId, impiantoClimaAcsId);

            if (!result)
                return NotFound("Complesso o impianto di climatizzazione e ACS non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un impianto di climatizzazione e ACS da un complesso
        /// </summary>
        [HttpDelete("{complessoId}/impiantoclimaacs/{impiantoClimaAcsId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveImpiantoClimaAcsFromComplesso(Guid complessoId, Guid impiantoClimaAcsId)
        {
            var result = await _complessoService.RemoveImpiantoClimaAcsFromComplessoAsync(complessoId, impiantoClimaAcsId);

            if (!result)
                return NotFound("Complesso o impianto di climatizzazione e ACS non trovati");

            return Ok();
        }

        // AltriImpianti
        /// <summary>
        /// Associa un altro impianto a un complesso
        /// </summary>
        [HttpPost("{complessoId}/altriimpianti/{altriImpiantiId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddAltriImpiantiToComplesso(Guid complessoId, Guid altriImpiantiId)
        {
            var result = await _complessoService.AddAltriImpiantiToComplessoAsync(complessoId, altriImpiantiId);

            if (!result)
                return NotFound("Complesso o altro impianto non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un altro impianto da un complesso
        /// </summary>
        [HttpDelete("{complessoId}/altriimpianti/{altriImpiantiId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveAltriImpiantiFromComplesso(Guid complessoId, Guid altriImpiantiId)
        {
            var result = await _complessoService.RemoveAltriImpiantiFromComplessoAsync(complessoId, altriImpiantiId);

            if (!result)
                return NotFound("Complesso o altro impianto non trovati");

            return Ok();
        }

        // DocumentiGenerali
        /// <summary>
        /// Associa un documento generale a un complesso
        /// </summary>
        [HttpPost("{complessoId}/documentigenerali/{documentiGeneraliId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddDocumentiGeneraliToComplesso(Guid complessoId, Guid documentiGeneraliId)
        {
            var result = await _complessoService.AddDocumentiGeneraliToComplessoAsync(complessoId, documentiGeneraliId);

            if (!result)
                return NotFound("Complesso o documento generale non trovati");

            return Ok();
        }

        /// <summary>
        /// Rimuove l'associazione di un documento generale da un complesso
        /// </summary>
        [HttpDelete("{complessoId}/documentigenerali/{documentiGeneraliId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveDocumentiGeneraliFromComplesso(Guid complessoId, Guid documentiGeneraliId)
        {
            var result = await _complessoService.RemoveDocumentiGeneraliFromComplessoAsync(complessoId, documentiGeneraliId);

            if (!result)
                return NotFound("Complesso o documento generale non trovati");

            return Ok();
        }
    }
}