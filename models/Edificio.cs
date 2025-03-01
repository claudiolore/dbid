using System;
using System.Collections.Generic;

namespace Models
{
    public class Edificio
    {
        // Identificazione principale
        public Guid Id { get; set; }
        public List<Guid>? UnitaImmobiliariIds { get; set; } = new List<Guid>();

        #region Dati da rilevare durante il sopralluogo
        public bool? AgibilitaRilieviArchitettoniciRilevato { get; set; }
        public string? CausaInagibilitaRilieviArchitettoniciRilevato { get; set; }
        public string? DestinazioneUsoPrevalenteRilevata { get; set; }
        public int? LivelliFuoriTerra { get; set; }
        public int? LivelliSeminterrati { get; set; }
        public int? LivelliInterrati { get; set; }
        public int? CorpiScala { get; set; }
        public string? TipologiaEdilizia { get; set; }
        public string? PosizioneRispettoAiFabbricatiCircostanti { get; set; }
        public string? StruttureImmobile { get; set; }
        public string? Note { get; set; }
        #endregion

        #region Dati da aggiungere in Back Office prima del sopralluogo
        public string? Sezione { get; set; }
        public string? Foglio { get; set; }
        public string? Particella { get; set; }
        public string? TipoCatasto { get; set; } // Scelta tra N.C.T e N.C.E.U
        public string? RicevutePagamentoTributi { get; set; }
        public string? PianoManutenzioneOpera { get; set; }
        #endregion

        #region Dati da aggiungere in Back Office dopo il sopralluogo
        public string? SegnalazioneCertificataAgibilita { get; set; }
        public string? ProgettoStrutturale { get; set; }
        public string? CollaudoStatico { get; set; }
        public string? UltimoInterventoFabbricato { get; set; }
        public string? TipologiaStrutturale { get; set; }
        public string? RapportoVerificaFunzionamentoAscensori { get; set; }
        public string? TitoloEdilizio { get; set; }
        public string? Prospetto { get; set; }
        public string? Sezione_Doc { get; set; } // Renamed to avoid conflict with Sezione above
        public string? Planimetria { get; set; }
        #endregion

        #region Entità collegate
        public List<Guid>? StruttureIds { get; set; } = new List<Guid>();
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
