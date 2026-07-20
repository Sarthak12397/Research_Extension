public class Sender
{
    public Guid Id { get; private set; }
    public string SenderCode { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Nationality { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public string IdDocumentType { get; private set; }
    public string IdDocumentNumber { get; private set; }
    public string IdDocumentNumberHash { get; private set; }
    public bool IsKycVerified { get; private set; }
    public DateTime? KycVerifiedAt { get; private set; }
    public string KycRiskLevel { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }

    private Sender() { }

    public Sender(
        string senderCode,
        string fullName,
        string email,
        string phoneNumber,
        string nationality,
        DateOnly dateOfBirth,
        string idDocumentType,
        string idDocumentNumber,
        string idDocumentNumberHash,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.");

        if (string.IsNullOrWhiteSpace(idDocumentNumber))
            throw new ArgumentException("ID document number is required.");

        if (string.IsNullOrWhiteSpace(idDocumentNumberHash))
            throw new ArgumentException("ID document hash is required.");

        Id = Guid.NewGuid();
        SenderCode = senderCode;
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        Nationality = nationality;
        DateOfBirth = dateOfBirth;
        IdDocumentType = idDocumentType;
        IdDocumentNumber = idDocumentNumber;
        IdDocumentNumberHash = idDocumentNumberHash;
        IsKycVerified = false;
        KycRiskLevel = "Low";
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void MarkKycVerified(string verifiedBy, string riskLevel)
    {
        if (string.IsNullOrWhiteSpace(riskLevel))
            throw new ArgumentException("KYC risk level is required.");

        IsKycVerified = true;
        KycVerifiedAt = DateTime.UtcNow;
        KycRiskLevel = riskLevel;
        UpdatedBy = verifiedBy;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ValidateKycVerified()
    {
        if (!IsKycVerified)
            throw new KycRequiredException(SenderCode);
    }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }
}