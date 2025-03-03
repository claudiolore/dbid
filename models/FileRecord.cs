using System;

namespace Models
{
    public class FileRecord
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public DateTime UploadedAt { get; set; }
        public string EntityId { get; set; }
        public string EntityType { get; set; }
    }
}