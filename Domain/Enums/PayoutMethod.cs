public enum PayoutMethod
{
      BankDeposit,   // Requires bank code, account number, account holder name
    WalletTransfer, // Requires wallet provider and mobile number
    CashPickup,    // Requires pickup location and beneficiary ID
    BranchPayout,  // Internal branch pays beneficiary
    HomeDelivery 
}