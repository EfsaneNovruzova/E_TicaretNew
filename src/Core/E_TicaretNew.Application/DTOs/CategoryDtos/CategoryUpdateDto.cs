namespace E_TicaretNew.Application.DTOs.CategoryDtos;
public class CategoryUpdateDto
{ 
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid? ParentId { get; set; }
}
