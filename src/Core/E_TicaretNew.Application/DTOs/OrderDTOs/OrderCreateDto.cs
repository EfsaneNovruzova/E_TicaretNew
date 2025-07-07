using E_TicaretNew.Application.DTOs.OrderProductDTOs;

namespace E_TicaretNew.Application.DTOs.OrderDTOs;

public class OrderCreateDto
{
   
    public Guid PaymentId { get; set; }
  
    public List<OrderProductCreateDto> Products { get; set; }
}
