using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.FileDTOs;
using E_TicaretNew.Application.Shared.Responses;
using Microsoft.AspNetCore.Hosting;
using System.Net;


public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<BaseResponse<string>> UploadFileAsync(FileUploadDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return new BaseResponse<string>("File is empty", false, HttpStatusCode.BadRequest);

        try
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(dto.File.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.File.CopyToAsync(stream);

            var relativePath = Path.Combine("uploads", uniqueFileName);
            return new BaseResponse<string>("File uploaded successfully", relativePath, HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>($"File upload failed: {ex.Message}", false, HttpStatusCode.InternalServerError);
        }
    }

    public Task<BaseResponse<bool>> DeleteFileAsync(FileDeleteDto dto)
    {
        try
        {
            var fullPath = Path.Combine(_env.WebRootPath, dto.FilePath);
            if (!File.Exists(fullPath))
                return Task.FromResult(new BaseResponse<bool>("File not found", false, HttpStatusCode.NotFound));

            File.Delete(fullPath);
            return Task.FromResult(new BaseResponse<bool>("File deleted successfully", true, HttpStatusCode.OK));
        }
        catch (Exception ex)
        {
            return Task.FromResult(new BaseResponse<bool>($"File delete failed: {ex.Message}", false, HttpStatusCode.InternalServerError));
        }
    }
}


