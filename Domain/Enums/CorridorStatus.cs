public enum CorridorStatus
{
    Inactive,               // Default on creation, not open for business
    Active,                 // Accepting quotes and transactions
    Suspended,              // Temporarily halted, requires review to reactivate
    Maintenance,            // Planned downtime
    ComplianceRestricted    // Regulatorily restricted, compliance team controls re-activation
}