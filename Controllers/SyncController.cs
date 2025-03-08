using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.DTO;
using Services;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // Assicura che tutte le richieste siano autenticate
    public class SyncController : ControllerBase
    {
        private readonly ISyncService _syncService;
        private readonly ILogger<SyncController> _logger;

        public SyncController(ISyncService syncService, ILogger<SyncController> logger)
        {
            _syncService = syncService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint principale per ricevere tutti i dati dall'app mobile.
        /// </summary>
        /// <param name="request">I dati da sincronizzare</param>
        /// <returns>Statistiche di sincronizzazione</returns>
        [HttpPost("data")]
        [ApiExplorerSettings(IgnoreApi = false)]
        // Disabilitiamo la validazione automatica del modello
        public async Task<IActionResult> SyncData([FromBody] SyncRequest request)
        {
            // Controllo base sulla presenza dei dati
            if (request == null)
            {
                return BadRequest("La richiesta non può essere nulla");
            }

            // Inizializza le proprietà se sono nulle per evitare errori nel processing
            if (request.Data == null)
            {
                request.Data = new SyncData();
            }

            if (request.DeviceInfo == null)
            {
                request.DeviceInfo = new DeviceInfo
                {
                    Platform = "unknown",
                    Version = "unknown",
                    DeviceId = Guid.NewGuid().ToString()
                };
            }

            try
            {
                _logger.LogInformation($"Ricevuta richiesta di sincronizzazione da {request.DeviceInfo?.Platform} v{request.DeviceInfo?.Version}, DeviceId: {request.DeviceInfo?.DeviceId}");

                // Logga dettagli sulla quantità di dati ricevuti
                _logger.LogInformation($"Dati ricevuti: " +
                    $"Complessi: {request.Data.Complessi?.Count ?? 0}, " +
                    $"Edifici: {request.Data.Edifici?.Count ?? 0}, " +
                    $"UnitaImmobiliari: {request.Data.UnitaImmobiliari?.Count ?? 0}, " +
                    $"Strutture: {request.Data.Strutture?.Count ?? 0}, " +
                    $"ImpiantiIdrici: {request.Data.ImpiantiIdrici?.Count ?? 0}");

                var result = await _syncService.SyncData(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la sincronizzazione dei dati");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Si è verificato un errore durante la sincronizzazione: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Endpoint per verificare lo stato di sincronizzazione e l'ultima sincronizzazione effettuata.
        /// </summary>
        /// <param name="deviceId">ID del dispositivo</param>
        /// <returns>Stato di sincronizzazione</returns>
        [HttpGet("status")]
        public async Task<IActionResult> GetSyncStatus([FromQuery] string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest("L'ID del dispositivo è obbligatorio");
            }

            try
            {
                var result = await _syncService.GetSyncStatus(deviceId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero dello stato di sincronizzazione");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Si è verificato un errore durante il recupero dello stato di sincronizzazione: " + ex.Message
                });
            }
        }
    }

    [ApiController]
    [Route("api/files")]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly ISyncService _syncService;
        private readonly ILogger<FilesController> _logger;

        public FilesController(ISyncService syncService, ILogger<FilesController> logger)
        {
            _syncService = syncService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint per caricare i file associati (foto, documenti, ecc).
        /// </summary>
        /// <param name="request">Dati del file da caricare</param>
        /// <returns>Risultato del caricamento</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromBody] FileUploadRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.FileName))
            {
                return BadRequest("I dati del file sono obbligatori");
            }

            // Ulteriori validazioni
            if (request.FileSize <= 0)
            {
                return BadRequest("La dimensione del file deve essere maggiore di zero");
            }

            if (string.IsNullOrEmpty(request.EntityId) || string.IsNullOrEmpty(request.EntityType))
            {
                return BadRequest("EntityId e EntityType sono obbligatori");
            }

            try
            {
                _logger.LogInformation($"Ricevuta richiesta di caricamento file: {request.FileName} per entità {request.EntityType}/{request.EntityId}");
                var result = await _syncService.UploadFile(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il caricamento del file");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Si è verificato un errore durante il caricamento del file: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Endpoint per notificare il completamento della sincronizzazione dei file.
        /// </summary>
        /// <param name="request">Dati sulla sincronizzazione completata</param>
        /// <returns>Stato di completamento</returns>
        [HttpPost("sync-complete")]
        public async Task<IActionResult> CompleteSyncFiles([FromBody] SyncFilesCompleteRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.DeviceId))
            {
                return BadRequest("L'ID del dispositivo è obbligatorio");
            }

            try
            {
                _logger.LogInformation($"Ricevuta notifica di completamento sincronizzazione file per dispositivo {request.DeviceId}");
                var result = await _syncService.CompleteSyncFiles(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la notifica di completamento della sincronizzazione dei file");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Si è verificato un errore durante la notifica di completamento: " + ex.Message
                });
            }
        }
    }
}
