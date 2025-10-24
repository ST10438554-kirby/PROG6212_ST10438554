using System;

namespace ClaimSystem.Models
{
    public class SupportingDocument
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public Claim Claim { get; set; }

        public string FileName { get; set; }        // original name
        public string StoredFileName { get; set; }  // unique stored filename on disk
        public long Size { get; set; }              // in bytes
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
