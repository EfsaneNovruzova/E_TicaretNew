namespace E_TicaretNew.Application.DTOs.ProductDTOs;

public class ProductCreateDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string UserId { get; set; }
    public Guid CategoryId { get; set; }
}
