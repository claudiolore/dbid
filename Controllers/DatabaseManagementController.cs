using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Services;

namespace dbid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseManagementController : ControllerBase
    {
        private readonly IDatabaseManagementService _databaseService;

        public DatabaseManagementController(IDatabaseManagementService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Elimina completamente todos los datos de la base de datos
        /// </summary>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("reset")]
        public async Task<IActionResult> ResetDatabase()
        {
            try
            {
                bool result = await _databaseService.ResetDatabaseAsync();

                if (result)
                {
                    return Ok(new { success = true, message = "La base de datos ha sido reseteada exitosamente." });
                }
                else
                {
                    return StatusCode(500, new { success = false, message = "Error al resetear la base de datos" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al resetear la base de datos", error = ex.Message });
            }
        }
    }
}