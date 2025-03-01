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
    public class DocumentiGeneraliController : ControllerBase
    {
        private readonly IDocumentiGeneraliService _documentiGeneraliService;

        public DocumentiGeneraliController(IDocumentiGeneraliService documentiGeneraliService)
        {
            _documentiGeneraliService = documentiGeneraliService;
        }

        /// <summary>
        /// Ottiene tutti i documenti generali
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DocumentiGenerali>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var documenti = await _documentiGeneraliService.GetAllAsync();
            return Ok(documenti);
        }

        /// <summary>
        /// Ottiene un documento generale specifico tramite ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DocumentiGenerali), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var documento = await _documentiGeneraliService.GetByIdAsync(id);
            if (documento == null)
                return NotFound();

            return Ok(documento);
        }

        /// <summary>
        /// Crea un nuovo documento generale
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(DocumentiGenerali), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] DocumentiGenerali documento)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _documentiGeneraliService.CreateAsync(documento);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Aggiorna un documento generale esistente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DocumentiGenerali), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] DocumentiGenerali documento)
        {
            if (id != documento.Id)
                return BadRequest("L'ID nel path non corrisponde all'ID nell'oggetto");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingDocumento = await _documentiGeneraliService.GetByIdAsync(id);
            if (existingDocumento == null)
                return NotFound();

            var result = await _documentiGeneraliService.UpdateAsync(id, documento);
            return Ok(result);
        }

        /// <summary>
        /// Elimina un documento generale
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingDocumento = await _documentiGeneraliService.GetByIdAsync(id);
            if (existingDocumento == null)
                return NotFound();

            await _documentiGeneraliService.DeleteAsync(id);
            return NoContent();
        }
    }
} 