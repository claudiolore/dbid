using System;
using System.Collections.Generic;

namespace Models
{
    public class Strutture
    {
        // Identificazione principale
        public Guid Id { get; set; }
        public string? Codice { get; set; }
        public string? Posizione { get; set; }
        public string? TipologiaStruttura { get; set; }
        public string? SottotipologiaStruttura { get; set; }
        public decimal? SpessoreLordo { get; set; } // in cm
        public string? FinituraInterna { get; set; }
        public string? FinituraEsterna { get; set; }
        public string? StatoConservativo { get; set; }

        // Entità collegate
        public List<Guid>? SegnalazioneProblemaIds { get; set; } = new List<Guid>();
    }
} 