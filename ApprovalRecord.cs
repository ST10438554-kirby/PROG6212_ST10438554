using System;

namespace ClaimSystem.Models
{
    public class ApprovalRecord
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public Claim Claim { get; set; }

        public string ApproverId { get; set; }
        public string ApproverName { get; set; }
        public DateTime ActionAt { get; set; } = DateTime.UtcNow;
        public bool Approved { get; set; }
        public string Comment { get; set; }
        public string Role { get; set; }
    }
}
