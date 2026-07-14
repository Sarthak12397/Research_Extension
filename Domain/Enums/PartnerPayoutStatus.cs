public enum PartnerPayoutStatus
{
    Pending,   // Instruction sent, awaiting partner acknowledgement
    Accepted,  // Partner accepted, payout not yet completed
    Ready,     // Cash pickup ready for beneficiary collection
    Paid,      // Beneficiary paid
    Rejected,  // Partner rejected instruction
    Reversed   // Reversed after prior acceptance or payment
}