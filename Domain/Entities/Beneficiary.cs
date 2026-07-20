public class Beneficiary
{
    public Guid Id { get; private set; }
    public string BeneficiaryCode { get; private set; }
    public Guid SenderId { get; private set; }
    public string FullName { get; private set; }
    public string DestinationCountry { get; private set; }
    public PayoutMethod PreferredPayoutMethod { get; private set; }

    public string? BankCode { get; private set; }
    public string? BranchCode { get; private set; }
    public string? AccountNumber { get; private set; }
    public string? AccountNumberHash { get; private set; }
    public string? AccountHolderName { get; private set; }

    public string? WalletProvider { get; private set; }
    public string? WalletMobileNumber { get; private set; }

    public string? PickupLocationCode { get; private set; }
    public string? BeneficiaryIdDocumentType { get; private set; }
    public string? BeneficiaryIdDocumentNumber { get; private set; }

    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string CreatedBy { get; private set; }

    private Beneficiary() { }

    public Beneficiary(
        string beneficiaryCode,
        Guid senderId,
        string fullName,
        string destinationCountry,
        PayoutMethod preferredPayoutMethod,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Beneficiary full name is required.");

        if (string.IsNullOrWhiteSpace(destinationCountry))
            throw new ArgumentException("Destination country is required.");

        Id = Guid.NewGuid();
        BeneficiaryCode = beneficiaryCode;
        SenderId = senderId;
        FullName = fullName;
        DestinationCountry = destinationCountry;
        PreferredPayoutMethod = preferredPayoutMethod;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void SetBankDetails(
        string bankCode,
        string branchCode,
        string accountNumber,
        string accountNumberHash,
        string accountHolderName)
    {
        BankCode = bankCode;
        BranchCode = branchCode;
        AccountNumber = accountNumber;
        AccountNumberHash = accountNumberHash;
        AccountHolderName = accountHolderName;
    }

    public void SetWalletDetails(string walletProvider, string walletMobileNumber)
    {
        WalletProvider = walletProvider;
        WalletMobileNumber = walletMobileNumber;
    }

    public void SetCashPickupDetails(
        string pickupLocationCode,
        string idDocumentType,
        string idDocumentNumber)
    {
        PickupLocationCode = pickupLocationCode;
        BeneficiaryIdDocumentType = idDocumentType;
        BeneficiaryIdDocumentNumber = idDocumentNumber;
    }

    public void ValidateBelongsToSender(Guid senderId)
    {
        if (SenderId != senderId)
            throw new InvalidOperationException(
                "Beneficiary does not belong to the specified sender.");
    }
}