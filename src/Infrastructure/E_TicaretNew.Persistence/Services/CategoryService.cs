using System.Net;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.CategoryDtos;
using E_TicaretNew.Application.Shared.Responses;
using E_TicaretNew.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_TicaretNew.Persistence.Services;

public class CategoryService : ICategoryService
{
    private ICategoryRepository _categoryRepository { get; }

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }


    public async Task<BaseResponse<string>> AddAsync(CategoryCreateDto dto)
    {
        var categoryDb = await _categoryRepository
             .GetByFiltered(c => c.Name.ToLower().Trim() == dto.Name.ToLower().Trim())
             .FirstOrDefaultAsync();

        if (categoryDb is not null)
        {
            return new BaseResponse<string>("This category already exists", HttpStatusCode.BadRequest);
        }

        Category category = new()
        {
            Name = dto.Name,
            ParentId = dto.ParentId
        };

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangeAsync();

        return new BaseResponse<string>(HttpStatusCode.Created);
    }



    public  async Task<BaseResponse<string>> DeleteAsync(Guid id)
    {
        var entity = await _categoryRepository.GetByIdAsync(id);

        if (entity == null)
        {
            return new BaseResponse<string>(
                "Category not found",
                false,
                HttpStatusCode.NotFound);
        }

        _categoryRepository.Delete(entity);
        await _categoryRepository.SaveChangeAsync();

        return new BaseResponse<string>(
            "Category deleted successfully",
            "Deleted",
            HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<CategoryGetDto>>> GetAll()
    {
        var allCategories = await _categoryRepository.GetAll().ToListAsync();

        if (allCategories == null || allCategories.Count == 0)
        {
            return new BaseResponse<List<CategoryGetDto>>(
                "No categories found",
                false,
                HttpStatusCode.NotFound);
        }

        var mainCategories = allCategories
            .Where(c => c.ParentId == null)
            .Select(c => MapWithChildren(c, allCategories))
            .ToList();

        return new BaseResponse<List<CategoryGetDto>>(
            "Nested categories retrieved successfully",
            mainCategories,
            HttpStatusCode.OK);
    }

    private CategoryGetDto MapWithChildren(Category category, List<Category> allCategories)
    {
        return new CategoryGetDto
        {
            Id = category.Id,
            Name = category.Name,
            Children = allCategories
                .Where(c => c.ParentId == category.Id)
                .Select(child => MapWithChildren(child, allCategories))
                .ToList()
        };
    }


    public async Task<BaseResponse<CategoryGetDto>> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            return new BaseResponse<CategoryGetDto>(
                "Category not found",
                false,
                HttpStatusCode.NotFound);
        }

        var categoryDto = new CategoryGetDto
        {
            Id = category.Id,
            Name = category.Name

        };

        return new BaseResponse<CategoryGetDto>(
            "Category retrieved successfully",
            categoryDto,
            HttpStatusCode.OK);
    }

    public async Task<BaseResponse<CategoryGetDto>> GetByNameAsync(string search)
    {
        var category = await _categoryRepository
             .GetByFiltered(c => c.Name.ToLower().Trim() == search.ToLower().Trim())
             .FirstOrDefaultAsync();

        if (category == null)
        {
            return new BaseResponse<CategoryGetDto>(
                "Category not found",
                false,
                HttpStatusCode.NotFound);
        }

        var categoryDto = new CategoryGetDto
        {
            Id = category.Id,
            Name = category.Name

        };

        return new BaseResponse<CategoryGetDto>(
            "Category retrieved successfully",
            categoryDto,
            HttpStatusCode.OK);
    }

    public async Task<BaseResponse<CategoryUpdateDto>> UpdateAsync(CategoryUpdateDto dto)
    {
        var categoryDb = await _categoryRepository.GetByIdAsync(dto.Id);
        if (categoryDb is null)
        {
            return new BaseResponse<CategoryUpdateDto>(HttpStatusCode.NotFound);
        }

        var existedCategory = await _categoryRepository
           .GetByFiltered(c => c.Name.ToLower().Trim() == dto.Name.ToLower().Trim())
           .FirstOrDefaultAsync();

        if (existedCategory is not null)
        {
            return new BaseResponse<CategoryUpdateDto>("This category already exit", HttpStatusCode.BadRequest);
        }
        categoryDb.Name = dto.Name;
        categoryDb.ParentId = dto.ParentId;
        await _categoryRepository.SaveChangeAsync();
        return new BaseResponse<CategoryUpdateDto>("Successfully update", dto, HttpStatusCode.OK);

    }
}
