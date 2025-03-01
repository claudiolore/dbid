using System;

namespace Models
{
    public class SegnalazioneProblema
    {
        // Identificazione principale
        public Guid Id { get; set; }
        public string? Descrizione { get; set; }
        public string? Foto { get; set; }
    }
} 