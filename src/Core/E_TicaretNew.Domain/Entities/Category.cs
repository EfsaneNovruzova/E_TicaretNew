namespace E_TicaretNew.Domain.Entities;

public class Category:BaseEntity
{
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }
    public ICollection<Category>? Children { get; set; }
    public ICollection<Product> Products { get; set; }
}
