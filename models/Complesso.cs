using System;
using System.Collections.Generic;

namespace Models
{
    public class Complesso
    {
        // Identificazione principale
        public Guid Id { get; set; }
        public List<Guid>? EdificiIds { get; set; } = new List<Guid>();

        #region Dati ANAS importati da file Excel (sola visualizzazione)
        public string? Codice { get; set; }
        public string? Tipologia { get; set; }
        public int? Priorita { get; set; }
        public string? Lotto { get; set; }
        public string? RegioneAnas { get; set; }
        public string? ProvinciaAnas { get; set; }
        public string? StradaAnas { get; set; }
        public string? ProgressivaChilometrica { get; set; }
        public string? ComuneAnas { get; set; }
        public string? LocalitaAnas { get; set; }
        public string? ViaAnas { get; set; }
        public int? NumeroCivicoAnas { get; set; }
        public int? CapAnas { get; set; }
        public bool? AgibilitaRilieviArchitettoniciAnas { get; set; }
        public string? CausaInagibilitaRilieviArchitettoniciAnas { get; set; }
        #endregion

        #region Dati da rilevare durante il sopralluogo
        public string? Operatore { get; set; }
        public DateTime? DataRilievo { get; set; }
        public string? CoordinateGeografiche { get; set; }
        public string? RegioneRilevato { get; set; }
        public string? ProvinciaRilevato { get; set; }
        public string? ComuneRilevato { get; set; }
        public int? CapRilevato { get; set; }
        public string? LocalitaRilevato { get; set; }
        public List<string>? IndirizziRilevati { get; set; } = new List<string>();
        public bool? AgibilitaRilieviArchitettoniciRilevato { get; set; }
        public string? CausaInagibilitaRilieviArchitettoniciRilevato { get; set; }
        public string? DestinazioneUso { get; set; }
        public List<string>? FotoGenerali { get; set; } = new List<string>();
        public string? StruttureComplesso { get; set; }
        public string? Note { get; set; }
        #endregion

        #region Dati da aggiungere in Back Office prima del sopralluogo
        public string? ElaboratoPlanimetrico { get; set; }
        public string? AerofotografiaCartografiaStorica { get; set; }
        public string? EstrattoDiMappaCatastale { get; set; }
        #endregion

        #region Dati da aggiungere in Back Office dopo il sopralluogo
        public string? PianiUrbanistici { get; set; }
        public string? RelazioneGeotecnicaDelSito { get; set; }
        public string? CategorieUsoNTC2018 { get; set; }
        public string? DocumentoValutazioneRischi { get; set; }
        public string? DichiarazioneInteresseCulturaleArtistico { get; set; }
        public string? IstanzaAccertamentoSussistenza { get; set; }
        public string? SegnalazioneCertificataAgibilita { get; set; }
        public string? ProgettoUltimoInterventoEffettuato { get; set; }
        public string? TitoloProprieta { get; set; }
        #endregion

        #region Entità collegate
        public List<Guid>? StruttureIds { get; set; } = new List<Guid>();
        public List<Guid>? IdraulicoAdduzioneIds { get; set; } = new List<Guid>();
        public List<Guid>? ScarichiIdriciFognariIds { get; set; } = new List<Guid>();
        public List<Guid>? ImpiantoClimaAcsIds { get; set; } = new List<Guid>();
        public List<Guid>? ImpiantiElettriciIds { get; set; } = new List<Guid>();
        public List<Guid>? AltriImpiantiIds { get; set; } = new List<Guid>();
        public List<Guid>? DocumentiGeneraliIds { get; set; } = new List<Guid>();
        #endregion

    }
}
