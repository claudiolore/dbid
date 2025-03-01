using System;

namespace Models
{
    public class IdraulicoAdduzione
    {
        // Identificazione principale
        public Guid Id { get; set; }
        public string? Codice { get; set; }
        public string? TipoImpianto { get; set; }
        public string? Contatore { get; set; }
        public string? StatoConservativo { get; set; }
        public string? Foto { get; set; }
        public string? ContrattoFornitura { get; set; }
        public string? DichiarazioneConformita { get; set; }
        public string? DichiarazioneRispondenza { get; set; }
    }
} 