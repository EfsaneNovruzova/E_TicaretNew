using E_TicaretNew.Application.DTOs.OrderProductDTOs;

namespace E_TicaretNew.Application.DTOs.OrderDTOs;

public class OrderUpdateDto
{
    public int Id { get; set; }  
    public int PaymentId { get; set; }
    public List<OrderProductUpdateDto> Products { get; set; }
}
