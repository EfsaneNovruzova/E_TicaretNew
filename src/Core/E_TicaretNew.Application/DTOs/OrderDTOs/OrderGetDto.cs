using E_TicaretNew.Application.DTOs.OrderProductDTOs;

namespace E_TicaretNew.Application.DTOs.OrderDTOs;

public class OrderGetDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
   // public PaymentDto Payment { get; set; }
    public List<OrderProductGetDto> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
