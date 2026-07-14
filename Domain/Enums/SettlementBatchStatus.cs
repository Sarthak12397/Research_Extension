public enum SettlementBatchStatus
{
    Pending,      // Created, awaiting reconciliation and approval
    UnderReview,  // Finance team reviewing
    Approved,     // Approved for bank settlement
    Settled,      // Bank transfer confirmed
    Mismatched    // Partner report does not match internal records
}