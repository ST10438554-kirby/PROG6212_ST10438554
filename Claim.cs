using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaimSystem.Models
{
    public enum ClaimStatus { Pending, Approved, Rejected }

    public class Claim
    {
        public int Id { get; set; }

        [Required]
        public string LecturerId { get; set; }   // Identity user id

        [Required]
        public DateTime ClaimMonth { get; set; }

        [Required]
        [Range(0.0, 10000)]
        public decimal HoursWorked { get; set; }

        [Required]
        [Range(0.0, 100000)]
        public decimal HourlyRate { get; set; }

        public decimal ClaimAmount { get; set; }

        public string Notes { get; set; }

        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SupportingDocument> SupportingDocuments { get; set; } = new List<SupportingDocument>();
        public ICollection<ApprovalRecord> ApprovalRecords { get; set; } = new List<ApprovalRecord>();
    }
}
