public record PartnerPayoutResponse(
    bool IsSuccess,
    PartnerPayoutStatus PartnerStatus,
    string? PartnerTransactionReference,
    string RawJson,
    string? FailureReason);



public interface IPayoutPartnerAdapter
{
    // SendPayoutInstruction: dispatch payout to partner API
    // Must be idempotent on the partner side via idempotency key
    Task<PartnerPayoutResponse> SendPayoutInstructionAsync(
        PayoutInstruction instruction,
        CancellationToken ct = default);

    // VerifyWebhookSignature: HMAC-SHA256 verification using per-partner shared secret
    // Secret must come from environment/secret manager — never hardcoded
    bool VerifyWebhookSignature(string rawPayload, string signature, string partnerCode);

    // PollPayoutStatus: for pending instructions where webhook was not received
    Task<PartnerPayoutStatus> PollPayoutStatusAsync(
        string partnerTransactionReference,
        string partnerCode,
        CancellationToken ct = default);
}