public class SettleDebtRequest
{
    public Guid TripId { get; set; }
    public Guid DebtorId { get; set; }
    public Guid CreditorId { get; set; }
}