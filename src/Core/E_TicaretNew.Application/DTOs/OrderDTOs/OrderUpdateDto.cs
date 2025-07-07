using E_TicaretNew.Application.DTOs.OrderProductDTOs;
using E_TicaretNew.Domain.Enums.OrderEnum;

namespace E_TicaretNew.Application.DTOs.OrderDTOs;

public class OrderUpdateDto
{
    public int Id { get; set; }  
    public Guid PaymentId { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderProductUpdateDto> Products { get; set; }
}
