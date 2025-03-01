using System;
using System.Collections.Generic;

namespace Models
{
    public class UnitaImmobiliare
    {
        // Identificazione principale
        public Guid Id { get; set; }

        #region Dati da rilevare durante il sopralluogo
        public bool? AgibilitaRilieviArchitettoniciRilevato { get; set; }
        public string? CausaInagibilitaRilieviArchitettoniciRilevato { get; set; }
        public string? Scala { get; set; }
        public string? Piano { get; set; }
        public string? Interno { get; set; }
        public string? StatoOccupazioneImmobile { get; set; }
        public string? StatoOccupazioneImmobileDescrizione { get; set; }
        public string? NomeConduttoreEffettivo { get; set; }
        public string? DestinazioneUsoRilevata { get; set; }
        public string? DisegnoSchemaInfissi { get; set; }
        public string? Note { get; set; }
        #endregion

        #region Dati da aggiungere in Back Office prima del sopralluogo
        public string? Subalterno { get; set; }
        public string? CategoriaCatastale { get; set; }
        public string? PlanimetriaCatastale { get; set; }
        public string? VisuraIpotecaria { get; set; }
        public string? VisuraCatastaleStorica { get; set; }
        #endregion

        #region Dati da aggiungere in Back Office dopo il sopralluogo
        public string? CertificatoDestinazioneUrbanistica { get; set; }
        public string? AttestazionePrestazioneEnergetica { get; set; }
        public string? TitoloEdilizio { get; set; }
        public string? ContrattoUso { get; set; }
        public string? TipoUso { get; set; }
        public string? CategoriaAttivita { get; set; }
        public string? SegnalazioneCertificataAgibilita { get; set; }
        public string? RicevutePagamentoTributi { get; set; }
        public string? RilievoArchitettonico { get; set; }
        #endregion

        #region Entità collegate
        public List<Guid>? InfissiIds { get; set; } = new List<Guid>();
        public List<Guid>? IdraulicoAdduzioneIds { get; set; } = new List<Guid>();
        public List<Guid>? ScarichiIdriciFognariIds { get; set; } = new List<Guid>();
        public List<Guid>? ImpiantoClimaAcsIds { get; set; } = new List<Guid>();
        public List<Guid>? ImpiantiElettriciIds { get; set; } = new List<Guid>();
        public List<Guid>? AltriImpiantiIds { get; set; } = new List<Guid>();
        public List<Guid>? DocumentiGeneraliIds { get; set; } = new List<Guid>();
        #endregion
    }
}