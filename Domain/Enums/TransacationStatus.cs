public enum TransactionStatus
{
    Draft,            // Quote preparation, not yet confirmed
    Quoted,           // Rate and fee calculated, not confirmed by sender
    Submitted,        // Sender confirmed, payment collection in progress
    ComplianceHold,   // Manual compliance review required before payout
    Approved,         // Compliance and business rules passed
    SentToPartner,    // Payout instruction dispatched to partner
    ReadyForPayout,   // Cash pickup or branch payout is available
    Paid,             // Beneficiary received funds
    Failed,           // Partner rejected or technical failure
    Cancelled,        // Cancelled before payout sent
    Refunded,         // Sender refund completed after cancellation/failure
    Reversed          // Reversed after partner/accounting adjustment
}