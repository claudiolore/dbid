using System;

namespace Models
{
    public class SyncRecord
    {
        public SyncRecord()
        {
            DeviceId = string.Empty;
            DevicePlatform = string.Empty;
            DeviceVersion = string.Empty;
        }

        public Guid Id { get; set; }
        public string DeviceId { get; set; }
        public string DevicePlatform { get; set; }
        public string DeviceVersion { get; set; }
        public DateTime SyncedAt { get; set; }
        public int TotalEntitiesProcessed { get; set; }
        public int EntitiesCreated { get; set; }
        public int EntitiesUpdated { get; set; }
        public int EntitiesDeleted { get; set; }
        public int EntitiesUnchanged { get; set; }
        public bool FilesSyncCompleted { get; set; }
        public DateTime? FilesSyncCompletedAt { get; set; }
        public int FilesProcessed { get; set; }
        public int FilesSuccessful { get; set; }
        public int FilesFailed { get; set; }
    }
}