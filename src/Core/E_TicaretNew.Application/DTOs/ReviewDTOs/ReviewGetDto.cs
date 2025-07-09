namespace E_TicaretNew.Application.DTOs.ReviewDTOs;

public class ReviewGetDto
{
    public Guid Id { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
    public string UserName { get; set; }
}
