namespace E_TicaretNew.Application.DTOs.ReviewDTOs;

public class ReviewCreateDto
{
    public string Comment { get; set; }
    public int Rating { get; set; }
    public Guid ProductId { get; set; }
}