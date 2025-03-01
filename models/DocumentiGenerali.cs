using System;
using System.Collections.Generic;

namespace Models
{
    public class DocumentiGenerali
    {
        // Identificazione principale
        public Guid Id { get; set; }
        public List<string>? Documenti { get; set; } = new List<string>();
        public string? Descrizione { get; set; }
    }
} 