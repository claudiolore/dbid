using System;
using System.Collections.Generic;

namespace Models.DTO
{
    // Modelli per le richieste di sincronizzazione
    public class SyncRequest
    {
        public SyncData Data { get; set; }
        public DateTime Timestamp { get; set; }
        public DeviceInfo DeviceInfo { get; set; }
    }

    public class SyncData
    {
        public List<ComplessoDTO> Complessi { get; set; } = new List<ComplessoDTO>();
        public List<EdificioDTO> Edifici { get; set; } = new List<EdificioDTO>();
        public List<UnitaImmobiliareDTO> UnitaImmobiliari { get; set; } = new List<UnitaImmobiliareDTO>();
        public List<StruttureDTO> Strutture { get; set; } = new List<StruttureDTO>();
        public List<ImpiantoIdricoDTO> ImpiantiIdrici { get; set; } = new List<ImpiantoIdricoDTO>();
        public List<ImpiantoScarichiDTO> ImpiantiScarichi { get; set; } = new List<ImpiantoScarichiDTO>();
        public List<ImpiantoClimaAcsDTO> ImpiantiClimaAcs { get; set; } = new List<ImpiantoClimaAcsDTO>();
        public List<ImpiantoElettricoDTO> ImpiantiElettrici { get; set; } = new List<ImpiantoElettricoDTO>();
        public List<AltroImpiantoDTO> AltriImpianti { get; set; } = new List<AltroImpiantoDTO>();
        public List<InfissoDTO> Infissi { get; set; } = new List<InfissoDTO>();
        public List<DocumentoDTO> Documenti { get; set; } = new List<DocumentoDTO>();
        public Dictionary<string, object> Enums { get; set; } = new Dictionary<string, object>();
    }

    public class DeviceInfo
    {
        public string Platform { get; set; }
        public string Version { get; set; }
    }

    // Modelli per le risposte di sincronizzazione
    public class SyncResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public SyncStats Stats { get; set; }
        public Dictionary<string, EntityStats> EntitiesStats { get; set; } = new Dictionary<string, EntityStats>();
        public DateTime Timestamp { get; set; }
    }

    public class SyncStats
    {
        public int Total { get; set; }
        public int Created { get; set; }
        public int Updated { get; set; }
        public int Deleted { get; set; }
        public int Unchanged { get; set; }
    }

    public class EntityStats
    {
        public int Total { get; set; }
        public int Created { get; set; }
        public int Updated { get; set; }
        public int Deleted { get; set; }
        public int Unchanged { get; set; }
    }

    public class SyncStatusResponse
    {
        public DateTime? LastSyncTimestamp { get; set; }
        public string Status { get; set; }
        public string DeviceId { get; set; }
    }

    // Modelli per il caricamento dei file
    public class FileUploadRequest
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public string EntityId { get; set; }
        public string EntityType { get; set; }
        // In un'implementazione reale, qui ci sarebbe il contenuto del file
        // public byte[] FileContent { get; set; }
    }

    public class FileUploadResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid FileId { get; set; }
        public string FilePath { get; set; }
    }

    // Modelli per la notifica di completamento sincronizzazione file
    public class SyncFilesCompleteRequest
    {
        public string DeviceId { get; set; }
        public int FilesProcessed { get; set; }
        public int FilesSuccessful { get; set; }
        public int FilesFailed { get; set; }
    }

    public class SyncCompleteResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }

    // Modelli DTO per le entità
    public class ComplessoDTO
    {
        public string Id { get; set; } // Formato: C-xxxxx (GUID)
        public string Cod { get; set; }
        public string Gruppo { get; set; }
        public string Tipologia { get; set; }
        public int? Priorita { get; set; }
        public string Lotto { get; set; }
        public string RegioneAnas { get; set; }
        public string ProvinciaAnas { get; set; }
        public string StradaAnas { get; set; }
        public string ProgressivaChilometrica { get; set; }
        public string ComuneAnas { get; set; }
        public string LocalitaAnas { get; set; }
        public string ViaAnas { get; set; }
        public int? NumeroCivicoAnas { get; set; }
        public int? CapAnas { get; set; }
        public bool? AgibilitaRilieviArchitettoniciAnas { get; set; }
        public string CausaInagibilitaRilieviArchitettoniciAnas { get; set; }
        public string Operatore { get; set; }
        public string DataRilievo { get; set; }
        public decimal? CoordinateGeografiche { get; set; }
        public string RegioneRilevato { get; set; }
        public string ProvinciaRilevato { get; set; }
        public string ComuneRilevato { get; set; }
        public int? CapRilevato { get; set; }
        public string LocalitaRilevato { get; set; }
        public List<string> IndirizziRilevato { get; set; } = new List<string>();
        public bool? AgibilitaRilieviArchitettoniciRilevato { get; set; }
        public string CausaInagibilitaRilieviArchitettoniciRilevato { get; set; }
        public string DestinazioneUso { get; set; }
        public List<string> FotoGenerali { get; set; } = new List<string>();
        public string Note { get; set; }
        public string? ElaboratoPlanimetrico { get; set; }
        public string? AerofotografiaCartografiaStorica { get; set; }
        public string? EstrattoMappaCatastale { get; set; }
        public string? PianiUrbanistici { get; set; }
        public string? RelazioneGeotecnica { get; set; }
        public string CategorieUsoNTC2018 { get; set; }
        public string? DocumentoValutazioneRischi { get; set; }
        public string? DichiarazioneInteresseArtistico { get; set; }
        public string? IstanzaAccertamentoSussistenza { get; set; }
        public string? SegnalazioneCertificataAgibilita { get; set; }
        public string? ProgettoUltimoIntervento { get; set; }
        public string? TitoloProprieta { get; set; }
        public List<string> Edifici { get; set; } = new List<string>();
        public List<string> Strutture { get; set; } = new List<string>();
        public List<string> ImpiantiIdriciAdduzione { get; set; } = new List<string>();
        public List<string> ImpiantiScarichi { get; set; } = new List<string>();
        public List<string> ImpiantiClimaAcs { get; set; } = new List<string>();
        public List<string> ImpiantiElettrici { get; set; } = new List<string>();
        public List<string> AltriImpianti { get; set; } = new List<string>();
    }

    public class EdificioDTO
    {
        public string Id { get; set; } // Formato: E-xxxxx (GUID)
        public string? ParentId { get; set; }
        public string? ParentType { get; set; } // "complesso"
        // Altri campi specifici dell'edificio
    }

    public class UnitaImmobiliareDTO
    {
        public string Id { get; set; } // Formato: UI-xxxxx (GUID)
        public string? ParentId { get; set; }
        public string? ParentType { get; set; } // "edificio"
        // Altri campi specifici dell'unità immobiliare
    }

    public class StruttureDTO
    {
        public string Id { get; set; } // Formato: S-xxxxx (GUID)
        public string ParentId { get; set; }
        public string ParentType { get; set; } // 'complesso', 'edificio', 'unita'
        public string ComplessoId { get; set; } // Opzionale
        public string EdificioId { get; set; } // Opzionale
        public string UnitaId { get; set; } // Opzionale
        // Altri campi specifici della struttura
    }

    public class ImpiantoIdricoDTO
    {
        public string Id { get; set; } // Formato: II-xxxxx (GUID)
        public string ParentId { get; set; }
        public string ParentType { get; set; } // 'complesso', 'edificio', 'unita'
        // Altri campi specifici dell'impianto idrico
    }

    public class ImpiantoScarichiDTO
    {
        public string Id { get; set; } // Formato: IS-xxxxx (GUID)
        public string ParentId { get; set; }
        public string ParentType { get; set; } // 'complesso', 'edificio', 'unita'
        // Altri campi specifici dell'impianto scarichi
    }

    public class ImpiantoClimaAcsDTO
    {
        public string Id { get; set; } // Formato: ICA-xxxxx (GUID)
        public string ParentId { get; set; }
        public string ParentType { get; set; } // 'complesso', 'edificio', 'unita'
        // Altri campi specifici dell'impianto clima/ACS
    }

    public class ImpiantoElettricoDTO
    {
        public string Id { get; set; } // Formato: IE-xxxxx (GUID)
        public string ParentId { get; set; }
        public string ParentType { get; set; } // 'complesso', 'edificio', 'unita'
        // Altri campi specifici dell'impianto elettrico
    }

    public class AltroImpiantoDTO
    {
        public string Id { get; set; } // Formato: AI-xxxxx (GUID)
        public string ParentId { get; set; }
        public string ParentType { get; set; } // 'complesso', 'edificio', 'unita'
        // Altri campi specifici dell'altro impianto
    }

    public class InfissoDTO
    {
        public string Id { get; set; } // Formato: IN-xxxxx (GUID)
        public string ParentId { get; set; }
        public string ParentType { get; set; } // 'edificio', 'unita'
        // Altri campi specifici dell'infisso
    }

    public class DocumentoDTO
    {
        public string Id { get; set; } // Formato: D-xxxxx (GUID)
        public string ParentId { get; set; }
        public string ParentType { get; set; } // 'complesso', 'edificio', 'unita'
        // Altri campi specifici dei documenti
    }
}