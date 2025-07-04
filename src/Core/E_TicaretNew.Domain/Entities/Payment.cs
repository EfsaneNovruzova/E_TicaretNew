using E_TicaretNew.Domain.Entities;

namespace E_TicaretNew.Domain.Entities;


public class Payment : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public decimal Amount { get; set; } 

    public string PaymentMethod { get; set; } = null!; 
     public bool IsSuccessful { get; set; } = false;  

    public string? TransactionId { get; set; }  // Bank və ya ödəniş sisteminin əməliyyat nömrəsi
}

