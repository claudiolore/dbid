using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    // Modelli per le richieste di sincronizzazione
    public class SyncRequest
    {
        public SyncData? Data { get; set; }
        public DateTime Timestamp { get; set; }
        public DeviceInfo? DeviceInfo { get; set; }
    }

    public class SyncData
    {
        public List<ComplessoDTO>? Complessi { get; set; } = new List<ComplessoDTO>();
        public List<EdificioDTO>? Edifici { get; set; } = new List<EdificioDTO>();
        public List<UnitaImmobiliareDTO>? UnitaImmobiliari { get; set; } = new List<UnitaImmobiliareDTO>();
        public List<StruttureDTO>? Strutture { get; set; } = new List<StruttureDTO>();
        public List<ImpiantoIdricoDTO>? ImpiantiIdrici { get; set; } = new List<ImpiantoIdricoDTO>();
        public List<ImpiantoScarichiDTO>? ImpiantiScarichi { get; set; } = new List<ImpiantoScarichiDTO>();
        public List<ImpiantoClimaAcsDTO>? ImpiantiClimaAcs { get; set; } = new List<ImpiantoClimaAcsDTO>();
        public List<ImpiantoElettricoDTO>? ImpiantiElettrici { get; set; } = new List<ImpiantoElettricoDTO>();
        public List<AltroImpiantoDTO>? AltriImpianti { get; set; } = new List<AltroImpiantoDTO>();
        public List<InfissoDTO>? Infissi { get; set; } = new List<InfissoDTO>();
        public List<DocumentoDTO>? Documenti { get; set; } = new List<DocumentoDTO>();
        public Dictionary<string, object>? Enums { get; set; } = new Dictionary<string, object>();
    }

    public class DeviceInfo
    {
        public string? Platform { get; set; }
        public string? Version { get; set; }
        public string? DeviceId { get; set; }
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
        public string? Id { get; set; }
        public string? Cod { get; set; }
        public string? Tipologia { get; set; }
        public string? Priorita { get; set; }
        public string? Lotto { get; set; }
        public string? Regione { get; set; }
        public string? Provincia { get; set; }
        public string? StradaAnas { get; set; }
        public string? ProgressivaChilometrica { get; set; }
        public string? ComuneAnas { get; set; }
        public string? LocalitaAnas { get; set; }
        public string? ViaAnas { get; set; }
        public string? NumeroCivicoAnas { get; set; }
        public string? CapAnas { get; set; }
        public bool AgibilitaRilieviArchitettoniciAnas { get; set; }
        public string? CausaInagibilitaRilieviArchitettoniciAnas { get; set; }

        // Relazioni
        public List<string>? Strutture { get; set; } = new List<string>();
        public List<string>? Edifici { get; set; } = new List<string>();
        public List<string>? ImpiantiIdriciAdduzione { get; set; } = new List<string>();
        public List<string>? ImpiantiScarichi { get; set; } = new List<string>();
        public List<string>? ImpiantiClimaAcs { get; set; } = new List<string>();
        public List<string>? ImpiantiElettrici { get; set; } = new List<string>();
        public List<string>? AltriImpianti { get; set; } = new List<string>();
    }

    public class EdificioDTO
    {
        public string? Id { get; set; }
        public string? IdComplesso { get; set; }
        public bool AgibilitaRilieviArchitettoniciRilevato { get; set; }
        public string? CausaInagibilitaRilieviArchitettoniciRilevato { get; set; }
        public string? DestinazioneUsoPrevalenteRilevata { get; set; }
        public int LivelliFuoriTerra { get; set; }
        public int LivelliSeminterrati { get; set; }
        public int LivelliInterrati { get; set; }
        public int CorpiScala { get; set; }
        public string? TipologiaEdilizia { get; set; }
        public string? PosizioneRispettoAiFabbricati { get; set; }
        public object? StruttureImmobile { get; set; }
        public string? Note { get; set; }
        public string? Sezione { get; set; }
        public string? Foglio { get; set; }
        public string? Particella { get; set; }
        public string? TipoCatasto { get; set; }
        public object? RicevutePagamentoTributi { get; set; }
        public object? PianoManutenzioneOpera { get; set; }
        public object? SegnalazioneCertificataAgibilita { get; set; }
        public object? ProgettoStrutturale { get; set; }
        public object? CollaudoStatico { get; set; }
        public string? UltimoInterventoFabbricato { get; set; }
        public string? TipologiaStrutturale { get; set; }
        public object? RapportoVerificaAscensori { get; set; }
        public object? TitoloEdilizio { get; set; }
        public object? Prospetto { get; set; }
        public object? SezioneDoc { get; set; }
        public object? Planimetria { get; set; }

        // Relazioni
        public string? ParentId { get; set; }
        public string? ParentType { get; set; }
        public string? ComplessoId { get; set; }
        public List<string>? UnitaImmobiliari { get; set; } = new List<string>();
        public List<string>? Strutture { get; set; } = new List<string>();
        public List<string>? Infissi { get; set; } = new List<string>();
        public List<string>? ImpiantiIdriciAdduzione { get; set; } = new List<string>();
        public List<string>? ImpiantiScarichi { get; set; } = new List<string>();
        public List<string>? ImpiantiClimaAcs { get; set; } = new List<string>();
        public List<string>? ImpiantiElettrici { get; set; } = new List<string>();
        public List<string>? AltriImpianti { get; set; } = new List<string>();
        public List<string>? DocumentiGenerali { get; set; } = new List<string>();
    }

    public class UnitaImmobiliareDTO
    {
        public string? Id { get; set; }
        public string? IdEdificio { get; set; }
        public bool AgibilitaRilieviArchitettoniciRilevato { get; set; }
        public string? CausaInagibilitaRilieviArchitettoniciRilevato { get; set; }
        public string? Scala { get; set; }
        public string? Piano { get; set; }
        public string? Interno { get; set; }
        public string? StatoOccupazioneImmobile { get; set; }
        public string? StatoOccupazioneImmobileDescrizione { get; set; }
        public string? NomeConduttoreEffettivo { get; set; }
        public string? DestinazioneUsoRilevata { get; set; }
        public object? DisegnoSchemaInfissi { get; set; }
        public string? Note { get; set; }
        public string? Subalterno { get; set; }
        public string? CategoriaCatastale { get; set; }
        public object? PlanimetriaCatastale { get; set; }
        public object? VisuraIpotecaria { get; set; }
        public object? VisuraCatastaleStorica { get; set; }
        public object? CertificatoDestinazioneUrbanistica { get; set; }
        public object? AttestazionePrestazioneEnergetica { get; set; }
        public object? TitoloEdilizio { get; set; }
        public object? ContrattoUso { get; set; }
        public string? TipoUso { get; set; }
        public string? CategoriaAttivita { get; set; }
        public object? SegnalazioneCertificataAgibilita { get; set; }
        public object? RicevutePagamentoTributi { get; set; }
        public object? RilievoArchitettonico { get; set; }

        // Relazioni
        public string? ParentId { get; set; }
        public string? ParentType { get; set; }
        public string? EdificioId { get; set; }
        public List<string>? Strutture { get; set; } = new List<string>();
        public List<string>? Infissi { get; set; } = new List<string>();
        public List<string>? ImpiantiIdriciAdduzione { get; set; } = new List<string>();
        public List<string>? ImpiantiScarichi { get; set; } = new List<string>();
        public List<string>? ImpiantiClimaAcs { get; set; } = new List<string>();
        public List<string>? ImpiantiElettrici { get; set; } = new List<string>();
        public List<string>? AltriImpianti { get; set; } = new List<string>();
    }

    public class StruttureDTO
    {
        public string? Id { get; set; }
        public string? Codice { get; set; }
        public string? Posizione { get; set; }
        public string? SpessoreLordo { get; set; }
        public string? TipologiaStruttura { get; set; }
        public string? FinituraInterna { get; set; }
        public string? FinituraEsterna { get; set; }
        public string? StatoConservativo { get; set; }
        public object? Foto { get; set; }
        public string? SottotipologiaStruttura { get; set; }
        public List<object>? SegnalazioniProblema { get; set; } = new List<object>();

        // Relazioni
        public string? ParentId { get; set; }
        public string? ParentType { get; set; }
        public string? ComplessoId { get; set; }
        public string? EdificioId { get; set; }
        public string? UnitaId { get; set; }
    }

    public class ImpiantoIdricoDTO
    {
        public string? Id { get; set; }
        public string? Codice { get; set; }
        public string? TipoImpianto { get; set; }
        public string? Contatore { get; set; }
        public string? StatoConservativo { get; set; }
        public object? Foto { get; set; }

        // Relazioni
        public string? ParentId { get; set; }
        public string? ParentType { get; set; }
    }

    public class ImpiantoScarichiDTO
    {
        public string? Id { get; set; }
        public string? Codice { get; set; }
        public string? TipoImpianto { get; set; }
        public string? StatoConservativo { get; set; }
        public List<object>? Foto { get; set; } = new List<object>();
        public List<object>? AllaccioFogna { get; set; } = new List<object>();

        // Relazioni
        public string? ParentId { get; set; }
        public string? ParentType { get; set; }
    }

    public class ImpiantoClimaAcsDTO
    {
        public string? Id { get; set; }
        public string? Codice { get; set; }
        public string? TipoImpianto { get; set; }
        public string? Generatore { get; set; }
        public string? Emissione { get; set; }
        public string? LocaliServiti { get; set; }
        public string? StatoConservativo { get; set; }
        public object? Foto { get; set; }
        public object? CertificazioneImpianto { get; set; }
        public object? DichiarazioneConformita { get; set; }
        public object? DichiarazioneRispondenza { get; set; }

        // Relazioni
        public string? ParentId { get; set; }
        public string? ParentType { get; set; }
    }

    public class ImpiantoElettricoDTO
    {
        public string? Id { get; set; }
        public string? Codice { get; set; }
        public string? DistribuzioneElettrica { get; set; }
        public string? LocaliServiti { get; set; }
        public string? StatoConservativo { get; set; }
        public string? Contatore { get; set; }
        public object? FotoContatore { get; set; }
        public object? FotoGenerale { get; set; }
        public object? ContrattoFornitura { get; set; }
        public object? DichiarazioneConformita { get; set; }
        public object? DichiarazioneRispondenza { get; set; }

        // Relazioni
        public string? ParentId { get; set; }
        public string? ParentType { get; set; }
    }

    public class AltroImpiantoDTO
    {
        public string? Id { get; set; }
        public string? Codice { get; set; }
        public string? TipoImpianto { get; set; }
        public string? StatoConservativo { get; set; }
        public object? Foto { get; set; }
        public object? DichiarazioneConformita { get; set; }
        public object? DichiarazioneRispondenza { get; set; }

        // Relazioni
        public string? ParentId { get; set; }
        public string? ParentType { get; set; }
    }

    public class InfissoDTO
    {
        public string? Id { get; set; }

        // Relazioni
        public string? ParentId { get; set; }
        public string? ParentType { get; set; }
    }

    public class DocumentoDTO
    {
        public string? Id { get; set; }

        // Relazioni
        public string? ParentId { get; set; }
        public string? ParentType { get; set; }
    }
}