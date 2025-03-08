using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;
using System.Reflection;
using System.Transactions;
using System.Security.Cryptography;
using System.Text;

namespace Services
{
    public interface ISyncService
    {
        Task<SyncResponse> SyncData(SyncRequest request);
        Task<SyncStatusResponse> GetSyncStatus(string deviceId);
        Task<FileUploadResponse> UploadFile(FileUploadRequest request);
        Task<SyncCompleteResponse> CompleteSyncFiles(SyncFilesCompleteRequest request);
    }

    public class SyncService : ISyncService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SyncService> _logger;

        public SyncService(ApplicationDbContext context, ILogger<SyncService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SyncResponse> SyncData(SyncRequest request)
        {
            _logger.LogInformation($"Inizio sincronizzazione dati da dispositivo {request.DeviceInfo?.Platform} v{request.DeviceInfo?.Version}");

            var response = new SyncResponse
            {
                Success = true,
                Message = "Sincronizzazione completata",
                Stats = new SyncStats(),
                EntitiesStats = new Dictionary<string, EntityStats>(),
                Timestamp = DateTime.UtcNow
            };

            // Verifica il formato della richiesta
            if (request.Data == null)
            {
                _logger.LogWarning("I dati di sincronizzazione sono vuoti, creando una risposta vuota");
                return response;
            }

            try
            {
                // Converte richiesta nel formato DTO appropriato se non è già in quel formato
                // Questo è necessario perché il formato JSON dall'app è diverso dal nostro DTO
                bool needsFormatConversion = NeedsFormatConversion(request.Data);

                if (needsFormatConversion)
                {
                    _logger.LogInformation("Rilevato formato dati da app mobile. Conversione in corso...");
                    try
                    {
                        request.Data = ConvertDataFormat(request.Data);
                        _logger.LogInformation($"Conversione formato dati completata. {IdMappings.Count} ID mappati.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Errore durante la conversione del formato dati");
                        response.Success = false;
                        response.Message = "Errore durante la conversione del formato dati: " + ex.Message;
                        return response;
                    }
                }

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Sincronizza i complessi
                        if (request.Data.Complessi != null && request.Data.Complessi.Any())
                        {
                            try
                            {
                                var stats = await SincronizzaComplessi(request.Data.Complessi);
                                response.EntitiesStats["complessi"] = stats;
                                AggiungiAStatisticheTotali(response.Stats, stats);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Errore durante la sincronizzazione dei complessi");
                                response.EntitiesStats["complessi"] = new EntityStats { Total = request.Data.Complessi.Count, Unchanged = request.Data.Complessi.Count };
                            }
                        }

                        // Sincronizza gli edifici
                        if (request.Data.Edifici != null && request.Data.Edifici.Any())
                        {
                            try
                            {
                                // Prima normalizza i dati per evitare errori di validazione
                                foreach (var edificio in request.Data.Edifici)
                                {
                                    // Assicurati che tutti i campi obbligatori siano impostati
                                    edificio.ParentId ??= edificio.IdComplesso;
                                    edificio.ParentType ??= "complesso";
                                    edificio.ComplessoId ??= edificio.IdComplesso;
                                    edificio.Prospetto ??= new object();
                                    edificio.SezioneDoc ??= new object();
                                    edificio.Planimetria ??= new object();
                                    edificio.TitoloEdilizio ??= new object();
                                    edificio.CollaudoStatico ??= new object();
                                    edificio.StruttureImmobile ??= new object();
                                    edificio.ProgettoStrutturale ??= new object();
                                    edificio.PianoManutenzioneOpera ??= new object();
                                    edificio.RicevutePagamentoTributi ??= new object();
                                    edificio.RapportoVerificaAscensori ??= new object();
                                    edificio.SegnalazioneCertificataAgibilita ??= new object();
                                }

                                var stats = await SincronizzaEdifici(request.Data.Edifici);
                                response.EntitiesStats["edifici"] = stats;
                                AggiungiAStatisticheTotali(response.Stats, stats);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Errore durante la sincronizzazione degli edifici");
                                response.EntitiesStats["edifici"] = new EntityStats { Total = request.Data.Edifici.Count, Unchanged = request.Data.Edifici.Count };
                            }
                        }

                        // Sincronizza le unità immobiliari
                        if (request.Data.UnitaImmobiliari != null && request.Data.UnitaImmobiliari.Any())
                        {
                            try
                            {
                                // Normalizza i dati per evitare errori di validazione
                                foreach (var unita in request.Data.UnitaImmobiliari)
                                {
                                    unita.ParentId ??= unita.IdEdificio;
                                    unita.ParentType ??= "edificio";
                                    unita.EdificioId ??= unita.IdEdificio;
                                    unita.DisegnoSchemaInfissi ??= new object();
                                    unita.PlanimetriaCatastale ??= new object();
                                    unita.VisuraIpotecaria ??= new object();
                                    unita.VisuraCatastaleStorica ??= new object();
                                    unita.CertificatoDestinazioneUrbanistica ??= new object();
                                    unita.AttestazionePrestazioneEnergetica ??= new object();
                                    unita.TitoloEdilizio ??= new object();
                                    unita.ContrattoUso ??= new object();
                                    unita.SegnalazioneCertificataAgibilita ??= new object();
                                    unita.RicevutePagamentoTributi ??= new object();
                                    unita.RilievoArchitettonico ??= new object();
                                }

                                var stats = await SincronizzaUnitaImmobiliari(request.Data.UnitaImmobiliari);
                                response.EntitiesStats["unita_immobiliari"] = stats;
                                AggiungiAStatisticheTotali(response.Stats, stats);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Errore durante la sincronizzazione delle unità immobiliari");
                                response.EntitiesStats["unita_immobiliari"] = new EntityStats { Total = request.Data.UnitaImmobiliari.Count, Unchanged = request.Data.UnitaImmobiliari.Count };
                            }
                        }

                        // Sincronizza le strutture
                        if (request.Data.Strutture != null && request.Data.Strutture.Any())
                        {
                            try
                            {
                                // Normalizza i dati
                                foreach (var struttura in request.Data.Strutture)
                                {
                                    struttura.Foto ??= new object();
                                    struttura.UnitaId ??= "";
                                    struttura.EdificioId ??= "";
                                    struttura.ComplessoId ??= "";

                                    // Imposta gli ID corretti in base al parentType
                                    if (struttura.ParentType == "complesso")
                                    {
                                        struttura.ComplessoId = struttura.ParentId;
                                        struttura.EdificioId = "";
                                        struttura.UnitaId = "";
                                    }
                                    else if (struttura.ParentType == "edificio")
                                    {
                                        struttura.EdificioId = struttura.ParentId;
                                        struttura.ComplessoId = "";
                                        struttura.UnitaId = "";
                                    }
                                    else if (struttura.ParentType == "unita")
                                    {
                                        struttura.UnitaId = struttura.ParentId;
                                        struttura.ComplessoId = "";
                                        struttura.EdificioId = "";
                                    }
                                }

                                var stats = await SincronizzaStrutture(request.Data.Strutture);
                                response.EntitiesStats["strutture"] = stats;
                                AggiungiAStatisticheTotali(response.Stats, stats);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Errore durante la sincronizzazione delle strutture");
                                response.EntitiesStats["strutture"] = new EntityStats { Total = request.Data.Strutture.Count, Unchanged = request.Data.Strutture.Count };
                            }
                        }

                        // Sincronizza gli impianti idrici
                        if (request.Data.ImpiantiIdrici != null && request.Data.ImpiantiIdrici.Any())
                        {
                            try
                            {
                                // Normalizza i dati
                                foreach (var impianto in request.Data.ImpiantiIdrici)
                                {
                                    impianto.Foto ??= new object();
                                }

                                var stats = await SincronizzaImpiantiIdrici(request.Data.ImpiantiIdrici);
                                response.EntitiesStats["impianti_idrici"] = stats;
                                AggiungiAStatisticheTotali(response.Stats, stats);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Errore durante la sincronizzazione degli impianti idrici");
                                response.EntitiesStats["impianti_idrici"] = new EntityStats { Total = request.Data.ImpiantiIdrici.Count, Unchanged = request.Data.ImpiantiIdrici.Count };
                            }
                        }

                        // Sincronizza gli impianti scarichi
                        if (request.Data.ImpiantiScarichi != null && request.Data.ImpiantiScarichi.Any())
                        {
                            try
                            {
                                var stats = await SincronizzaImpiantiScarichi(request.Data.ImpiantiScarichi);
                                response.EntitiesStats["impianti_scarichi"] = stats;
                                AggiungiAStatisticheTotali(response.Stats, stats);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Errore durante la sincronizzazione degli impianti scarichi");
                                response.EntitiesStats["impianti_scarichi"] = new EntityStats { Total = request.Data.ImpiantiScarichi.Count, Unchanged = request.Data.ImpiantiScarichi.Count };
                            }
                        }

                        // Sincronizza gli impianti clima ACS
                        if (request.Data.ImpiantiClimaAcs != null && request.Data.ImpiantiClimaAcs.Any())
                        {
                            try
                            {
                                // Normalizza i dati
                                foreach (var impianto in request.Data.ImpiantiClimaAcs)
                                {
                                    impianto.Foto ??= new object();
                                    impianto.CertificazioneImpianto ??= new object();
                                    impianto.DichiarazioneConformita ??= new object();
                                    impianto.DichiarazioneRispondenza ??= new object();
                                }

                                var stats = await SincronizzaImpiantiClimaAcs(request.Data.ImpiantiClimaAcs);
                                response.EntitiesStats["impianti_clima_acs"] = stats;
                                AggiungiAStatisticheTotali(response.Stats, stats);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Errore durante la sincronizzazione degli impianti clima/ACS");
                                response.EntitiesStats["impianti_clima_acs"] = new EntityStats { Total = request.Data.ImpiantiClimaAcs.Count, Unchanged = request.Data.ImpiantiClimaAcs.Count };
                            }
                        }

                        // Sincronizza gli impianti elettrici
                        if (request.Data.ImpiantiElettrici != null && request.Data.ImpiantiElettrici.Any())
                        {
                            try
                            {
                                // Normalizza i dati
                                foreach (var impianto in request.Data.ImpiantiElettrici)
                                {
                                    impianto.FotoContatore ??= new object();
                                    impianto.FotoGenerale ??= new object();
                                    impianto.ContrattoFornitura ??= new object();
                                    impianto.DichiarazioneConformita ??= new object();
                                    impianto.DichiarazioneRispondenza ??= new object();
                                }

                                var stats = await SincronizzaImpiantiElettrici(request.Data.ImpiantiElettrici);
                                response.EntitiesStats["impianti_elettrici"] = stats;
                                AggiungiAStatisticheTotali(response.Stats, stats);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Errore durante la sincronizzazione degli impianti elettrici");
                                response.EntitiesStats["impianti_elettrici"] = new EntityStats { Total = request.Data.ImpiantiElettrici.Count, Unchanged = request.Data.ImpiantiElettrici.Count };
                            }
                        }

                        // Sincronizza gli altri impianti
                        if (request.Data.AltriImpianti != null && request.Data.AltriImpianti.Any())
                        {
                            try
                            {
                                // Normalizza i dati
                                foreach (var impianto in request.Data.AltriImpianti)
                                {
                                    impianto.Foto ??= new object();
                                    impianto.DichiarazioneConformita ??= new object();
                                    impianto.DichiarazioneRispondenza ??= new object();
                                }

                                var stats = await SincronizzaAltriImpianti(request.Data.AltriImpianti);
                                response.EntitiesStats["altri_impianti"] = stats;
                                AggiungiAStatisticheTotali(response.Stats, stats);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Errore durante la sincronizzazione degli altri impianti");
                                response.EntitiesStats["altri_impianti"] = new EntityStats { Total = request.Data.AltriImpianti.Count, Unchanged = request.Data.AltriImpianti.Count };
                            }
                        }

                        // Sincronizza gli infissi
                        if (request.Data.Infissi != null && request.Data.Infissi.Any())
                        {
                            try
                            {
                                var stats = await SincronizzaInfissi(request.Data.Infissi);
                                response.EntitiesStats["infissi"] = stats;
                                AggiungiAStatisticheTotali(response.Stats, stats);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Errore durante la sincronizzazione degli infissi");
                                response.EntitiesStats["infissi"] = new EntityStats { Total = request.Data.Infissi.Count, Unchanged = request.Data.Infissi.Count };
                            }
                        }

                        // Sincronizza i documenti
                        if (request.Data.Documenti != null && request.Data.Documenti.Any())
                        {
                            try
                            {
                                var stats = await SincronizzaDocumenti(request.Data.Documenti);
                                response.EntitiesStats["documenti"] = stats;
                                AggiungiAStatisticheTotali(response.Stats, stats);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Errore durante la sincronizzazione dei documenti");
                                response.EntitiesStats["documenti"] = new EntityStats { Total = request.Data.Documenti.Count, Unchanged = request.Data.Documenti.Count };
                            }
                        }

                        try
                        {
                            // Salva i dati di sincronizzazione
                            var syncRecord = new SyncRecord
                            {
                                DeviceId = request.DeviceInfo?.DeviceId ?? Guid.NewGuid().ToString(),
                                DevicePlatform = request.DeviceInfo?.Platform ?? "unknown",
                                DeviceVersion = request.DeviceInfo?.Version ?? "unknown",
                                SyncedAt = DateTime.UtcNow,
                                TotalEntitiesProcessed = response.Stats.Total,
                                EntitiesCreated = response.Stats.Created,
                                EntitiesUpdated = response.Stats.Updated,
                                EntitiesDeleted = response.Stats.Deleted,
                                EntitiesUnchanged = response.Stats.Unchanged
                            };

                            _context.SyncRecords.Add(syncRecord);
                            await _context.SaveChangesAsync();

                            // Registra il record di sincronizzazione
                            await RegistraSincronizzazione(request);

                            _logger.LogInformation("Record di sincronizzazione salvato con successo");
                        }
                        catch (Exception ex)
                        {
                            // Non fallire l'intera sincronizzazione se non riusciamo a salvare il record
                            _logger.LogWarning(ex, "Impossibile salvare il record di sincronizzazione. La tabella SyncRecords potrebbe non esistere.");
                            // Continuiamo comunque, dato che i dati sono stati sincronizzati correttamente
                        }

                        await transaction.CommitAsync();
                        _logger.LogInformation($"Sincronizzazione completata con successo. Stats: {response.Stats.Created} creati, {response.Stats.Updated} aggiornati");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(ex, "Errore durante la sincronizzazione dei dati");
                        response.Success = false;
                        response.Message = "Errore durante la sincronizzazione: " + ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore generale durante la sincronizzazione");
                response.Success = false;
                response.Message = "Errore generale durante la sincronizzazione: " + ex.Message;
            }

            return response;
        }

        public async Task<SyncStatusResponse> GetSyncStatus(string deviceId)
        {
            var lastSync = await _context.SyncRecords
                .Where(s => s.DeviceId == deviceId)
                .OrderByDescending(s => s.SyncedAt)
                .FirstOrDefaultAsync();

            return new SyncStatusResponse
            {
                LastSyncTimestamp = lastSync?.SyncedAt,
                Status = lastSync != null ? "Sincronizzazione completata" : "Mai sincronizzato",
                DeviceId = deviceId
            };
        }

        public async Task<FileUploadResponse> UploadFile(FileUploadRequest request)
        {
            try
            {
                // Implementazione per il salvataggio del file
                // In una implementazione reale, qui si salverebbe il file fisicamente

                var fileRecord = new FileRecord
                {
                    FileName = request.FileName,
                    FilePath = "/uploads/" + request.FileName, // Percorso fittizio
                    FileSize = request.FileSize,
                    FileType = request.FileType,
                    UploadedAt = DateTime.UtcNow,
                    EntityId = request.EntityId,
                    EntityType = request.EntityType
                };

                _context.FileRecords.Add(fileRecord);
                await _context.SaveChangesAsync();

                return new FileUploadResponse
                {
                    Success = true,
                    FileId = fileRecord.Id,
                    FilePath = fileRecord.FilePath,
                    Message = "File caricato con successo"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il caricamento del file");
                return new FileUploadResponse
                {
                    Success = false,
                    Message = "Errore durante il caricamento del file: " + ex.Message
                };
            }
        }

        public async Task<SyncCompleteResponse> CompleteSyncFiles(SyncFilesCompleteRequest request)
        {
            try
            {
                // Aggiorna lo stato della sincronizzazione dei file
                var syncRecord = await _context.SyncRecords
                    .OrderByDescending(s => s.SyncedAt)
                    .FirstOrDefaultAsync(s => s.DeviceId == request.DeviceId);

                if (syncRecord != null)
                {
                    syncRecord.FilesSyncCompleted = true;
                    syncRecord.FilesSyncCompletedAt = DateTime.UtcNow;
                    syncRecord.FilesProcessed = request.FilesProcessed;
                    syncRecord.FilesSuccessful = request.FilesSuccessful;
                    syncRecord.FilesFailed = request.FilesFailed;

                    await _context.SaveChangesAsync();
                }

                return new SyncCompleteResponse
                {
                    Success = true,
                    Message = "Sincronizzazione file completata",
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il completamento della sincronizzazione dei file");
                return new SyncCompleteResponse
                {
                    Success = false,
                    Message = "Errore durante il completamento della sincronizzazione: " + ex.Message,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        #region Metodi privati per sincronizzazione delle entità

        private async Task<EntityStats> SincronizzaComplessi(List<ComplessoDTO> complessi)
        {
            var stats = new EntityStats();

            foreach (var complesso in complessi)
            {
                if (string.IsNullOrEmpty(complesso.Id))
                {
                    _logger.LogWarning("Trovato complesso senza ID, ignorato");
                    continue;
                }

                stats.Total++;

                try
                {
                    // Converte l'ID stringa in Guid se necessario
                    Guid complessoGuid;
                    if (!Guid.TryParse(complesso.Id, out complessoGuid))
                    {
                        // Se non è possibile convertire direttamente, creiamo un Guid deterministico basato sulla stringa
                        complessoGuid = new Guid(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(complesso.Id)));
                        _logger.LogInformation($"Convertito ID non-Guid '{complesso.Id}' in Guid deterministico {complessoGuid}");
                    }

                    // Cerca il complesso esistente
                    var existing = await _context.Complessi
                        .FirstOrDefaultAsync(c => c.Id == complessoGuid);

                    if (existing == null)
                    {
                        // Crea un nuovo complesso
                        var newComplesso = new Complesso
                        {
                            Id = complessoGuid,
                            // Usa i nomi delle proprietà corrispondenti all'entità Complesso
                            Codice = complesso.Cod,
                            Tipologia = complesso.Tipologia,
                            Priorita = ParseInt(complesso.Priorita),
                            Lotto = complesso.Lotto,
                            RegioneAnas = complesso.Regione,
                            ProvinciaAnas = complesso.Provincia,
                            StradaAnas = complesso.StradaAnas,
                            ProgressivaChilometrica = complesso.ProgressivaChilometrica,
                            ComuneAnas = complesso.ComuneAnas,
                            LocalitaAnas = complesso.LocalitaAnas,
                            ViaAnas = complesso.ViaAnas,
                            NumeroCivicoAnas = ParseInt(complesso.NumeroCivicoAnas),
                            CapAnas = ParseInt(complesso.CapAnas),
                            AgibilitaRilieviArchitettoniciAnas = complesso.AgibilitaRilieviArchitettoniciAnas,
                            CausaInagibilitaRilieviArchitettoniciAnas = complesso.CausaInagibilitaRilieviArchitettoniciAnas,
                            DataRilievo = DateTime.UtcNow // Aggiungiamo la data di creazione
                        };

                        _context.Complessi.Add(newComplesso);
                        stats.Created++;
                        _logger.LogInformation($"Creato nuovo complesso con ID: {complesso.Id}");
                    }
                    else
                    {
                        // Aggiorna complesso esistente
                        existing.Codice = complesso.Cod;
                        existing.Tipologia = complesso.Tipologia;
                        existing.Priorita = ParseInt(complesso.Priorita);
                        existing.Lotto = complesso.Lotto;
                        existing.RegioneAnas = complesso.Regione;
                        existing.ProvinciaAnas = complesso.Provincia;
                        existing.StradaAnas = complesso.StradaAnas;
                        existing.ProgressivaChilometrica = complesso.ProgressivaChilometrica;
                        existing.ComuneAnas = complesso.ComuneAnas;
                        existing.LocalitaAnas = complesso.LocalitaAnas;
                        existing.ViaAnas = complesso.ViaAnas;
                        existing.NumeroCivicoAnas = ParseInt(complesso.NumeroCivicoAnas);
                        existing.CapAnas = ParseInt(complesso.CapAnas);
                        existing.AgibilitaRilieviArchitettoniciAnas = complesso.AgibilitaRilieviArchitettoniciAnas;
                        existing.CausaInagibilitaRilieviArchitettoniciAnas = complesso.CausaInagibilitaRilieviArchitettoniciAnas;
                        existing.DataRilievo = DateTime.UtcNow; // Aggiorniamo la data di modifica

                        _context.Complessi.Update(existing);
                        stats.Updated++;
                        _logger.LogInformation($"Aggiornato complesso esistente con ID: {complesso.Id}");
                    }

                    await _context.SaveChangesAsync();

                    // Aggiorna le relazioni (strutture, impianti, ecc.)
                    // Per mappare correttamente le relazioni, dovremmo convertire gli ID string in Guid
                    if (complesso.Strutture != null && complesso.Strutture.Any())
                    {
                        var struttureGuids = new List<Guid>();
                        foreach (var strutturaId in complesso.Strutture)
                        {
                            if (Guid.TryParse(strutturaId, out Guid strutturaGuid))
                            {
                                struttureGuids.Add(strutturaGuid);
                            }
                            else
                            {
                                var deterministicGuid = new Guid(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(strutturaId)));
                                struttureGuids.Add(deterministicGuid);
                            }
                        }

                        var existingOrNew = await _context.Complessi.FindAsync(complessoGuid);
                        if (existingOrNew != null)
                        {
                            existingOrNew.StruttureIds = struttureGuids;
                            await _context.SaveChangesAsync();
                        }
                    }

                    // Qui implementeresti il codice per aggiornare altre relazioni in base alle liste di ID
                    // Ad esempio: complesso.ImpiantiIdriciAdduzione, complesso.ImpiantiScarichi, ecc.
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Errore durante la sincronizzazione del complesso con ID {complesso.Id}");
                    stats.Unchanged++;
                }
            }

            return stats;
        }

        private int? ParseInt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            if (int.TryParse(value, out int result))
                return result;

            return null;
        }

        private async Task<EntityStats> SincronizzaEdifici(List<EdificioDTO> edifici)
        {
            var stats = new EntityStats();

            foreach (var edificio in edifici)
            {
                if (string.IsNullOrEmpty(edificio.Id))
                {
                    _logger.LogWarning("Trovato edificio senza ID, ignorato");
                    continue;
                }

                stats.Total++;

                try
                {
                    // Converte l'ID stringa in Guid se necessario
                    Guid edificioGuid;
                    if (!Guid.TryParse(edificio.Id, out edificioGuid))
                    {
                        // Se non è possibile convertire direttamente, creiamo un Guid deterministico basato sulla stringa
                        edificioGuid = new Guid(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(edificio.Id)));
                        _logger.LogInformation($"Convertito ID non-Guid '{edificio.Id}' in Guid deterministico {edificioGuid}");
                    }

                    // Cerca il complesso parent se esiste
                    Guid? complessoGuid = null;
                    if (!string.IsNullOrEmpty(edificio.ComplessoId))
                    {
                        if (!Guid.TryParse(edificio.ComplessoId, out Guid parsedGuid))
                        {
                            complessoGuid = new Guid(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(edificio.ComplessoId)));
                            _logger.LogInformation($"Convertito ComplessoId non-Guid '{edificio.ComplessoId}' in Guid deterministico {complessoGuid}");
                        }
                        else
                        {
                            complessoGuid = parsedGuid;
                        }
                    }

                    // Cerca l'edificio esistente
                    var existing = await _context.Edifici
                        .FirstOrDefaultAsync(e => e.Id == edificioGuid);

                    if (existing == null)
                    {
                        // Crea un nuovo edificio
                        var newEdificio = new Edificio
                        {
                            Id = edificioGuid,
                            // Aggiungi altre proprietà qui
                            AgibilitaRilieviArchitettoniciRilevato = edificio.AgibilitaRilieviArchitettoniciRilevato,
                            CausaInagibilitaRilieviArchitettoniciRilevato = edificio.CausaInagibilitaRilieviArchitettoniciRilevato,
                            DestinazioneUsoPrevalenteRilevata = edificio.DestinazioneUsoPrevalenteRilevata,
                            LivelliFuoriTerra = edificio.LivelliFuoriTerra,
                            LivelliSeminterrati = edificio.LivelliSeminterrati,
                            LivelliInterrati = edificio.LivelliInterrati,
                            CorpiScala = edificio.CorpiScala,
                            TipologiaEdilizia = edificio.TipologiaEdilizia,
                            PosizioneRispettoAiFabbricatiCircostanti = edificio.PosizioneRispettoAiFabbricati,
                            Note = edificio.Note,
                            Sezione = edificio.Sezione,
                            Foglio = edificio.Foglio,
                            Particella = edificio.Particella,
                            TipoCatasto = edificio.TipoCatasto,
                            UltimoInterventoFabbricato = edificio.UltimoInterventoFabbricato,
                            TipologiaStrutturale = edificio.TipologiaStrutturale
                        };

                        _context.Edifici.Add(newEdificio);
                        stats.Created++;
                        _logger.LogInformation($"Creato nuovo edificio con ID: {edificio.Id}");
                    }
                    else
                    {
                        // Aggiorna edificio esistente
                        existing.AgibilitaRilieviArchitettoniciRilevato = edificio.AgibilitaRilieviArchitettoniciRilevato;
                        existing.CausaInagibilitaRilieviArchitettoniciRilevato = edificio.CausaInagibilitaRilieviArchitettoniciRilevato;
                        existing.DestinazioneUsoPrevalenteRilevata = edificio.DestinazioneUsoPrevalenteRilevata;
                        existing.LivelliFuoriTerra = edificio.LivelliFuoriTerra;
                        existing.LivelliSeminterrati = edificio.LivelliSeminterrati;
                        existing.LivelliInterrati = edificio.LivelliInterrati;
                        existing.CorpiScala = edificio.CorpiScala;
                        existing.TipologiaEdilizia = edificio.TipologiaEdilizia;
                        existing.PosizioneRispettoAiFabbricatiCircostanti = edificio.PosizioneRispettoAiFabbricati;
                        existing.Note = edificio.Note;
                        existing.Sezione = edificio.Sezione;
                        existing.Foglio = edificio.Foglio;
                        existing.Particella = edificio.Particella;
                        existing.TipoCatasto = edificio.TipoCatasto;
                        existing.UltimoInterventoFabbricato = edificio.UltimoInterventoFabbricato;
                        existing.TipologiaStrutturale = edificio.TipologiaStrutturale;

                        _context.Edifici.Update(existing);
                        stats.Updated++;
                        _logger.LogInformation($"Aggiornato edificio esistente con ID: {edificio.Id}");
                    }

                    await _context.SaveChangesAsync();

                    // Aggiorna le relazioni (unità immobiliari, strutture, impianti, ecc.)
                    // Per mappare correttamente le relazioni, dovremmo convertire gli ID string in Guid
                    if (edificio.UnitaImmobiliari != null && edificio.UnitaImmobiliari.Any())
                    {
                        var unitaGuids = new List<Guid>();
                        foreach (var unitaId in edificio.UnitaImmobiliari)
                        {
                            if (Guid.TryParse(unitaId, out Guid unitaGuid))
                            {
                                unitaGuids.Add(unitaGuid);
                            }
                            else
                            {
                                var deterministicGuid = new Guid(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(unitaId)));
                                unitaGuids.Add(deterministicGuid);
                            }
                        }

                        var existingOrNew = await _context.Edifici.FindAsync(edificioGuid);
                        if (existingOrNew != null)
                        {
                            existingOrNew.UnitaImmobiliariIds = unitaGuids;
                            await _context.SaveChangesAsync();
                        }
                    }

                    // Gestisci altre relazioni in modo simile
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Errore durante la sincronizzazione dell'edificio con ID {edificio.Id}");
                    stats.Unchanged++;
                }
            }

            return stats;
        }

        private async Task<EntityStats> SincronizzaUnitaImmobiliari(List<UnitaImmobiliareDTO> unitaImmobiliari)
        {
            var stats = new EntityStats();

            foreach (var unita in unitaImmobiliari)
            {
                if (string.IsNullOrEmpty(unita.Id))
                {
                    _logger.LogWarning("Trovata unità immobiliare senza ID, ignorata");
                    continue;
                }

                stats.Total++;

                try
                {
                    // Converte l'ID stringa in Guid se necessario
                    Guid unitaGuid;
                    if (!Guid.TryParse(unita.Id, out unitaGuid))
                    {
                        // Se non è possibile convertire direttamente, creiamo un Guid deterministico basato sulla stringa
                        unitaGuid = new Guid(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(unita.Id)));
                        _logger.LogInformation($"Convertito ID non-Guid '{unita.Id}' in Guid deterministico {unitaGuid}");
                    }

                    // Cerca l'unità immobiliare esistente
                    var existing = await _context.UnitaImmobiliari
                        .FirstOrDefaultAsync(u => u.Id == unitaGuid);

                    if (existing == null)
                    {
                        // Crea una nuova unità immobiliare
                        var newUnita = new UnitaImmobiliare
                        {
                            Id = unitaGuid,
                            // Mapping delle proprietà
                            AgibilitaRilieviArchitettoniciRilevato = unita.AgibilitaRilieviArchitettoniciRilevato,
                            CausaInagibilitaRilieviArchitettoniciRilevato = unita.CausaInagibilitaRilieviArchitettoniciRilevato,
                            Scala = unita.Scala,
                            Piano = unita.Piano,
                            Interno = unita.Interno,
                            StatoOccupazioneImmobile = unita.StatoOccupazioneImmobile,
                            StatoOccupazioneImmobileDescrizione = unita.StatoOccupazioneImmobileDescrizione,
                            NomeConduttoreEffettivo = unita.NomeConduttoreEffettivo,
                            DestinazioneUsoRilevata = unita.DestinazioneUsoRilevata,
                            Note = unita.Note,
                            Subalterno = unita.Subalterno,
                            CategoriaCatastale = unita.CategoriaCatastale,
                            TipoUso = unita.TipoUso,
                            CategoriaAttivita = unita.CategoriaAttivita
                        };

                        _context.UnitaImmobiliari.Add(newUnita);
                        stats.Created++;
                        _logger.LogInformation($"Creata nuova unità immobiliare con ID: {unita.Id}");
                    }
                    else
                    {
                        // Aggiorna unità immobiliare esistente
                        existing.AgibilitaRilieviArchitettoniciRilevato = unita.AgibilitaRilieviArchitettoniciRilevato;
                        existing.CausaInagibilitaRilieviArchitettoniciRilevato = unita.CausaInagibilitaRilieviArchitettoniciRilevato;
                        existing.Scala = unita.Scala;
                        existing.Piano = unita.Piano;
                        existing.Interno = unita.Interno;
                        existing.StatoOccupazioneImmobile = unita.StatoOccupazioneImmobile;
                        existing.StatoOccupazioneImmobileDescrizione = unita.StatoOccupazioneImmobileDescrizione;
                        existing.NomeConduttoreEffettivo = unita.NomeConduttoreEffettivo;
                        existing.DestinazioneUsoRilevata = unita.DestinazioneUsoRilevata;
                        existing.Note = unita.Note;
                        existing.Subalterno = unita.Subalterno;
                        existing.CategoriaCatastale = unita.CategoriaCatastale;
                        existing.TipoUso = unita.TipoUso;
                        existing.CategoriaAttivita = unita.CategoriaAttivita;

                        _context.UnitaImmobiliari.Update(existing);
                        stats.Updated++;
                        _logger.LogInformation($"Aggiornata unità immobiliare esistente con ID: {unita.Id}");
                    }

                    await _context.SaveChangesAsync();

                    // TODO: Aggiornare relazioni come fatto per edifici
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Errore durante la sincronizzazione dell'unità immobiliare con ID {unita.Id}");
                    stats.Unchanged++;
                }
            }

            return stats;
        }

        private async Task<EntityStats> SincronizzaStrutture(List<StruttureDTO> strutture)
        {
            var stats = new EntityStats();

            foreach (var struttura in strutture)
            {
                if (string.IsNullOrEmpty(struttura.Id))
                {
                    _logger.LogWarning("Trovata struttura senza ID, ignorata");
                    continue;
                }

                stats.Total++;

                try
                {
                    // Converte l'ID stringa in Guid se necessario
                    Guid strutturaGuid;
                    if (!Guid.TryParse(struttura.Id, out strutturaGuid))
                    {
                        // Se non è possibile convertire direttamente, creiamo un Guid deterministico basato sulla stringa
                        strutturaGuid = new Guid(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(struttura.Id)));
                        _logger.LogInformation($"Convertito ID non-Guid '{struttura.Id}' in Guid deterministico {strutturaGuid}");
                    }

                    // Converti gli ID dei parent se presenti
                    Guid? parentGuid = null;
                    if (!string.IsNullOrEmpty(struttura.ParentId))
                    {
                        if (!Guid.TryParse(struttura.ParentId, out Guid parsedGuid))
                        {
                            parentGuid = new Guid(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(struttura.ParentId)));
                            _logger.LogInformation($"Convertito ParentId ({struttura.ParentType}) non-Guid '{struttura.ParentId}' in Guid deterministico {parentGuid}");
                        }
                        else
                        {
                            parentGuid = parsedGuid;
                        }
                    }

                    // Cerca la struttura esistente
                    var existing = await _context.Strutture
                        .FirstOrDefaultAsync(s => s.Id == strutturaGuid);

                    if (existing == null)
                    {
                        // Crea una nuova struttura
                        var newStruttura = new Strutture
                        {
                            Id = strutturaGuid,
                            // Mapping delle proprietà
                            Codice = struttura.Codice,
                            Posizione = struttura.Posizione,
                            TipologiaStruttura = struttura.TipologiaStruttura,
                            FinituraInterna = struttura.FinituraInterna,
                            FinituraEsterna = struttura.FinituraEsterna,
                            StatoConservativo = struttura.StatoConservativo,
                            SottotipologiaStruttura = struttura.SottotipologiaStruttura
                        };

                        // Poiché Strutture non ha direttamente le relazioni, 
                        // dobbiamo gestirle separatamente aggiungendo la struttura
                        // alle liste appropriate dei parent
                        decimal spessoreLordoValue;
                        if (decimal.TryParse(struttura.SpessoreLordo, out spessoreLordoValue))
                        {
                            newStruttura.SpessoreLordo = spessoreLordoValue;
                        }

                        _context.Strutture.Add(newStruttura);
                        await _context.SaveChangesAsync();

                        // Gestisci le relazioni con parent
                        await AggiungiStrutturaAlParent(strutturaGuid, struttura.ParentType, parentGuid);

                        stats.Created++;
                        _logger.LogInformation($"Creata nuova struttura con ID: {struttura.Id}");
                    }
                    else
                    {
                        // Aggiorna struttura esistente
                        existing.Codice = struttura.Codice;
                        existing.Posizione = struttura.Posizione;
                        existing.TipologiaStruttura = struttura.TipologiaStruttura;
                        existing.FinituraInterna = struttura.FinituraInterna;
                        existing.FinituraEsterna = struttura.FinituraEsterna;
                        existing.StatoConservativo = struttura.StatoConservativo;
                        existing.SottotipologiaStruttura = struttura.SottotipologiaStruttura;

                        decimal spessoreLordoValue;
                        if (decimal.TryParse(struttura.SpessoreLordo, out spessoreLordoValue))
                        {
                            existing.SpessoreLordo = spessoreLordoValue;
                        }

                        _context.Strutture.Update(existing);
                        await _context.SaveChangesAsync();

                        // Gestisci le relazioni con parent
                        await AggiungiStrutturaAlParent(strutturaGuid, struttura.ParentType, parentGuid);

                        stats.Updated++;
                        _logger.LogInformation($"Aggiornata struttura esistente con ID: {struttura.Id}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Errore durante la sincronizzazione della struttura con ID {struttura.Id}");
                    stats.Unchanged++;
                }
            }

            return stats;
        }

        // Helper method to add structure to its parent entity
        private async Task AggiungiStrutturaAlParent(Guid strutturaId, string parentType, Guid? parentId)
        {
            if (!parentId.HasValue || string.IsNullOrEmpty(parentType))
                return;

            try
            {
                // Add the structure ID to the appropriate parent's list
                switch (parentType.ToLower())
                {
                    case "complesso":
                        var complesso = await _context.Complessi.FindAsync(parentId.Value);
                        if (complesso != null)
                        {
                            complesso.StruttureIds ??= new List<Guid>();
                            if (!complesso.StruttureIds.Contains(strutturaId))
                            {
                                complesso.StruttureIds.Add(strutturaId);
                                await _context.SaveChangesAsync();
                            }
                        }
                        break;

                    case "edificio":
                        var edificio = await _context.Edifici.FindAsync(parentId.Value);
                        if (edificio != null)
                        {
                            edificio.StruttureIds ??= new List<Guid>();
                            if (!edificio.StruttureIds.Contains(strutturaId))
                            {
                                edificio.StruttureIds.Add(strutturaId);
                                await _context.SaveChangesAsync();
                            }
                        }
                        break;

                        // Other types as needed
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante l'aggiunta della struttura {strutturaId} al parent {parentType} con ID {parentId}");
            }
        }

        private async Task<EntityStats> SincronizzaImpiantiIdrici(List<ImpiantoIdricoDTO> impiantiIdrici)
        {
            var stats = new EntityStats();

            // Create a list to store valid Guids
            var validImpiantiIds = new List<Guid>();
            var idMapping = new Dictionary<string, Guid>();

            // Process IDs first, separating valid Guids from mobile-generated ones
            foreach (var impiantoDto in impiantiIdrici)
            {
                Guid impiantoGuid;
                if (TryParseGuidWithPrefix(impiantoDto.Id, "II-", out impiantoGuid))
                {
                    validImpiantiIds.Add(impiantoGuid);
                    idMapping[impiantoDto.Id] = impiantoGuid;
                }
                else
                {
                    // For mobile-generated IDs, create a new GUID
                    var newGuid = Guid.NewGuid();
                    validImpiantiIds.Add(newGuid);
                    idMapping[impiantoDto.Id] = newGuid;
                }
            }

            // Carica gli impianti idrici esistenti
            var existingImpianti = await _context.IdraulicoAdduzione
                .Where(i => validImpiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impiantoDto in impiantiIdrici)
            {
                // Get the mapped GUID for this ID
                var impiantoId = idMapping[impiantoDto.Id];
                var impianto = existingImpianti.FirstOrDefault(i => i.Id == impiantoId);

                stats.Total++;

                if (impianto == null)
                {
                    // Normalizza i dati
                    if (impiantoDto.Foto == null)
                    {
                        impiantoDto.Foto = "[]";
                    }

                    // Crea nuovo impianto idrico
                    impianto = new IdraulicoAdduzione
                    {
                        Id = impiantoId,
                        // Altre proprietà mappate dal DTO
                        // ...
                    };

                    // Gestisci le relazioni in base al tipo di parent
                    if (impiantoDto.ParentId != null)
                    {
                        switch (impiantoDto.ParentType?.ToLower())
                        {
                            case "complesso":
                                var complessoId = Guid.Parse(impiantoDto.ParentId.Replace("C-", ""));
                                var complesso = await _context.Complessi.FindAsync(complessoId);
                                if (complesso != null)
                                {
                                    complesso.IdraulicoAdduzioneIds ??= new List<Guid>();
                                    if (!complesso.IdraulicoAdduzioneIds.Contains(impiantoId))
                                    {
                                        complesso.IdraulicoAdduzioneIds.Add(impiantoId);
                                    }
                                }
                                break;
                            case "edificio":
                                var edificioId = Guid.Parse(impiantoDto.ParentId.Replace("E-", ""));
                                // Gestisci la relazione con l'edificio
                                break;
                            case "unita":
                                var unitaId = Guid.Parse(impiantoDto.ParentId.Replace("UI-", ""));
                                // Gestisci la relazione con l'unità immobiliare
                                break;
                        }
                    }

                    _context.IdraulicoAdduzione.Add(impianto);
                    stats.Created++;
                }
                else
                {
                    // Aggiorna impianto esistente
                    bool modificato = false;

                    // Verifica cambiamenti proprietà e relazioni

                    if (modificato)
                    {
                        stats.Updated++;
                    }
                    else
                    {
                        stats.Unchanged++;
                    }
                }
            }

            // Verifica se ci sono impianti da eliminare
            var impiantiDaEliminare = await _context.IdraulicoAdduzione
                .Where(i => !validImpiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impianto in impiantiDaEliminare)
            {
                _context.IdraulicoAdduzione.Remove(impianto);
                stats.Deleted++;
            }

            await _context.SaveChangesAsync();
            stats.Total = stats.Created + stats.Updated + stats.Deleted + stats.Unchanged;

            return stats;
        }

        private async Task<EntityStats> SincronizzaImpiantiScarichi(List<ImpiantoScarichiDTO> impiantiScarichi)
        {
            var stats = new EntityStats();

            // Create a list to store valid Guids
            var validImpiantiIds = new List<Guid>();
            var idMapping = new Dictionary<string, Guid>();

            // Process IDs first, separating valid Guids from mobile-generated ones
            foreach (var impiantoDto in impiantiScarichi)
            {
                Guid impiantoGuid;
                if (TryParseGuidWithPrefix(impiantoDto.Id, "IS-", out impiantoGuid))
                {
                    validImpiantiIds.Add(impiantoGuid);
                    idMapping[impiantoDto.Id] = impiantoGuid;
                }
                else
                {
                    // For mobile-generated IDs, create a new GUID
                    var newGuid = Guid.NewGuid();
                    validImpiantiIds.Add(newGuid);
                    idMapping[impiantoDto.Id] = newGuid;
                }
            }

            // Carica gli impianti scarichi esistenti
            var existingImpianti = await _context.ScarichiIdriciFognari
                .Where(i => validImpiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impiantoDto in impiantiScarichi)
            {
                // Get the mapped GUID for this ID
                var impiantoId = idMapping[impiantoDto.Id];
                var impianto = existingImpianti.FirstOrDefault(i => i.Id == impiantoId);

                stats.Total++;

                if (impianto == null)
                {
                    // Normalizza i dati
                    if (impiantoDto.Foto == null)
                    {
                        impiantoDto.Foto = new List<object>();
                    }

                    // Crea nuovo impianto scarichi
                    impianto = new ScarichiIdriciFognari
                    {
                        Id = impiantoId,
                        // Altre proprietà mappate dal DTO
                        // ...
                    };

                    // Gestisci le relazioni in base al tipo di parent
                    if (impiantoDto.ParentId != null)
                    {
                        switch (impiantoDto.ParentType?.ToLower())
                        {
                            case "complesso":
                                var complessoId = Guid.Parse(impiantoDto.ParentId.Replace("C-", ""));
                                var complesso = await _context.Complessi.FindAsync(complessoId);
                                if (complesso != null)
                                {
                                    complesso.ScarichiIdriciFognariIds ??= new List<Guid>();
                                    if (!complesso.ScarichiIdriciFognariIds.Contains(impiantoId))
                                    {
                                        complesso.ScarichiIdriciFognariIds.Add(impiantoId);
                                    }
                                }
                                break;
                            case "edificio":
                                var edificioId = Guid.Parse(impiantoDto.ParentId.Replace("E-", ""));
                                // Gestisci la relazione con l'edificio
                                break;
                            case "unita":
                                var unitaId = Guid.Parse(impiantoDto.ParentId.Replace("UI-", ""));
                                // Gestisci la relazione con l'unità immobiliare
                                break;
                        }
                    }

                    _context.ScarichiIdriciFognari.Add(impianto);
                    stats.Created++;
                }
                else
                {
                    // Aggiorna impianto esistente
                    bool modificato = false;

                    // Verifica cambiamenti proprietà e relazioni

                    if (modificato)
                    {
                        stats.Updated++;
                    }
                    else
                    {
                        stats.Unchanged++;
                    }
                }
            }

            // Verifica se ci sono impianti da eliminare
            var impiantiDaEliminare = await _context.ScarichiIdriciFognari
                .Where(i => !validImpiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impianto in impiantiDaEliminare)
            {
                _context.ScarichiIdriciFognari.Remove(impianto);
                stats.Deleted++;
            }

            await _context.SaveChangesAsync();
            stats.Total = stats.Created + stats.Updated + stats.Deleted + stats.Unchanged;

            return stats;
        }

        private async Task<EntityStats> SincronizzaImpiantiClimaAcs(List<ImpiantoClimaAcsDTO> impiantiClimaAcs)
        {
            var stats = new EntityStats();

            // Create a list to store valid Guids
            var validImpiantiIds = new List<Guid>();
            var idMapping = new Dictionary<string, Guid>();

            // Process IDs first, separating valid Guids from mobile-generated ones
            foreach (var impiantoDto in impiantiClimaAcs)
            {
                Guid impiantoGuid;
                if (TryParseGuidWithPrefix(impiantoDto.Id, "ICA-", out impiantoGuid))
                {
                    validImpiantiIds.Add(impiantoGuid);
                    idMapping[impiantoDto.Id] = impiantoGuid;
                }
                else
                {
                    // For mobile-generated IDs, create a new GUID
                    var newGuid = Guid.NewGuid();
                    validImpiantiIds.Add(newGuid);
                    idMapping[impiantoDto.Id] = newGuid;
                }
            }

            // Carica gli impianti clima ACS esistenti
            var existingImpianti = await _context.ImpiantoClimaAcs
                .Where(i => validImpiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impiantoDto in impiantiClimaAcs)
            {
                // Get the mapped GUID for this ID
                var impiantoId = idMapping[impiantoDto.Id];
                var impianto = existingImpianti.FirstOrDefault(i => i.Id == impiantoId);

                stats.Total++;

                if (impianto == null)
                {
                    // Normalizza i dati
                    if (impiantoDto.Foto == null)
                    {
                        impiantoDto.Foto = "[]";
                    }
                    if (impiantoDto.CertificazioneImpianto == null)
                    {
                        impiantoDto.CertificazioneImpianto = "[]";
                    }
                    if (impiantoDto.DichiarazioneConformita == null)
                    {
                        impiantoDto.DichiarazioneConformita = "[]";
                    }
                    if (impiantoDto.DichiarazioneRispondenza == null)
                    {
                        impiantoDto.DichiarazioneRispondenza = "[]";
                    }

                    // Crea nuovo impianto clima ACS
                    impianto = new ImpiantoClimaAcs
                    {
                        Id = impiantoId,
                        // Altre proprietà mappate dal DTO
                        // ...
                    };

                    // Gestisci le relazioni in base al tipo di parent
                    if (impiantoDto.ParentId != null)
                    {
                        switch (impiantoDto.ParentType?.ToLower())
                        {
                            case "complesso":
                                var complessoId = Guid.Parse(impiantoDto.ParentId.Replace("C-", ""));
                                var complesso = await _context.Complessi.FindAsync(complessoId);
                                if (complesso != null)
                                {
                                    complesso.ImpiantoClimaAcsIds ??= new List<Guid>();
                                    if (!complesso.ImpiantoClimaAcsIds.Contains(impiantoId))
                                    {
                                        complesso.ImpiantoClimaAcsIds.Add(impiantoId);
                                    }
                                }
                                break;
                            case "edificio":
                            case "unita":
                                // Gestisci le relazioni con edificio/unità immobiliare
                                break;
                        }
                    }

                    _context.ImpiantoClimaAcs.Add(impianto);
                    stats.Created++;
                }
                else
                {
                    // Aggiorna impianto esistente
                    bool modificato = false;
                    // Implementa aggiornamenti
                    if (modificato)
                    {
                        stats.Updated++;
                    }
                    else
                    {
                        stats.Unchanged++;
                    }
                }
            }

            // Verifica se ci sono impianti da eliminare
            var impiantiDaEliminare = await _context.ImpiantoClimaAcs
                .Where(i => !validImpiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impianto in impiantiDaEliminare)
            {
                _context.ImpiantoClimaAcs.Remove(impianto);
                stats.Deleted++;
            }

            await _context.SaveChangesAsync();
            stats.Total = stats.Created + stats.Updated + stats.Deleted + stats.Unchanged;

            return stats;
        }

        private async Task<EntityStats> SincronizzaImpiantiElettrici(List<ImpiantoElettricoDTO> impiantiElettrici)
        {
            var stats = new EntityStats();

            // Create a list to store valid Guids
            var validImpiantiIds = new List<Guid>();
            var idMapping = new Dictionary<string, Guid>();

            // Process IDs first, separating valid Guids from mobile-generated ones
            foreach (var impiantoDto in impiantiElettrici)
            {
                Guid impiantoGuid;
                if (TryParseGuidWithPrefix(impiantoDto.Id, "IE-", out impiantoGuid))
                {
                    validImpiantiIds.Add(impiantoGuid);
                    idMapping[impiantoDto.Id] = impiantoGuid;
                }
                else
                {
                    // For mobile-generated IDs, create a new GUID
                    var newGuid = Guid.NewGuid();
                    validImpiantiIds.Add(newGuid);
                    idMapping[impiantoDto.Id] = newGuid;
                }
            }

            // Carica gli impianti elettrici esistenti
            var existingImpianti = await _context.ImpiantiElettrici
                .Where(i => validImpiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impiantoDto in impiantiElettrici)
            {
                // Get the mapped GUID for this ID
                var impiantoId = idMapping[impiantoDto.Id];
                var impianto = existingImpianti.FirstOrDefault(i => i.Id == impiantoId);

                stats.Total++;

                if (impianto == null)
                {
                    // Normalizza i dati
                    if (impiantoDto.FotoContatore == null)
                    {
                        impiantoDto.FotoContatore = "[]";
                    }
                    if (impiantoDto.FotoGenerale == null)
                    {
                        impiantoDto.FotoGenerale = "[]";
                    }
                    if (impiantoDto.ContrattoFornitura == null)
                    {
                        impiantoDto.ContrattoFornitura = "[]";
                    }
                    if (impiantoDto.DichiarazioneConformita == null)
                    {
                        impiantoDto.DichiarazioneConformita = "[]";
                    }
                    if (impiantoDto.DichiarazioneRispondenza == null)
                    {
                        impiantoDto.DichiarazioneRispondenza = "[]";
                    }

                    // Crea nuovo impianto elettrico
                    impianto = new ImpiantiElettrici
                    {
                        Id = impiantoId,
                        // Altre proprietà mappate dal DTO
                        // ...
                    };

                    // Gestisci le relazioni in base al tipo di parent
                    if (impiantoDto.ParentId != null)
                    {
                        switch (impiantoDto.ParentType?.ToLower())
                        {
                            case "complesso":
                                var complessoId = Guid.Parse(impiantoDto.ParentId.Replace("C-", ""));
                                var complesso = await _context.Complessi.FindAsync(complessoId);
                                if (complesso != null)
                                {
                                    complesso.ImpiantiElettriciIds ??= new List<Guid>();
                                    if (!complesso.ImpiantiElettriciIds.Contains(impiantoId))
                                    {
                                        complesso.ImpiantiElettriciIds.Add(impiantoId);
                                    }
                                }
                                break;
                            case "edificio":
                            case "unita":
                                // Gestisci le relazioni con edificio/unità immobiliare
                                break;
                        }
                    }

                    _context.ImpiantiElettrici.Add(impianto);
                    stats.Created++;
                }
                else
                {
                    // Aggiorna impianto esistente
                    bool modificato = false;
                    // Implementa aggiornamenti
                    if (modificato)
                    {
                        stats.Updated++;
                    }
                    else
                    {
                        stats.Unchanged++;
                    }
                }
            }

            // Verifica se ci sono impianti da eliminare
            var impiantiDaEliminare = await _context.ImpiantiElettrici
                .Where(i => !validImpiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impianto in impiantiDaEliminare)
            {
                _context.ImpiantiElettrici.Remove(impianto);
                stats.Deleted++;
            }

            await _context.SaveChangesAsync();
            stats.Total = stats.Created + stats.Updated + stats.Deleted + stats.Unchanged;

            return stats;
        }

        private async Task<EntityStats> SincronizzaAltriImpianti(List<AltroImpiantoDTO> altriImpianti)
        {
            var stats = new EntityStats();

            // Create a list to store valid Guids
            var validImpiantiIds = new List<Guid>();
            var idMapping = new Dictionary<string, Guid>();

            // Process IDs first, separating valid Guids from mobile-generated ones
            foreach (var impiantoDto in altriImpianti)
            {
                Guid impiantoGuid;
                if (TryParseGuidWithPrefix(impiantoDto.Id, "AI-", out impiantoGuid))
                {
                    validImpiantiIds.Add(impiantoGuid);
                    idMapping[impiantoDto.Id] = impiantoGuid;
                }
                else
                {
                    // For mobile-generated IDs, create a new GUID
                    var newGuid = Guid.NewGuid();
                    validImpiantiIds.Add(newGuid);
                    idMapping[impiantoDto.Id] = newGuid;
                }
            }

            // Carica gli altri impianti esistenti
            var existingImpianti = await _context.AltriImpianti
                .Where(i => validImpiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impiantoDto in altriImpianti)
            {
                // Get the mapped GUID for this ID
                var impiantoId = idMapping[impiantoDto.Id];
                var impianto = existingImpianti.FirstOrDefault(i => i.Id == impiantoId);

                stats.Total++;

                if (impianto == null)
                {
                    // Normalizza i dati
                    if (impiantoDto.Foto == null)
                    {
                        impiantoDto.Foto = new object();
                    }
                    if (impiantoDto.DichiarazioneConformita == null)
                    {
                        impiantoDto.DichiarazioneConformita = new object();
                    }
                    if (impiantoDto.DichiarazioneRispondenza == null)
                    {
                        impiantoDto.DichiarazioneRispondenza = new object();
                    }

                    // Crea nuovo altro impianto
                    impianto = new AltriImpianti
                    {
                        Id = impiantoId,
                        // Altre proprietà mappate dal DTO
                        // ...
                    };

                    // Gestisci le relazioni in base al tipo di parent
                    if (impiantoDto.ParentId != null)
                    {
                        switch (impiantoDto.ParentType?.ToLower())
                        {
                            case "complesso":
                                var complessoId = Guid.Parse(impiantoDto.ParentId.Replace("C-", ""));
                                var complesso = await _context.Complessi.FindAsync(complessoId);
                                if (complesso != null)
                                {
                                    complesso.AltriImpiantiIds ??= new List<Guid>();
                                    if (!complesso.AltriImpiantiIds.Contains(impiantoId))
                                    {
                                        complesso.AltriImpiantiIds.Add(impiantoId);
                                    }
                                }
                                break;
                            case "edificio":
                            case "unita":
                                // Gestisci le relazioni con edificio/unità immobiliare
                                break;
                        }
                    }

                    _context.AltriImpianti.Add(impianto);
                    stats.Created++;
                }
                else
                {
                    // Aggiorna impianto esistente
                    bool modificato = false;
                    // Implementa aggiornamenti
                    if (modificato)
                    {
                        stats.Updated++;
                    }
                    else
                    {
                        stats.Unchanged++;
                    }
                }
            }

            // Verifica se ci sono impianti da eliminare
            var impiantiDaEliminare = await _context.AltriImpianti
                .Where(i => !validImpiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impianto in impiantiDaEliminare)
            {
                _context.AltriImpianti.Remove(impianto);
                stats.Deleted++;
            }

            await _context.SaveChangesAsync();
            stats.Total = stats.Created + stats.Updated + stats.Deleted + stats.Unchanged;

            return stats;
        }

        private async Task<EntityStats> SincronizzaInfissi(List<InfissoDTO> infissi)
        {
            var stats = new EntityStats();

            // Create a list to store valid Guids
            var validInfissiIds = new List<Guid>();
            var idMapping = new Dictionary<string, Guid>();

            // Process IDs first, separating valid Guids from mobile-generated ones
            foreach (var infissoDto in infissi)
            {
                Guid infissoGuid;
                if (TryParseGuidWithPrefix(infissoDto.Id, "IN-", out infissoGuid))
                {
                    validInfissiIds.Add(infissoGuid);
                    idMapping[infissoDto.Id] = infissoGuid;
                }
                else
                {
                    // For mobile-generated IDs, create a new GUID
                    var newGuid = Guid.NewGuid();
                    validInfissiIds.Add(newGuid);
                    idMapping[infissoDto.Id] = newGuid;
                }
            }

            // Carica gli infissi esistenti
            var existingInfissi = await _context.Infissi
                .Where(i => validInfissiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var infissoDto in infissi)
            {
                // Get the mapped GUID for this ID
                var infissoId = idMapping[infissoDto.Id];
                var infisso = existingInfissi.FirstOrDefault(i => i.Id == infissoId);

                stats.Total++;

                if (infisso == null)
                {
                    // Crea nuovo infisso
                    infisso = new Infissi
                    {
                        Id = infissoId,
                        // Altre proprietà mappate dal DTO
                    };

                    // Gestisci le relazioni in base al tipo di parent
                    if (infissoDto.ParentId != null)
                    {
                        switch (infissoDto.ParentType?.ToLower())
                        {
                            case "edificio":
                                var edificioId = Guid.Parse(infissoDto.ParentId.Replace("E-", ""));
                                // Gestisci la relazione con l'edificio
                                break;
                            case "unita":
                                var unitaId = Guid.Parse(infissoDto.ParentId.Replace("UI-", ""));
                                // Gestisci la relazione con l'unità immobiliare
                                break;
                        }
                    }

                    _context.Infissi.Add(infisso);
                    stats.Created++;
                }
                else
                {
                    // Aggiorna infisso esistente
                    bool modificato = false;
                    // Implementa aggiornamenti
                    if (modificato)
                    {
                        stats.Updated++;
                    }
                    else
                    {
                        stats.Unchanged++;
                    }
                }
            }

            // Verifica se ci sono infissi da eliminare
            var infissiDaEliminare = await _context.Infissi
                .Where(i => !validInfissiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var infisso in infissiDaEliminare)
            {
                _context.Infissi.Remove(infisso);
                stats.Deleted++;
            }

            await _context.SaveChangesAsync();
            stats.Total = stats.Created + stats.Updated + stats.Deleted + stats.Unchanged;

            return stats;
        }

        private async Task<EntityStats> SincronizzaDocumenti(List<DocumentoDTO> documenti)
        {
            var stats = new EntityStats();

            // Create a list to store valid Guids
            var validDocumentiIds = new List<Guid>();
            var idMapping = new Dictionary<string, Guid>();

            // Process IDs first, separating valid Guids from mobile-generated ones
            foreach (var documentoDto in documenti)
            {
                Guid documentoGuid;
                if (TryParseGuidWithPrefix(documentoDto.Id, "D-", out documentoGuid))
                {
                    validDocumentiIds.Add(documentoGuid);
                    idMapping[documentoDto.Id] = documentoGuid;
                }
                else
                {
                    // For mobile-generated IDs, create a new GUID
                    var newGuid = Guid.NewGuid();
                    validDocumentiIds.Add(newGuid);
                    idMapping[documentoDto.Id] = newGuid;
                }
            }

            // Carica i documenti esistenti
            var existingDocumenti = await _context.DocumentiGenerali
                .Where(d => validDocumentiIds.Contains(d.Id))
                .ToListAsync();

            foreach (var documentoDto in documenti)
            {
                // Get the mapped GUID for this ID
                var documentoId = idMapping[documentoDto.Id];
                var documento = existingDocumenti.FirstOrDefault(d => d.Id == documentoId);

                stats.Total++;

                if (documento == null)
                {
                    // Crea nuovo documento
                    documento = new DocumentiGenerali
                    {
                        Id = documentoId,
                        // Altre proprietà mappate dal DTO
                    };

                    // Gestisci le relazioni in base al tipo di parent
                    if (documentoDto.ParentId != null)
                    {
                        switch (documentoDto.ParentType?.ToLower())
                        {
                            case "complesso":
                                var complessoId = Guid.Parse(documentoDto.ParentId.Replace("C-", ""));
                                var complesso = await _context.Complessi.FindAsync(complessoId);
                                if (complesso != null)
                                {
                                    complesso.DocumentiGeneraliIds ??= new List<Guid>();
                                    if (!complesso.DocumentiGeneraliIds.Contains(documentoId))
                                    {
                                        complesso.DocumentiGeneraliIds.Add(documentoId);
                                    }
                                }
                                break;
                            case "edificio":
                            case "unita":
                                // Gestisci le relazioni con edificio/unità immobiliare
                                break;
                        }
                    }

                    _context.DocumentiGenerali.Add(documento);
                    stats.Created++;
                }
                else
                {
                    // Aggiorna documento esistente
                    bool modificato = false;
                    // Implementa aggiornamenti
                    if (modificato)
                    {
                        stats.Updated++;
                    }
                    else
                    {
                        stats.Unchanged++;
                    }
                }
            }

            // Verifica se ci sono documenti da eliminare
            var documentiDaEliminare = await _context.DocumentiGenerali
                .Where(d => !validDocumentiIds.Contains(d.Id))
                .ToListAsync();

            foreach (var documento in documentiDaEliminare)
            {
                _context.DocumentiGenerali.Remove(documento);
                stats.Deleted++;
            }

            await _context.SaveChangesAsync();
            stats.Total = stats.Created + stats.Updated + stats.Deleted + stats.Unchanged;

            return stats;
        }

        private void AggiungiAStatisticheTotali(SyncStats stats, EntityStats entityStats)
        {
            stats.Total += entityStats.Total;
            stats.Created += entityStats.Created;
            stats.Updated += entityStats.Updated;
            stats.Deleted += entityStats.Deleted;
            stats.Unchanged += entityStats.Unchanged;
        }

        // Verifica se il formato dei dati necessita di conversione
        private bool NeedsFormatConversion(SyncData data)
        {
            // Verifica se il formato dei dati è quello dell'app mobile
            // Questo può essere personalizzato in base alla struttura del tuo modello
            if (data == null) return false;

            // Verifica se il formato ha gli stessi campi del DTO
            // Ad esempio, verifica se l'oggetto Complessi dell'app ha un formato diverso
            if (data.Complessi != null && data.Complessi.Any())
            {
                var firstComplesso = data.Complessi.First();

                // Verifica se ha il campo "id" (usato dal mobile) invece che il campo "Id" (standard)
                // o se utilizza altri nomi di campo non standard
                var props = firstComplesso.GetType().GetProperties();
                bool hasNonStandardFields = props.Any(p => p.Name == "id" || p.Name == "strutture" || p.Name == "impiantiIdriciAdduzione");

                return hasNonStandardFields;
            }

            // Se non ci sono complessi, controlla gli edifici
            if (data.Edifici != null && data.Edifici.Any())
            {
                var firstEdificio = data.Edifici.First();
                var props = firstEdificio.GetType().GetProperties();
                return props.Any(p => p.Name == "id" || p.Name == "idComplesso" || p.Name == "parentId");
            }

            return false;
        }

        // Converte il formato dei dati dall'app al formato atteso dal sistema
        private SyncData ConvertDataFormat(SyncData originalData)
        {
            // Crea un nuovo SyncData con lo stesso formato dei DTO
            var newData = new SyncData();

            // Registra gli ID e prepara le mappature
            IdMappings.Clear(); // Inizializza il dizionario delle mappature

            // Elabora i complessi
            if (originalData.Complessi != null)
            {
                foreach (var complesso in originalData.Complessi)
                {
                    var complessoDto = new ComplessoDTO();
                    CopyPropertiesDynamically(complesso, complessoDto);

                    // Gestisci eventuali campi con nomi diversi
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "Id", TryGetProperty<string>(complesso, "id"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "Cod", TryGetProperty<string>(complesso, "cod"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "Tipologia", TryGetProperty<string>(complesso, "tipologia"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "Priorita", TryGetProperty<string>(complesso, "priorita"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "Lotto", TryGetProperty<string>(complesso, "lotto"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "Regione", TryGetProperty<string>(complesso, "regione"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "Provincia", TryGetProperty<string>(complesso, "provincia"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "StradaAnas", TryGetProperty<string>(complesso, "stradaAnas"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "ProgressivaChilometrica", TryGetProperty<string>(complesso, "progressivaChilometrica"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "ComuneAnas", TryGetProperty<string>(complesso, "comuneAnas"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "LocalitaAnas", TryGetProperty<string>(complesso, "localitaAnas"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "ViaAnas", TryGetProperty<string>(complesso, "viaAnas"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "NumeroCivicoAnas", TryGetProperty<string>(complesso, "numeroCivicoAnas"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "CapAnas", TryGetProperty<string>(complesso, "capAnas"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "AgibilitaRilieviArchitettoniciAnas", TryGetProperty<bool>(complesso, "agibilitaRilieviArchitettoniciAnas"));
                    TrySetProperty(typeof(ComplessoDTO), complessoDto, "CausaInagibilitaRilieviArchitettoniciAnas", TryGetProperty<string>(complesso, "causaInagibilitaRilieviArchitettoniciAnas"));

                    // Gestisci gli ID dell'entità e le relazioni
                    string id = TryGetProperty<string>(complesso, "id");
                    if (!string.IsNullOrEmpty(id))
                    {
                        complessoDto.Id = id;
                        IdMappings[id] = id; // Mantieni lo stesso ID
                    }

                    // Gestisci le relazioni
                    complessoDto.Strutture = ConvertRelationList(TryGetProperty<List<string>>(complesso, "strutture"));
                    complessoDto.Edifici = ConvertRelationList(TryGetProperty<List<string>>(complesso, "edifici"));
                    complessoDto.ImpiantiIdriciAdduzione = ConvertRelationList(TryGetProperty<List<string>>(complesso, "impiantiIdriciAdduzione"));
                    complessoDto.ImpiantiScarichi = ConvertRelationList(TryGetProperty<List<string>>(complesso, "impiantiScarichi"));
                    complessoDto.ImpiantiClimaAcs = ConvertRelationList(TryGetProperty<List<string>>(complesso, "impiantiClimaAcs"));
                    complessoDto.ImpiantiElettrici = ConvertRelationList(TryGetProperty<List<string>>(complesso, "impiantiElettrici"));
                    complessoDto.AltriImpianti = ConvertRelationList(TryGetProperty<List<string>>(complesso, "altriImpianti"));

                    newData.Complessi.Add(complessoDto);
                }
            }

            // Elabora gli edifici
            if (originalData.Edifici != null)
            {
                foreach (var edificio in originalData.Edifici)
                {
                    var edificioDto = new EdificioDTO();
                    CopyPropertiesDynamically(edificio, edificioDto);

                    // Gestisci eventuali campi con nomi diversi
                    TrySetProperty(typeof(EdificioDTO), edificioDto, "Id", TryGetProperty<string>(edificio, "id"));
                    TrySetProperty(typeof(EdificioDTO), edificioDto, "IdComplesso", TryGetProperty<string>(edificio, "idComplesso"));

                    // Gestisci gli ID dell'entità e le relazioni
                    string id = TryGetProperty<string>(edificio, "id");
                    if (!string.IsNullOrEmpty(id))
                    {
                        edificioDto.Id = id;
                        IdMappings[id] = id; // Mantieni lo stesso ID
                    }

                    // Gestisci le parent relazioni
                    edificioDto.ParentId = TryGetProperty<string>(edificio, "idComplesso");
                    edificioDto.ParentType = "complesso";
                    edificioDto.ComplessoId = TryGetProperty<string>(edificio, "idComplesso");

                    // Gestisci le relazioni con altre entità
                    edificioDto.UnitaImmobiliari = ConvertRelationList(TryGetProperty<List<string>>(edificio, "unitaImmobiliari"));
                    edificioDto.Strutture = ConvertRelationList(TryGetProperty<List<string>>(edificio, "strutture"));
                    edificioDto.ImpiantiIdriciAdduzione = ConvertRelationList(TryGetProperty<List<string>>(edificio, "impiantiIdriciAdduzione"));
                    edificioDto.ImpiantiScarichi = ConvertRelationList(TryGetProperty<List<string>>(edificio, "impiantiScarichi"));
                    edificioDto.ImpiantiClimaAcs = ConvertRelationList(TryGetProperty<List<string>>(edificio, "impiantiClimaAcs"));
                    edificioDto.ImpiantiElettrici = ConvertRelationList(TryGetProperty<List<string>>(edificio, "impiantiElettrici"));
                    edificioDto.AltriImpianti = ConvertRelationList(TryGetProperty<List<string>>(edificio, "altriImpianti"));
                    edificioDto.Infissi = ConvertRelationList(TryGetProperty<List<string>>(edificio, "infissi"));
                    edificioDto.DocumentiGenerali = ConvertRelationList(TryGetProperty<List<string>>(edificio, "documentiGenerali"));

                    newData.Edifici.Add(edificioDto);
                }
            }

            // Elabora le unità immobiliari
            if (originalData.UnitaImmobiliari != null)
            {
                foreach (var unita in originalData.UnitaImmobiliari)
                {
                    var unitaDto = new UnitaImmobiliareDTO();
                    CopyPropertiesDynamically(unita, unitaDto);

                    // Gestisci eventuali campi con nomi diversi
                    TrySetProperty(typeof(UnitaImmobiliareDTO), unitaDto, "Id", TryGetProperty<string>(unita, "id"));
                    TrySetProperty(typeof(UnitaImmobiliareDTO), unitaDto, "IdEdificio", TryGetProperty<string>(unita, "idEdificio"));

                    // Gestisci gli ID dell'entità e le relazioni
                    string id = TryGetProperty<string>(unita, "id");
                    if (!string.IsNullOrEmpty(id))
                    {
                        unitaDto.Id = id;
                        IdMappings[id] = id; // Mantieni lo stesso ID
                    }

                    // Gestisci le parent relazioni
                    unitaDto.ParentId = TryGetProperty<string>(unita, "idEdificio");
                    unitaDto.ParentType = "edificio";
                    unitaDto.EdificioId = TryGetProperty<string>(unita, "idEdificio");

                    // Gestisci le relazioni con altre entità
                    unitaDto.Strutture = ConvertRelationList(TryGetProperty<List<string>>(unita, "strutture"));
                    unitaDto.ImpiantiIdriciAdduzione = ConvertRelationList(TryGetProperty<List<string>>(unita, "impiantiIdriciAdduzione"));
                    unitaDto.ImpiantiScarichi = ConvertRelationList(TryGetProperty<List<string>>(unita, "impiantiScarichi"));
                    unitaDto.ImpiantiClimaAcs = ConvertRelationList(TryGetProperty<List<string>>(unita, "impiantiClimaAcs"));
                    unitaDto.ImpiantiElettrici = ConvertRelationList(TryGetProperty<List<string>>(unita, "impiantiElettrici"));
                    unitaDto.AltriImpianti = ConvertRelationList(TryGetProperty<List<string>>(unita, "altriImpianti"));
                    unitaDto.Infissi = ConvertRelationList(TryGetProperty<List<string>>(unita, "infissi"));

                    newData.UnitaImmobiliari.Add(unitaDto);
                }
            }

            // Elabora le strutture
            if (originalData.Strutture != null)
            {
                foreach (var struttura in originalData.Strutture)
                {
                    var strutturaDto = new StruttureDTO();
                    CopyPropertiesDynamically(struttura, strutturaDto);

                    // Gestisci eventuali campi con nomi diversi
                    TrySetProperty(typeof(StruttureDTO), strutturaDto, "Id", TryGetProperty<string>(struttura, "id"));

                    // Gestisci gli ID dell'entità e le relazioni
                    string id = TryGetProperty<string>(struttura, "id");
                    if (!string.IsNullOrEmpty(id))
                    {
                        strutturaDto.Id = id;
                        IdMappings[id] = id; // Mantieni lo stesso ID
                    }

                    // Gestisci le parent relazioni
                    strutturaDto.ParentId = TryGetProperty<string>(struttura, "parentId");
                    strutturaDto.ParentType = TryGetProperty<string>(struttura, "parentType");

                    // Imposta i riferimenti specifici in base al parentType
                    string parentType = TryGetProperty<string>(struttura, "parentType");
                    if (parentType == "complesso")
                    {
                        strutturaDto.ComplessoId = TryGetProperty<string>(struttura, "parentId");
                    }
                    else if (parentType == "edificio")
                    {
                        strutturaDto.EdificioId = TryGetProperty<string>(struttura, "parentId");
                    }
                    else if (parentType == "unita")
                    {
                        strutturaDto.UnitaId = TryGetProperty<string>(struttura, "parentId");
                    }

                    newData.Strutture.Add(strutturaDto);
                }
            }

            // Elabora gli impianti idrici
            ProcessImpianti<ImpiantoIdricoDTO>(originalData.ImpiantiIdrici, newData.ImpiantiIdrici);

            // Elabora gli impianti scarichi
            ProcessImpianti<ImpiantoScarichiDTO>(originalData.ImpiantiScarichi, newData.ImpiantiScarichi);

            // Elabora gli impianti clima/ACS
            ProcessImpianti<ImpiantoClimaAcsDTO>(originalData.ImpiantiClimaAcs, newData.ImpiantiClimaAcs);

            // Elabora gli impianti elettrici
            ProcessImpianti<ImpiantoElettricoDTO>(originalData.ImpiantiElettrici, newData.ImpiantiElettrici);

            // Elabora gli altri impianti
            ProcessImpianti<AltroImpiantoDTO>(originalData.AltriImpianti, newData.AltriImpianti);

            // Elabora gli infissi
            ProcessImpianti<InfissoDTO>(originalData.Infissi, newData.Infissi);

            // Elabora gli documenti
            ProcessImpianti<DocumentoDTO>(originalData.Documenti, newData.Documenti);

            // Aggiorna tutti i riferimenti negli elenchi di ID delle entità
            UpdateRelationships(newData);

            return newData;
        }

        private void ProcessImpianti<T>(IList<T> sourceList, IList<T> targetList) where T : new()
        {
            if (sourceList == null) return;

            foreach (var impianto in sourceList)
            {
                var impiantoDto = new T();
                CopyPropertiesDynamically(impianto, impiantoDto);

                // Gestisci eventuali campi con nomi diversi
                TrySetProperty(typeof(T), impiantoDto, "Id", TryGetProperty<string>(impianto, "id"));

                // Gestisci le parent relazioni
                TrySetProperty(typeof(T), impiantoDto, "ParentId", TryGetProperty<string>(impianto, "parentId"));
                TrySetProperty(typeof(T), impiantoDto, "ParentType", TryGetProperty<string>(impianto, "parentType"));

                // Gestisci gli ID dell'entità e le relazioni
                string id = TryGetProperty<string>(impianto, "id");
                if (!string.IsNullOrEmpty(id))
                {
                    TrySetProperty(typeof(T), impiantoDto, "Id", id);
                    IdMappings[id] = id; // Mantieni lo stesso ID
                }

                targetList.Add(impiantoDto);
            }
        }

        private List<string> ConvertRelationList(List<string> originalIds)
        {
            if (originalIds == null) return new List<string>();

            var convertedIds = new List<string>();
            foreach (var id in originalIds)
            {
                // Se l'ID originale è già mappato, usa l'ID mappato
                if (IdMappings.ContainsKey(id))
                {
                    convertedIds.Add(IdMappings[id]);
                }
                else
                {
                    // Altrimenti mantieni l'ID originale e aggiungilo alla mappatura
                    IdMappings[id] = id;
                    convertedIds.Add(id);
                }
            }

            return convertedIds;
        }

        // Metodo per recuperare in modo sicuro una proprietà che potrebbe non esistere
        private T TryGetProperty<T>(object obj, string propertyName)
        {
            try
            {
                var property = obj.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    return (T)property.GetValue(obj);
                }
            }
            catch
            {
                // Ignora errori e restituisci il valore predefinito per il tipo T
            }

            return default(T);
        }

        // Metodo per copiare dinamicamente le proprietà da un oggetto all'altro
        private void CopyPropertiesDynamically(object source, object destination)
        {
            if (source == null || destination == null)
                return;

            // Ottieni tutte le proprietà della destinazione
            var destProps = destination.GetType().GetProperties()
                .Where(p => p.CanWrite)
                .ToDictionary(p => p.Name, p => p);

            // Ottieni tutte le proprietà dell'origine
            var sourceProps = source.GetType().GetProperties();

            foreach (var sourceProp in sourceProps)
            {
                try
                {
                    // Verifica se la destinazione ha una proprietà con lo stesso nome
                    if (destProps.TryGetValue(sourceProp.Name, out var destProp))
                    {
                        // Ottieni il valore dalla proprietà di origine
                        var value = sourceProp.GetValue(source);

                        // Salta i null per evitare eccezioni
                        if (value == null)
                            continue;

                        // Se i tipi sono compatibili, copia direttamente
                        if (destProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                        {
                            destProp.SetValue(destination, value);
                        }
                        // Converti i tipi speciali come DateTime
                        else if (destProp.PropertyType == typeof(string) && value is DateTime)
                        {
                            destProp.SetValue(destination, ((DateTime)value).ToString("o"));
                        }
                        // Converti valori numerici in decimal se necessario
                        else if (destProp.PropertyType == typeof(decimal))
                        {
                            destProp.SetValue(destination, SafeToDecimal(value));
                        }
                        // Gestisci le liste di stringhe
                        else if (destProp.PropertyType == typeof(List<string>) && value is IEnumerable<string> sourceList)
                        {
                            destProp.SetValue(destination, sourceList.ToList());
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log dell'errore ma continua il processo
                    _logger.LogWarning(ex, $"Errore durante la copia della proprietà {sourceProp.Name}");
                }
            }
        }

        // Converte un valore in decimal in modo sicuro
        private decimal SafeToDecimal(object value)
        {
            if (value == null)
                return 0;

            try
            {
                if (value is decimal decimalValue)
                    return decimalValue;

                if (value is double doubleValue)
                    return Convert.ToDecimal(doubleValue);

                if (value is string stringValue && !string.IsNullOrEmpty(stringValue))
                {
                    if (decimal.TryParse(stringValue, out decimal result))
                        return result;
                }

                return Convert.ToDecimal(value);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Impossibile convertire il valore '{value}' in decimal. Usato valore di default 0.");
                return 0;
            }
        }

        // Mantiene una mappa degli ID originali agli ID convertiti
        private readonly Dictionary<string, string> IdMappings = new Dictionary<string, string>();

        // Aggiorna le relazioni tra entità utilizzando la mappa degli ID
        private void UpdateRelationships(SyncData data)
        {
            try
            {
                _logger.LogInformation("Aggiornamento relazioni tra entità...");

                // Aggiorna relazioni per i complessi
                if (data.Complessi != null)
                {
                    foreach (var complesso in data.Complessi)
                    {
                        UpdateIdList(complesso.Edifici);
                        UpdateIdList(complesso.Strutture);
                        UpdateIdList(complesso.ImpiantiIdriciAdduzione);
                        UpdateIdList(complesso.ImpiantiScarichi);
                        UpdateIdList(complesso.ImpiantiClimaAcs);
                        UpdateIdList(complesso.ImpiantiElettrici);
                        UpdateIdList(complesso.AltriImpianti);
                    }
                }

                // Aggiorna relazioni per gli edifici
                if (data.Edifici != null)
                {
                    foreach (var edificio in data.Edifici)
                    {
                        // Aggiorna il parentId se esiste nella mappa
                        if (!string.IsNullOrEmpty(edificio.ParentId) && IdMappings.ContainsKey(edificio.ParentId))
                        {
                            edificio.ParentId = IdMappings[edificio.ParentId];
                        }

                        // Nota: i nomi delle proprietà corretti possono variare in base all'implementazione effettiva del DTO
                        // Qui verifichiamo con reflection l'esistenza delle proprietà prima di tentare di aggiornarle
                        var properties = edificio.GetType().GetProperties();

                        foreach (var property in properties)
                        {
                            if (property.PropertyType == typeof(List<string>) && property.GetValue(edificio) is List<string> idList)
                            {
                                UpdateIdList(idList);
                            }
                        }
                    }
                }

                // Aggiorna relazioni per le unità immobiliari
                if (data.UnitaImmobiliari != null)
                {
                    foreach (var unitaImmobiliare in data.UnitaImmobiliari)
                    {
                        // Aggiorna il parentId se esiste nella mappa
                        if (!string.IsNullOrEmpty(unitaImmobiliare.ParentId) && IdMappings.ContainsKey(unitaImmobiliare.ParentId))
                        {
                            unitaImmobiliare.ParentId = IdMappings[unitaImmobiliare.ParentId];

                            // Verifica se EdificioId è una proprietà valida prima di assegnarla
                            var edificioIdProperty = unitaImmobiliare.GetType().GetProperty("EdificioId");
                            if (edificioIdProperty != null)
                            {
                                edificioIdProperty.SetValue(unitaImmobiliare, unitaImmobiliare.ParentId);
                            }
                        }

                        // Utilizziamo lo stesso approccio basato su reflection per le unità immobiliari
                        var properties = unitaImmobiliare.GetType().GetProperties();

                        foreach (var property in properties)
                        {
                            if (property.PropertyType == typeof(List<string>) && property.GetValue(unitaImmobiliare) is List<string> idList)
                            {
                                UpdateIdList(idList);
                            }
                        }
                    }
                }

                // Aggiorna relazioni per gli impianti
                foreach (var impianto in data.ImpiantiIdrici ?? Enumerable.Empty<ImpiantoIdricoDTO>())
                {
                    if (!string.IsNullOrEmpty(impianto.ParentId) && IdMappings.ContainsKey(impianto.ParentId))
                    {
                        impianto.ParentId = IdMappings[impianto.ParentId];
                    }
                }

                foreach (var impianto in data.ImpiantiScarichi ?? Enumerable.Empty<ImpiantoScarichiDTO>())
                {
                    if (!string.IsNullOrEmpty(impianto.ParentId) && IdMappings.ContainsKey(impianto.ParentId))
                    {
                        impianto.ParentId = IdMappings[impianto.ParentId];
                    }
                }

                foreach (var impianto in data.ImpiantiClimaAcs ?? Enumerable.Empty<ImpiantoClimaAcsDTO>())
                {
                    if (!string.IsNullOrEmpty(impianto.ParentId) && IdMappings.ContainsKey(impianto.ParentId))
                    {
                        impianto.ParentId = IdMappings[impianto.ParentId];
                    }
                }

                foreach (var impianto in data.ImpiantiElettrici ?? Enumerable.Empty<ImpiantoElettricoDTO>())
                {
                    if (!string.IsNullOrEmpty(impianto.ParentId) && IdMappings.ContainsKey(impianto.ParentId))
                    {
                        impianto.ParentId = IdMappings[impianto.ParentId];
                    }
                }

                foreach (var impianto in data.AltriImpianti ?? Enumerable.Empty<AltroImpiantoDTO>())
                {
                    if (!string.IsNullOrEmpty(impianto.ParentId) && IdMappings.ContainsKey(impianto.ParentId))
                    {
                        impianto.ParentId = IdMappings[impianto.ParentId];
                    }
                }

                // Aggiorna relazioni per strutture e infissi
                foreach (var struttura in data.Strutture ?? Enumerable.Empty<StruttureDTO>())
                {
                    if (!string.IsNullOrEmpty(struttura.ParentId) && IdMappings.ContainsKey(struttura.ParentId))
                    {
                        struttura.ParentId = IdMappings[struttura.ParentId];
                    }
                }

                foreach (var infisso in data.Infissi ?? Enumerable.Empty<InfissoDTO>())
                {
                    if (!string.IsNullOrEmpty(infisso.ParentId) && IdMappings.ContainsKey(infisso.ParentId))
                    {
                        infisso.ParentId = IdMappings[infisso.ParentId];
                    }
                }

                // Aggiorna relazioni per i documenti
                foreach (var documento in data.Documenti ?? Enumerable.Empty<DocumentoDTO>())
                {
                    if (!string.IsNullOrEmpty(documento.ParentId) && IdMappings.ContainsKey(documento.ParentId))
                    {
                        documento.ParentId = IdMappings[documento.ParentId];
                    }
                }

                _logger.LogInformation("Aggiornamento relazioni completato");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'aggiornamento delle relazioni tra entità");
                throw;
            }
        }

        // Metodo di utilità per aggiornare una lista di ID con i nuovi ID mappati
        private void UpdateIdList(List<string> idList)
        {
            if (idList == null || !idList.Any())
                return;

            for (int i = 0; i < idList.Count; i++)
            {
                string oldId = idList[i];
                if (!string.IsNullOrEmpty(oldId) && IdMappings.ContainsKey(oldId))
                {
                    idList[i] = IdMappings[oldId];
                    _logger.LogDebug($"Relazione aggiornata: {oldId} -> {idList[i]}");
                }
            }
        }

        // Registra un record di sincronizzazione nel database
        private async Task RegistraSincronizzazione(SyncRequest request)
        {
            try
            {
                // Verifica se esiste già un record per questo dispositivo
                var deviceId = request.DeviceInfo?.DeviceId;
                if (string.IsNullOrEmpty(deviceId))
                {
                    // Se non c'è un DeviceId, genera un ID univoco basato sulla piattaforma
                    deviceId = $"{request.DeviceInfo?.Platform ?? "unknown"}-{Guid.NewGuid().ToString()}";
                }

                var existingRecord = await _context.SyncRecords
                    .FirstOrDefaultAsync(s => s.DeviceId == deviceId);

                if (existingRecord != null)
                {
                    // Aggiorna il record esistente
                    existingRecord.SyncedAt = DateTime.UtcNow;
                    existingRecord.DeviceVersion = request.DeviceInfo?.Version;
                    _context.SyncRecords.Update(existingRecord);
                }
                else
                {
                    // Crea un nuovo record
                    var syncRecord = new SyncRecord
                    {
                        DeviceId = deviceId,
                        DevicePlatform = request.DeviceInfo?.Platform ?? "unknown",
                        DeviceVersion = request.DeviceInfo?.Version ?? "unknown",
                        SyncedAt = DateTime.UtcNow
                    };

                    _context.SyncRecords.Add(syncRecord);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Registrazione sincronizzazione completata per il dispositivo {deviceId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la registrazione della sincronizzazione");
                // Non lanciamo un'eccezione per non interrompere il flusso principale
            }
        }

        // Metodo per impostare una proprietà tramite reflection
        private void TrySetProperty(Type type, object obj, string propertyName, object value)
        {
            try
            {
                var property = type.GetProperty(propertyName);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(obj, value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Impossibile impostare la proprietà {propertyName}");
            }
        }

        // Helper method to try parsing a GUID with a prefix
        private bool TryParseGuidWithPrefix(string input, string prefix, out Guid result)
        {
            result = Guid.Empty;

            if (string.IsNullOrEmpty(input))
                return false;

            // Check if the input starts with the prefix
            if (input.StartsWith(prefix))
            {
                string guidPart = input.Substring(prefix.Length);
                return Guid.TryParse(guidPart, out result);
            }

            // If not prefixed, try direct parsing
            return Guid.TryParse(input, out result);
        }
    }
    #endregion
}
