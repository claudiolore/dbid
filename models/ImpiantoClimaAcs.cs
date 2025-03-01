using System;

namespace Models
{
    public class ImpiantoClimaAcs
    {
        // Identificazione principale
        public Guid Id { get; set; }
        public string? Codice { get; set; }
        public string? TipoImpianto { get; set; }
        public string? Generatore { get; set; }
        public string? Emissione { get; set; }
        public string? LocaliServiti { get; set; }
        public string? StatoConservativo { get; set; }
        public string? Foto { get; set; }
        public string? DichiarazioneConformita { get; set; }
        public string? DichiarazioneRispondenza { get; set; }
    }
} 