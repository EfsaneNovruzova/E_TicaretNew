using E_TicaretNew.Application.DTOs.OrderProductDTOs;
using E_TicaretNew.Domain.Enums.OrderEnum;

namespace E_TicaretNew.Application.DTOs.OrderDTOs;

public class OrderGetDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public Guid PaymentId { get; set; }
    public List<OrderProductGetDto> Products { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
}
