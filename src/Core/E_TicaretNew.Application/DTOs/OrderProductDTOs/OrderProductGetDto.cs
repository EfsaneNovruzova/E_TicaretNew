namespace E_TicaretNew.Application.DTOs.OrderProductDTOs;

public class OrderProductGetDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }  
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
