public enum FeeType
{
    Fixed,               // Flat per-transaction amount
    Percentage,          // Percentage of send amount
    TierBased,           // Different fee by amount slabs
    PartnerCommission,   // Amount or percentage payable to agent or payout partner
    TaxVat,              // Tax if applicable by source country
    PromotionalDiscount  // Temporary discount (negative fee)
}