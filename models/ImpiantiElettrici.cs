using System;

namespace Models
{
    public class ImpiantiElettrici
    {
        // Identificazione principale
        public Guid Id { get; set; }
        public string? Codice { get; set; }
        public string? DistribuzioneElettrica { get; set; }
        public string? LocaliServiti { get; set; }
        public string? StatoConservativo { get; set; }
        public string? Contatore { get; set; }
        public string? FotoContatore { get; set; }
        public string? FotoGenerale { get; set; }
        public string? ContrattoFornitura { get; set; }
        public string? DichiarazioneConformita { get; set; }
        public string? DichiarazioneRispondenza { get; set; }
    }
} 