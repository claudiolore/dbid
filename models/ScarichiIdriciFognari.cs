using System;

namespace Models
{
    public class ScarichiIdriciFognari
    {
        // Identificazione principale
        public Guid Id { get; set; }
        public string? Codice { get; set; }
        public string? TipoImpianto { get; set; }
        public string? StatoConservativo { get; set; }
        public string? AllaccioInFogna { get; set; }
        public string? Foto { get; set; }
    }
} 