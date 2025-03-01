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
    }
} 