namespace E_TicaretNew.Application.DTOs.ProductDTOs;

public class ProductFilterDto
{
    public Guid? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Search { get; set; }
}
