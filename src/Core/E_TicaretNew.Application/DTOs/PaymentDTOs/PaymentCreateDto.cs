namespace E_TicaretNew.Application.DTOs.PaymentDTOs;

public class PaymentCreateDto
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public string? TransactionId { get; set; }
}
