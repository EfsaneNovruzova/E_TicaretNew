namespace E_Ticaret.Domain.Entities;

public class Review
{
    public string Comment { get; set; }

    public int Rating { get; set; } 
    public int UserId { get; set; }
    public User User { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
