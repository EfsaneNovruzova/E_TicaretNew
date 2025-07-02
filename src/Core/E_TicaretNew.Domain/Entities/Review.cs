﻿namespace E_Ticaret.Domain.Entities;

public class Review
{
    public string Comment { get; set; }

    public int Rating { get; set; } 
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}
