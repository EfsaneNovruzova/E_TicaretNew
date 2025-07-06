using E_TicaretNew.Application.DTOs.OrderProductDTOs;

namespace E_TicaretNew.Application.DTOs.OrderDTOs;

public class OrderCreateDto
{
    public string UserId { get; set; }  
    public int PaymentId { get; set; }  

    public List<OrderProductCreateDto> Products { get; set; }
}
