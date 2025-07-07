using Microsoft.AspNetCore.Http;

namespace E_TicaretNew.Application.DTOs.FileDTOs;

public class FileUploadDto
{
    public IFormFile File { get; set; } = null!;
    //public string? Description { get; set; } 
}
