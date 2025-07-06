namespace E_TicaretNew.Application.DTOs.ProductDTOs;

public class ProductGetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }

    public string UserId { get; set; }
    public string CategoryName { get; set; }

    //public List<ImageGetDto> Images { get; set; }
    public int FavoritesCount { get; set; }
    public int ReviewsCount { get; set; }
}
