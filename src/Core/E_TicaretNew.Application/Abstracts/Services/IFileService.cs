using E_TicaretNew.Application.DTOs.FileDTOs;
using E_TicaretNew.Application.Shared.Responses;
using Microsoft.AspNetCore.Http;

namespace E_TicaretNew.Application.Abstracts.Services;
   public interface IFileService
   {
        Task<BaseResponse<string>> UploadFileAsync(FileUploadDto dto);
        Task<BaseResponse<bool>> DeleteFileAsync(FileDeleteDto dto);
   }


