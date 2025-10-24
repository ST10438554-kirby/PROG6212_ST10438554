namespace ClaimSystem.Models
{
    public class FileUploadOptions
    {
        public string[] AllowedExtensions { get; set; }
        public long MaxFileSizeBytes { get; set; }
        public int MaxFilesPerClaim { get; set; }
        public string UploadFolder { get; set; }
    }
}
