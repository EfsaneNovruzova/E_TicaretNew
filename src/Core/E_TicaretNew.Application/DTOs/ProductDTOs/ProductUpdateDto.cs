namespace E_TicaretNew.Application.DTOs.ProductDTOs;

public class ProductUpdateDto
{
    public Guid Id { get; set; }  
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
}
