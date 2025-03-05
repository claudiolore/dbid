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
                response.Success = false;
                response.Message = "Formato dati non valido: manca il campo 'data'";
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
                            var stats = await SincronizzaComplessi(request.Data.Complessi);
                            response.EntitiesStats["complessi"] = stats;
                            AggiungiAStatisticheTotali(response.Stats, stats);
                        }

                        // Sincronizza gli edifici
                        if (request.Data.Edifici != null && request.Data.Edifici.Any())
                        {
                            var stats = await SincronizzaEdifici(request.Data.Edifici);
                            response.EntitiesStats["edifici"] = stats;
                            AggiungiAStatisticheTotali(response.Stats, stats);
                        }

                        // Sincronizza le unità immobiliari
                        if (request.Data.UnitaImmobiliari != null && request.Data.UnitaImmobiliari.Any())
                        {
                            var stats = await SincronizzaUnitaImmobiliari(request.Data.UnitaImmobiliari);
                            response.EntitiesStats["unita_immobiliari"] = stats;
                            AggiungiAStatisticheTotali(response.Stats, stats);
                        }

                        // Sincronizza le strutture
                        if (request.Data.Strutture != null && request.Data.Strutture.Any())
                        {
                            var stats = await SincronizzaStrutture(request.Data.Strutture);
                            response.EntitiesStats["strutture"] = stats;
                            AggiungiAStatisticheTotali(response.Stats, stats);
                        }

                        // Sincronizza gli impianti idrici
                        if (request.Data.ImpiantiIdrici != null && request.Data.ImpiantiIdrici.Any())
                        {
                            var stats = await SincronizzaImpiantiIdrici(request.Data.ImpiantiIdrici);
                            response.EntitiesStats["impianti_idrici"] = stats;
                            AggiungiAStatisticheTotali(response.Stats, stats);
                        }

                        // Sincronizza gli impianti scarichi
                        if (request.Data.ImpiantiScarichi != null && request.Data.ImpiantiScarichi.Any())
                        {
                            var stats = await SincronizzaImpiantiScarichi(request.Data.ImpiantiScarichi);
                            response.EntitiesStats["impianti_scarichi"] = stats;
                            AggiungiAStatisticheTotali(response.Stats, stats);
                        }

                        // Sincronizza gli impianti clima ACS
                        if (request.Data.ImpiantiClimaAcs != null && request.Data.ImpiantiClimaAcs.Any())
                        {
                            var stats = await SincronizzaImpiantiClimaAcs(request.Data.ImpiantiClimaAcs);
                            response.EntitiesStats["impianti_clima_acs"] = stats;
                            AggiungiAStatisticheTotali(response.Stats, stats);
                        }

                        // Sincronizza gli impianti elettrici
                        if (request.Data.ImpiantiElettrici != null && request.Data.ImpiantiElettrici.Any())
                        {
                            var stats = await SincronizzaImpiantiElettrici(request.Data.ImpiantiElettrici);
                            response.EntitiesStats["impianti_elettrici"] = stats;
                            AggiungiAStatisticheTotali(response.Stats, stats);
                        }

                        // Sincronizza gli altri impianti
                        if (request.Data.AltriImpianti != null && request.Data.AltriImpianti.Any())
                        {
                            var stats = await SincronizzaAltriImpianti(request.Data.AltriImpianti);
                            response.EntitiesStats["altri_impianti"] = stats;
                            AggiungiAStatisticheTotali(response.Stats, stats);
                        }

                        // Sincronizza gli infissi
                        if (request.Data.Infissi != null && request.Data.Infissi.Any())
                        {
                            var stats = await SincronizzaInfissi(request.Data.Infissi);
                            response.EntitiesStats["infissi"] = stats;
                            AggiungiAStatisticheTotali(response.Stats, stats);
                        }

                        // Sincronizza i documenti
                        if (request.Data.Documenti != null && request.Data.Documenti.Any())
                        {
                            var stats = await SincronizzaDocumenti(request.Data.Documenti);
                            response.EntitiesStats["documenti"] = stats;
                            AggiungiAStatisticheTotali(response.Stats, stats);
                        }

                        // Salva i dati di sincronizzazione
                        var syncRecord = new SyncRecord
                        {
                            DeviceId = request.DeviceInfo.Platform + "-" + Guid.NewGuid().ToString(),
                            DevicePlatform = request.DeviceInfo.Platform,
                            DeviceVersion = request.DeviceInfo.Version,
                            SyncedAt = DateTime.UtcNow,
                            TotalEntitiesProcessed = response.Stats.Total,
                            EntitiesCreated = response.Stats.Created,
                            EntitiesUpdated = response.Stats.Updated,
                            EntitiesDeleted = response.Stats.Deleted,
                            EntitiesUnchanged = response.Stats.Unchanged
                        };

                        _context.SyncRecords.Add(syncRecord);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        // Registra il record di sincronizzazione
                        await RegistraSincronizzazione(request);

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
            var complessiIds = complessi.Select(c => Guid.Parse(c.Id.Replace("C-", ""))).ToList();

            // Carica i complessi esistenti
            var existingComplessi = await _context.Complessi
                .Where(c => complessiIds.Contains(c.Id))
                .ToListAsync();

            foreach (var complessoDto in complessi)
            {
                var complessoId = Guid.Parse(complessoDto.Id.Replace("C-", ""));
                var complesso = existingComplessi.FirstOrDefault(c => c.Id == complessoId);

                if (complesso == null)
                {
                    // Crea nuovo complesso
                    complesso = new Complesso
                    {
                        Id = complessoId,
                        // Mappa le proprietà dal DTO all'entità
                        Codice = complessoDto.Cod,
                        Tipologia = complessoDto.Tipologia,
                        Priorita = complessoDto.Priorita,
                        Lotto = complessoDto.Lotto,
                        RegioneAnas = complessoDto.RegioneAnas,
                        ProvinciaAnas = complessoDto.ProvinciaAnas,
                        StradaAnas = complessoDto.StradaAnas,
                        ProgressivaChilometrica = complessoDto.ProgressivaChilometrica,
                        ComuneAnas = complessoDto.ComuneAnas,
                        LocalitaAnas = complessoDto.LocalitaAnas,
                        ViaAnas = complessoDto.ViaAnas,
                        NumeroCivicoAnas = complessoDto.NumeroCivicoAnas,
                        CapAnas = complessoDto.CapAnas,
                        AgibilitaRilieviArchitettoniciAnas = complessoDto.AgibilitaRilieviArchitettoniciAnas,
                        CausaInagibilitaRilieviArchitettoniciAnas = complessoDto.CausaInagibilitaRilieviArchitettoniciAnas,
                        Operatore = complessoDto.Operatore,
                        DataRilievo = string.IsNullOrEmpty(complessoDto.DataRilievo) ? null : DateTime.Parse(complessoDto.DataRilievo),
                        CoordinateGeografiche = complessoDto.CoordinateGeografiche.ToString(),
                        RegioneRilevato = complessoDto.RegioneRilevato,
                        ProvinciaRilevato = complessoDto.ProvinciaRilevato,
                        ComuneRilevato = complessoDto.ComuneRilevato,
                        CapRilevato = complessoDto.CapRilevato,
                        LocalitaRilevato = complessoDto.LocalitaRilevato,
                        IndirizziRilevati = complessoDto.IndirizziRilevato,
                        AgibilitaRilieviArchitettoniciRilevato = complessoDto.AgibilitaRilieviArchitettoniciRilevato,
                        CausaInagibilitaRilieviArchitettoniciRilevato = complessoDto.CausaInagibilitaRilieviArchitettoniciRilevato,
                        DestinazioneUso = complessoDto.DestinazioneUso,
                        FotoGenerali = complessoDto.FotoGenerali,
                        Note = complessoDto.Note,
                        ElaboratoPlanimetrico = complessoDto.ElaboratoPlanimetrico,
                        AerofotografiaCartografiaStorica = complessoDto.AerofotografiaCartografiaStorica,
                        EstrattoDiMappaCatastale = complessoDto.EstrattoMappaCatastale,
                        PianiUrbanistici = complessoDto.PianiUrbanistici,
                        RelazioneGeotecnicaDelSito = complessoDto.RelazioneGeotecnica,
                        CategorieUsoNTC2018 = complessoDto.CategorieUsoNTC2018,
                        DocumentoValutazioneRischi = complessoDto.DocumentoValutazioneRischi,
                        DichiarazioneInteresseCulturaleArtistico = complessoDto.DichiarazioneInteresseArtistico,
                        IstanzaAccertamentoSussistenza = complessoDto.IstanzaAccertamentoSussistenza,
                        SegnalazioneCertificataAgibilita = complessoDto.SegnalazioneCertificataAgibilita,
                        ProgettoUltimoInterventoEffettuato = complessoDto.ProgettoUltimoIntervento,
                        TitoloProprieta = complessoDto.TitoloProprieta,
                    };

                    // Gestisci le relazioni
                    if (complessoDto.Edifici != null && complessoDto.Edifici.Any())
                    {
                        complesso.EdificiIds = complessoDto.Edifici.Select(id => Guid.Parse(id.Replace("E-", ""))).ToList();
                    }

                    if (complessoDto.Strutture != null && complessoDto.Strutture.Any())
                    {
                        complesso.StruttureIds = complessoDto.Strutture.Select(id => Guid.Parse(id.Replace("S-", ""))).ToList();
                    }

                    if (complessoDto.ImpiantiIdriciAdduzione != null && complessoDto.ImpiantiIdriciAdduzione.Any())
                    {
                        complesso.IdraulicoAdduzioneIds = complessoDto.ImpiantiIdriciAdduzione.Select(id => Guid.Parse(id.Replace("II-", ""))).ToList();
                    }

                    if (complessoDto.ImpiantiScarichi != null && complessoDto.ImpiantiScarichi.Any())
                    {
                        complesso.ScarichiIdriciFognariIds = complessoDto.ImpiantiScarichi.Select(id => Guid.Parse(id.Replace("IS-", ""))).ToList();
                    }

                    if (complessoDto.ImpiantiClimaAcs != null && complessoDto.ImpiantiClimaAcs.Any())
                    {
                        complesso.ImpiantoClimaAcsIds = complessoDto.ImpiantiClimaAcs.Select(id => Guid.Parse(id.Replace("ICA-", ""))).ToList();
                    }

                    if (complessoDto.ImpiantiElettrici != null && complessoDto.ImpiantiElettrici.Any())
                    {
                        complesso.ImpiantiElettriciIds = complessoDto.ImpiantiElettrici.Select(id => Guid.Parse(id.Replace("IE-", ""))).ToList();
                    }

                    if (complessoDto.AltriImpianti != null && complessoDto.AltriImpianti.Any())
                    {
                        complesso.AltriImpiantiIds = complessoDto.AltriImpianti.Select(id => Guid.Parse(id.Replace("AI-", ""))).ToList();
                    }

                    _context.Complessi.Add(complesso);
                    stats.Created++;
                }
                else
                {
                    // Aggiorna complesso esistente
                    bool modificato = false;

                    // Verifica se ci sono cambiamenti e aggiorna i campi
                    if (complesso.Codice != complessoDto.Cod)
                    {
                        complesso.Codice = complessoDto.Cod;
                        modificato = true;
                    }

                    // ... continua con tutti gli altri campi

                    // Aggiorna relazioni
                    if (complessoDto.Edifici != null)
                    {
                        var nuoviEdificiIds = complessoDto.Edifici.Select(id => Guid.Parse(id.Replace("E-", ""))).ToList();
                        if (!nuoviEdificiIds.SequenceEqual(complesso.EdificiIds ?? new List<Guid>()))
                        {
                            complesso.EdificiIds = nuoviEdificiIds;
                            modificato = true;
                        }
                    }

                    // ... continua con tutte le altre relazioni

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

            // Verifica se ci sono complessi da eliminare
            var complessiDaEliminare = await _context.Complessi
                .Where(c => !complessiIds.Contains(c.Id))
                .ToListAsync();

            foreach (var complesso in complessiDaEliminare)
            {
                _context.Complessi.Remove(complesso);
                stats.Deleted++;
            }

            await _context.SaveChangesAsync();
            stats.Total = stats.Created + stats.Updated + stats.Deleted + stats.Unchanged;

            return stats;
        }

        private async Task<EntityStats> SincronizzaEdifici(List<EdificioDTO> edifici)
        {
            var stats = new EntityStats();
            var edificiIds = edifici.Select(e => Guid.Parse(e.Id.Replace("E-", ""))).ToList();

            // Carica gli edifici esistenti
            var existingEdifici = await _context.Edifici
                .Where(e => edificiIds.Contains(e.Id))
                .ToListAsync();

            foreach (var edificioDto in edifici)
            {
                var edificioId = Guid.Parse(edificioDto.Id.Replace("E-", ""));
                var edificio = existingEdifici.FirstOrDefault(e => e.Id == edificioId);

                if (edificio == null)
                {
                    // Crea nuovo edificio
                    edificio = new Edificio
                    {
                        Id = edificioId,
                        // Mappa proprietà dal DTO
                    };

                    // Gestisci le relazioni (unità immobiliari, strutture, impianti, ecc.)
                    if (edificioDto.ParentId != null)
                    {
                        var complessoId = Guid.Parse(edificioDto.ParentId.Replace("C-", ""));
                        var complesso = await _context.Complessi.FindAsync(complessoId);
                        if (complesso != null)
                        {
                            complesso.EdificiIds ??= new List<Guid>();
                            if (!complesso.EdificiIds.Contains(edificioId))
                            {
                                complesso.EdificiIds.Add(edificioId);
                            }
                        }
                    }

                    _context.Edifici.Add(edificio);
                    stats.Created++;
                }
                else
                {
                    // Aggiorna edificio esistente
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

            // Verifica se ci sono edifici da eliminare
            var edificiDaEliminare = await _context.Edifici
                .Where(e => !edificiIds.Contains(e.Id))
                .ToListAsync();

            foreach (var edificio in edificiDaEliminare)
            {
                _context.Edifici.Remove(edificio);
                stats.Deleted++;
            }

            await _context.SaveChangesAsync();
            stats.Total = stats.Created + stats.Updated + stats.Deleted + stats.Unchanged;

            return stats;
        }

        private async Task<EntityStats> SincronizzaUnitaImmobiliari(List<UnitaImmobiliareDTO> unitaImmobiliari)
        {
            var stats = new EntityStats();
            var unitaIds = unitaImmobiliari.Select(u => Guid.Parse(u.Id.Replace("UI-", ""))).ToList();

            // Carica le unità immobiliari esistenti
            var existingUnita = await _context.UnitaImmobiliari
                .Where(u => unitaIds.Contains(u.Id))
                .ToListAsync();

            foreach (var unitaDto in unitaImmobiliari)
            {
                var unitaId = Guid.Parse(unitaDto.Id.Replace("UI-", ""));
                var unita = existingUnita.FirstOrDefault(u => u.Id == unitaId);

                if (unita == null)
                {
                    // Crea nuova unità immobiliare
                    unita = new UnitaImmobiliare
                    {
                        Id = unitaId,
                        // Mappa proprietà dal DTO
                    };

                    // Gestisci le relazioni
                    if (unitaDto.ParentId != null)
                    {
                        var edificioId = Guid.Parse(unitaDto.ParentId.Replace("E-", ""));
                        var edificio = await _context.Edifici.FindAsync(edificioId);
                        if (edificio != null)
                        {
                            // Aggiorna le relazioni nell'edificio
                        }
                    }

                    _context.UnitaImmobiliari.Add(unita);
                    stats.Created++;
                }
                else
                {
                    // Aggiorna unità immobiliare esistente
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

            // Verifica se ci sono unità immobiliari da eliminare
            var unitaDaEliminare = await _context.UnitaImmobiliari
                .Where(u => !unitaIds.Contains(u.Id))
                .ToListAsync();

            foreach (var unita in unitaDaEliminare)
            {
                _context.UnitaImmobiliari.Remove(unita);
                stats.Deleted++;
            }

            await _context.SaveChangesAsync();
            stats.Total = stats.Created + stats.Updated + stats.Deleted + stats.Unchanged;

            return stats;
        }

        private async Task<EntityStats> SincronizzaStrutture(List<StruttureDTO> strutture)
        {
            var stats = new EntityStats();
            var struttureIds = strutture.Select(s => Guid.Parse(s.Id.Replace("S-", ""))).ToList();

            // Carica le strutture esistenti
            var existingStrutture = await _context.Strutture
                .Where(s => struttureIds.Contains(s.Id))
                .ToListAsync();

            foreach (var strutturaDto in strutture)
            {
                var strutturaId = Guid.Parse(strutturaDto.Id.Replace("S-", ""));
                var struttura = existingStrutture.FirstOrDefault(s => s.Id == strutturaId);

                if (struttura == null)
                {
                    // Crea nuova struttura
                    struttura = new Strutture
                    {
                        Id = strutturaId,
                        // Mappa proprietà dal DTO
                    };

                    // Gestisci le relazioni in base al tipo di parent
                    if (strutturaDto.ParentId != null)
                    {
                        switch (strutturaDto.ParentType?.ToLower())
                        {
                            case "complesso":
                                var complessoId = Guid.Parse(strutturaDto.ParentId.Replace("C-", ""));
                                var complesso = await _context.Complessi.FindAsync(complessoId);
                                if (complesso != null)
                                {
                                    complesso.StruttureIds ??= new List<Guid>();
                                    if (!complesso.StruttureIds.Contains(strutturaId))
                                    {
                                        complesso.StruttureIds.Add(strutturaId);
                                    }
                                }
                                break;
                            case "edificio":
                                var edificioId = Guid.Parse(strutturaDto.ParentId.Replace("E-", ""));
                                // Gestisci la relazione con l'edificio
                                break;
                            case "unita":
                                var unitaId = Guid.Parse(strutturaDto.ParentId.Replace("UI-", ""));
                                // Gestisci la relazione con l'unità immobiliare
                                break;
                        }
                    }

                    _context.Strutture.Add(struttura);
                    stats.Created++;
                }
                else
                {
                    // Aggiorna struttura esistente
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

            // Verifica se ci sono strutture da eliminare
            var struttureDaEliminare = await _context.Strutture
                .Where(s => !struttureIds.Contains(s.Id))
                .ToListAsync();

            foreach (var struttura in struttureDaEliminare)
            {
                _context.Strutture.Remove(struttura);
                stats.Deleted++;
            }

            await _context.SaveChangesAsync();
            stats.Total = stats.Created + stats.Updated + stats.Deleted + stats.Unchanged;

            return stats;
        }

        private async Task<EntityStats> SincronizzaImpiantiIdrici(List<ImpiantoIdricoDTO> impiantiIdrici)
        {
            var stats = new EntityStats();
            var impiantiIds = impiantiIdrici.Select(i => Guid.Parse(i.Id.Replace("II-", ""))).ToList();

            // Carica gli impianti idrici esistenti
            var existingImpianti = await _context.IdraulicoAdduzione
                .Where(i => impiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impiantoDto in impiantiIdrici)
            {
                var impiantoId = Guid.Parse(impiantoDto.Id.Replace("II-", ""));
                var impianto = existingImpianti.FirstOrDefault(i => i.Id == impiantoId);

                if (impianto == null)
                {
                    // Crea nuovo impianto idrico
                    impianto = new IdraulicoAdduzione
                    {
                        Id = impiantoId,
                        // Mappa proprietà dal DTO
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
                .Where(i => !impiantiIds.Contains(i.Id))
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
            var impiantiIds = impiantiScarichi.Select(i => Guid.Parse(i.Id.Replace("IS-", ""))).ToList();

            // Carica gli impianti scarichi esistenti
            var existingImpianti = await _context.ScarichiIdriciFognari
                .Where(i => impiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impiantoDto in impiantiScarichi)
            {
                var impiantoId = Guid.Parse(impiantoDto.Id.Replace("IS-", ""));
                var impianto = existingImpianti.FirstOrDefault(i => i.Id == impiantoId);

                if (impianto == null)
                {
                    // Crea nuovo impianto scarichi
                    impianto = new ScarichiIdriciFognari
                    {
                        Id = impiantoId,
                        // Mappa proprietà dal DTO
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
                .Where(i => !impiantiIds.Contains(i.Id))
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
            var impiantiIds = impiantiClimaAcs.Select(i => Guid.Parse(i.Id.Replace("ICA-", ""))).ToList();

            // Carica gli impianti clima ACS esistenti
            var existingImpianti = await _context.ImpiantoClimaAcs
                .Where(i => impiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impiantoDto in impiantiClimaAcs)
            {
                var impiantoId = Guid.Parse(impiantoDto.Id.Replace("ICA-", ""));
                var impianto = existingImpianti.FirstOrDefault(i => i.Id == impiantoId);

                if (impianto == null)
                {
                    // Crea nuovo impianto clima ACS
                    impianto = new ImpiantoClimaAcs
                    {
                        Id = impiantoId,
                        // Mappa proprietà dal DTO
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
                .Where(i => !impiantiIds.Contains(i.Id))
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
            var impiantiIds = impiantiElettrici.Select(i => Guid.Parse(i.Id.Replace("IE-", ""))).ToList();

            // Carica gli impianti elettrici esistenti
            var existingImpianti = await _context.ImpiantiElettrici
                .Where(i => impiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impiantoDto in impiantiElettrici)
            {
                var impiantoId = Guid.Parse(impiantoDto.Id.Replace("IE-", ""));
                var impianto = existingImpianti.FirstOrDefault(i => i.Id == impiantoId);

                if (impianto == null)
                {
                    // Crea nuovo impianto elettrico
                    impianto = new ImpiantiElettrici
                    {
                        Id = impiantoId,
                        // Mappa proprietà dal DTO
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
                .Where(i => !impiantiIds.Contains(i.Id))
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
            var impiantiIds = altriImpianti.Select(i => Guid.Parse(i.Id.Replace("AI-", ""))).ToList();

            // Carica gli altri impianti esistenti
            var existingImpianti = await _context.AltriImpianti
                .Where(i => impiantiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var impiantoDto in altriImpianti)
            {
                var impiantoId = Guid.Parse(impiantoDto.Id.Replace("AI-", ""));
                var impianto = existingImpianti.FirstOrDefault(i => i.Id == impiantoId);

                if (impianto == null)
                {
                    // Crea nuovo altro impianto
                    impianto = new AltriImpianti
                    {
                        Id = impiantoId,
                        // Mappa proprietà dal DTO
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
                .Where(i => !impiantiIds.Contains(i.Id))
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
            var infissiIds = infissi.Select(i => Guid.Parse(i.Id.Replace("IN-", ""))).ToList();

            // Carica gli infissi esistenti
            var existingInfissi = await _context.Infissi
                .Where(i => infissiIds.Contains(i.Id))
                .ToListAsync();

            foreach (var infissoDto in infissi)
            {
                var infissoId = Guid.Parse(infissoDto.Id.Replace("IN-", ""));
                var infisso = existingInfissi.FirstOrDefault(i => i.Id == infissoId);

                if (infisso == null)
                {
                    // Crea nuovo infisso
                    infisso = new Infissi
                    {
                        Id = infissoId,
                        // Mappa proprietà dal DTO
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
                .Where(i => !infissiIds.Contains(i.Id))
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
            var documentiIds = documenti.Select(d => Guid.Parse(d.Id.Replace("D-", ""))).ToList();

            // Carica i documenti esistenti
            var existingDocumenti = await _context.DocumentiGenerali
                .Where(d => documentiIds.Contains(d.Id))
                .ToListAsync();

            foreach (var documentoDto in documenti)
            {
                var documentoId = Guid.Parse(documentoDto.Id.Replace("D-", ""));
                var documento = existingDocumenti.FirstOrDefault(d => d.Id == documentoId);

                if (documento == null)
                {
                    // Crea nuovo documento
                    documento = new DocumentiGenerali
                    {
                        Id = documentoId,
                        // Mappa proprietà dal DTO
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
                .Where(d => !documentiIds.Contains(d.Id))
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
            try
            {
                // Controlla se almeno un complesso è presente e se il formato degli ID è come quello dell'app
                // (es. "C1741128947250-wtdpuoxt3" invece di "C-guid")
                if (data.Complessi != null && data.Complessi.Any())
                {
                    var firstId = data.Complessi.First().Id;
                    // L'app usa ID come "C1741128947250-wtdpuoxt3" invece di "C-guid"
                    return !firstId.StartsWith("C-") &&
                           (firstId.StartsWith("C") || firstId.Contains("-"));
                }

                // Se non ci sono complessi, verifica gli edifici
                if (data.Edifici != null && data.Edifici.Any())
                {
                    var firstId = data.Edifici.First().Id;
                    return !firstId.StartsWith("E-") &&
                           (firstId.StartsWith("E") || firstId.Contains("-"));
                }

                // Se non ci sono edifici, verifica le unità immobiliari
                if (data.UnitaImmobiliari != null && data.UnitaImmobiliari.Any())
                {
                    var firstId = data.UnitaImmobiliari.First().Id;
                    return !firstId.StartsWith("UI-") &&
                           (firstId.StartsWith("U") || firstId.Contains("-"));
                }

                // Se non ci sono neanche unità immobiliari, verifica gli impianti
                if (data.ImpiantiElettrici != null && data.ImpiantiElettrici.Any())
                {
                    var firstId = data.ImpiantiElettrici.First().Id;
                    return !firstId.StartsWith("IE-") &&
                           (firstId.StartsWith("IE") || firstId.Contains("-"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Errore durante la verifica del formato dati. Assumo formato standard.");
                return false;
            }

            return false;
        }

        // Converte il formato dei dati dall'app al formato atteso dal sistema
        private SyncData ConvertDataFormat(SyncData originalData)
        {
            // Pulisci la mappa di conversione prima di iniziare
            IdMappings.Clear();

            _logger.LogInformation("Inizio conversione formato dati da app mobile");

            // Verifiche di base sulla struttura dei dati
            if (originalData == null)
            {
                _logger.LogError("Dati originali nulli, impossibile procedere con la conversione");
                throw new ArgumentNullException(nameof(originalData), "I dati originali non possono essere nulli");
            }

            var convertedData = new SyncData
            {
                Complessi = new List<ComplessoDTO>(),
                Edifici = new List<EdificioDTO>(),
                UnitaImmobiliari = new List<UnitaImmobiliareDTO>(),
                ImpiantiIdrici = new List<ImpiantoIdricoDTO>(),
                ImpiantiScarichi = new List<ImpiantoScarichiDTO>(),
                ImpiantiClimaAcs = new List<ImpiantoClimaAcsDTO>(),
                ImpiantiElettrici = new List<ImpiantoElettricoDTO>(),
                AltriImpianti = new List<AltroImpiantoDTO>(),
                Strutture = new List<StruttureDTO>(),
                Infissi = new List<InfissoDTO>(),
                Documenti = new List<DocumentoDTO>(),
                Enums = originalData.Enums
            };

            try
            {
                // Converti i complessi
                if (originalData.Complessi != null)
                {
                    _logger.LogInformation($"Conversione di {originalData.Complessi.Count} complessi");

                    foreach (var original in originalData.Complessi)
                    {
                        if (original == null)
                        {
                            _logger.LogWarning("Trovato complesso nullo in JSON, ignorato.");
                            continue;
                        }

                        if (string.IsNullOrEmpty(original.Id))
                        {
                            _logger.LogWarning("Trovato complesso senza ID in JSON, ignorato.");
                            continue;
                        }

                        // Genera un ID nel formato corretto
                        string convertedId = "C-" + Guid.NewGuid().ToString();

                        try
                        {
                            // Crea un nuovo DTO con solo i campi di base garantiti
                            var converted = new ComplessoDTO
                            {
                                Id = convertedId
                                // Nota: ComplessoDTO non ha ParentId e ParentType perché è l'entità di livello più alto
                            };

                            // Copia dinamicamente le proprietà che potrebbero esistere
                            CopyPropertiesDynamically(original, converted);

                            // Associa l'ID originale al nuovo ID per ricostruire le relazioni
                            IdMappings[original.Id] = converted.Id;
                            _logger.LogDebug($"Mapped Complex ID: {original.Id} -> {converted.Id}");

                            convertedData.Complessi.Add(converted);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Errore durante la conversione del complesso con ID {original.Id}");
                        }
                    }
                }

                // Converti gli edifici
                if (originalData.Edifici != null)
                {
                    _logger.LogInformation($"Conversione di {originalData.Edifici.Count} edifici");

                    foreach (var original in originalData.Edifici)
                    {
                        if (original == null)
                        {
                            _logger.LogWarning("Trovato edificio nullo in JSON, ignorato.");
                            continue;
                        }

                        if (string.IsNullOrEmpty(original.Id))
                        {
                            _logger.LogWarning("Trovato edificio senza ID in JSON, ignorato.");
                            continue;
                        }

                        // Genera un ID nel formato corretto
                        string convertedId = "E-" + Guid.NewGuid().ToString();

                        try
                        {
                            // Trova il parent ID
                            string parentId = TryGetProperty<string>(original, "IdComplesso") ??
                                            TryGetProperty<string>(original, "ComplessoId") ??
                                            TryGetProperty<string>(original, "ParentId");

                            var converted = new EdificioDTO
                            {
                                Id = convertedId,
                                ParentId = parentId,
                                ParentType = "complesso"
                            };

                            // Copia dinamicamente le proprietà che potrebbero esistere
                            CopyPropertiesDynamically(original, converted);

                            // Associa l'ID originale al nuovo ID per ricostruire le relazioni
                            IdMappings[original.Id] = converted.Id;
                            _logger.LogDebug($"Mapped Building ID: {original.Id} -> {converted.Id}");

                            convertedData.Edifici.Add(converted);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Errore durante la conversione dell'edificio con ID {original.Id}");
                        }
                    }
                }

                // Converti le unità immobiliari
                if (originalData.UnitaImmobiliari != null)
                {
                    _logger.LogInformation($"Conversione di {originalData.UnitaImmobiliari.Count} unità immobiliari");

                    foreach (var original in originalData.UnitaImmobiliari)
                    {
                        if (original == null)
                        {
                            _logger.LogWarning("Trovata unità immobiliare nulla in JSON, ignorata.");
                            continue;
                        }

                        if (string.IsNullOrEmpty(original.Id))
                        {
                            _logger.LogWarning("Trovata unità immobiliare senza ID in JSON, ignorata.");
                            continue;
                        }

                        // Genera un ID nel formato corretto
                        string convertedId = "UI-" + Guid.NewGuid().ToString();

                        try
                        {
                            // Trova il parent ID
                            string parentId = TryGetProperty<string>(original, "IdEdificio") ??
                                            TryGetProperty<string>(original, "EdificioId") ??
                                            TryGetProperty<string>(original, "ParentId");

                            var converted = new UnitaImmobiliareDTO
                            {
                                Id = convertedId,
                                ParentId = parentId,
                                ParentType = "edificio"
                            };

                            // Copia dinamicamente le proprietà che potrebbero esistere
                            CopyPropertiesDynamically(original, converted);

                            // Associa l'ID originale al nuovo ID per ricostruire le relazioni
                            IdMappings[original.Id] = converted.Id;
                            _logger.LogDebug($"Mapped Real Estate Unit ID: {original.Id} -> {converted.Id}");

                            convertedData.UnitaImmobiliari.Add(converted);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Errore durante la conversione dell'unità immobiliare con ID {original.Id}");
                        }
                    }
                }

                // Impianti idrici
                if (originalData.ImpiantiIdrici != null)
                {
                    _logger.LogInformation($"Conversione di {originalData.ImpiantiIdrici.Count} impianti idrici");

                    foreach (var original in originalData.ImpiantiIdrici)
                    {
                        if (original == null || string.IsNullOrEmpty(original.Id))
                            continue;

                        string convertedId = "II-" + Guid.NewGuid().ToString();

                        try
                        {
                            var converted = new ImpiantoIdricoDTO
                            {
                                Id = convertedId,
                                ParentId = TryGetProperty<string>(original, "ParentId"),
                                ParentType = TryGetProperty<string>(original, "ParentType")
                            };

                            // Copia dinamicamente le proprietà che potrebbero esistere
                            CopyPropertiesDynamically(original, converted);

                            IdMappings[original.Id] = converted.Id;
                            _logger.LogDebug($"Mapped Water System ID: {original.Id} -> {converted.Id}");
                            convertedData.ImpiantiIdrici.Add(converted);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Errore durante la conversione dell'impianto idrico con ID {original.Id}");
                        }
                    }
                }

                // Impianti clima ACS
                if (originalData.ImpiantiClimaAcs != null)
                {
                    _logger.LogInformation($"Conversione di {originalData.ImpiantiClimaAcs.Count} impianti clima/ACS");

                    foreach (var original in originalData.ImpiantiClimaAcs)
                    {
                        if (original == null || string.IsNullOrEmpty(original.Id))
                            continue;

                        string convertedId = "ICA-" + Guid.NewGuid().ToString();

                        try
                        {
                            var converted = new ImpiantoClimaAcsDTO
                            {
                                Id = convertedId,
                                ParentId = TryGetProperty<string>(original, "ParentId"),
                                ParentType = TryGetProperty<string>(original, "ParentType")
                            };

                            // Copia dinamicamente le proprietà che potrebbero esistere
                            CopyPropertiesDynamically(original, converted);

                            IdMappings[original.Id] = converted.Id;
                            _logger.LogDebug($"Mapped Climate System ID: {original.Id} -> {converted.Id}");
                            convertedData.ImpiantiClimaAcs.Add(converted);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Errore durante la conversione dell'impianto clima/ACS con ID {original.Id}");
                        }
                    }
                }

                // Ricostruisci le relazioni utilizzando la mappa di conversione degli ID
                UpdateRelationships(convertedData);

                _logger.LogInformation("Conversione formato dati completata con successo");
                return convertedData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la conversione del formato dati");
                throw; // Rilancia l'eccezione per gestirla nel metodo chiamante
            }
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
                _logger.LogInformation("Registrazione record di sincronizzazione...");

                // Calcola le statistiche delle entità
                int totalComplessi = request.Data.Complessi?.Count ?? 0;
                int totalEdifici = request.Data.Edifici?.Count ?? 0;
                int totalUnitaImmobiliari = request.Data.UnitaImmobiliari?.Count ?? 0;
                int totalImpiantiIdrici = request.Data.ImpiantiIdrici?.Count ?? 0;
                int totalImpiantiScarichi = request.Data.ImpiantiScarichi?.Count ?? 0;
                int totalImpiantiClimaAcs = request.Data.ImpiantiClimaAcs?.Count ?? 0;
                int totalImpiantiElettrici = request.Data.ImpiantiElettrici?.Count ?? 0;
                int totalAltriImpianti = request.Data.AltriImpianti?.Count ?? 0;
                int totalStrutture = request.Data.Strutture?.Count ?? 0;
                int totalInfissi = request.Data.Infissi?.Count ?? 0;
                int totalDocumenti = request.Data.Documenti?.Count ?? 0;

                int totalEntities = totalComplessi + totalEdifici + totalUnitaImmobiliari +
                                   totalImpiantiIdrici + totalImpiantiScarichi + totalImpiantiClimaAcs +
                                   totalImpiantiElettrici + totalAltriImpianti + totalStrutture +
                                   totalInfissi + totalDocumenti;

                var detailedStats = $"Complessi: {totalComplessi}, Edifici: {totalEdifici}, Unità Immobiliari: {totalUnitaImmobiliari}, " +
                                   $"Impianti Idrici: {totalImpiantiIdrici}, Impianti Scarichi: {totalImpiantiScarichi}, " +
                                   $"Impianti Clima/ACS: {totalImpiantiClimaAcs}, Impianti Elettrici: {totalImpiantiElettrici}, " +
                                   $"Altri Impianti: {totalAltriImpianti}, Strutture: {totalStrutture}, " +
                                   $"Infissi: {totalInfissi}, Documenti: {totalDocumenti}";

                // Recupera i dati dell'utente dalla richiesta (se disponibili)
                string userId = TryGetProperty<string>(request, "UserId") ?? "Sistema";

                // Crea un nuovo record di sincronizzazione con i campi appropriati per il tuo modello di dati
                // Adatta questa parte in base al tuo modello effettivo
                var syncRecord = new SyncRecord
                {
                    Id = Guid.NewGuid(), // Converte string in Guid
                    SyncedAt = DateTime.UtcNow, // Usa SyncedAt invece di Timestamp
                    DeviceId = $"{request.DeviceInfo?.Platform} v{request.DeviceInfo?.Version}",
                    TotalEntitiesProcessed = totalEntities
                };

                // Imposta altre proprietà tramite reflection se esistono
                var syncRecordType = syncRecord.GetType();

                TrySetProperty(syncRecordType, syncRecord, "Status", "Completed");
                TrySetProperty(syncRecordType, syncRecord, "Details", detailedStats);
                TrySetProperty(syncRecordType, syncRecord, "UserId", userId);

                // Aggiungi il record al database
                _context.SyncRecords.Add(syncRecord);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Record di sincronizzazione registrato con ID: {syncRecord.Id}");
                _logger.LogInformation($"Statistiche sincronizzazione: {detailedStats}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la registrazione del record di sincronizzazione");
                throw;
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
    }
    #endregion
}
