using System;
using System.Collections.Generic;

namespace Models
{
    public class Infissi
    {
        // Identificazione principale
        public Guid Id { get; set; }
        public string? Codice { get; set; }
        public string? MaterialeTelaio { get; set; }
        public decimal? SpessoreTelaio { get; set; } // in cm
        public string? TaglioTermico { get; set; }
        public string? NumeroAnte { get; set; }
        public string? Vetro { get; set; }
        public decimal? Larghezza { get; set; } // in cm
        public decimal? Altezza { get; set; } // in cm
        public string? SistemaOscuramento { get; set; }
        public bool? Inferriate { get; set; }
        public string? StatoConservativo { get; set; }
        public List<string>? Foto { get; set; } = new List<string>();
    }
} 