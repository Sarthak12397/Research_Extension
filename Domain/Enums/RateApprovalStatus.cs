public enum RateApprovalStatus
{
    PendingApproval,  // Submitted by maker, awaiting checker
    Approved,         // Checker approved, active from EffectiveFrom
    Rejected,         // Checker rejected
    Expired           // Superseded by a newer approved rate
}