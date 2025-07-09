using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.FileDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static E_TicaretNew.Application.Abstracts.Services.IFileService;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    [Authorize(Policy = "Files.Upload")]
    public async Task<IActionResult> Upload([FromForm] FileUploadDto dto)
    {
        var result = await _fileService.UploadFileAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete("delete")]
    [Authorize(Policy = "Files.Delete")]
    public async Task<IActionResult> Delete([FromQuery] FileDeleteDto dto)
    {
        var result = await _fileService.DeleteFileAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }
}

