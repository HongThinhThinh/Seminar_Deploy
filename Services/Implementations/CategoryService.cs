using AutoMapper;
using Repositories.Interfaces;
using Repositories.Models;
using Services.DTOs;
using Services.Interfaces;

namespace Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetCategoriesWithProductsAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            return category != null ? _mapper.Map<CategoryDto>(category) : null;
        }

        public async Task<CategoryDto?> GetCategoryWithProductsAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetCategoryWithProductsAsync(id);
            return category != null ? _mapper.Map<CategoryDto>(category) : null;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            // Validate unique name
            if (!await _unitOfWork.Categories.IsCategoryNameUniqueAsync(createCategoryDto.Name))
            {
                throw new ArgumentException("Category name already exists");
            }

            var category = _mapper.Map<Category>(createCategoryDto);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return await GetCategoryByIdAsync(category.Id) ?? throw new InvalidOperationException("Failed to create category");
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return null;
            }

            // Validate unique name
            if (!await _unitOfWork.Categories.IsCategoryNameUniqueAsync(updateCategoryDto.Name, id))
            {
                throw new ArgumentException("Category name already exists");
            }

            _mapper.Map(updateCategoryDto, existingCategory);
            await _unitOfWork.Categories.UpdateAsync(existingCategory);
            await _unitOfWork.SaveChangesAsync();

            return await GetCategoryByIdAsync(id);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetCategoryWithProductsAsync(id);
            if (category == null)
            {
                return false;
            }

            // Check if category has products
            if (category.Products.Any(p => !p.IsDeleted))
            {
                throw new InvalidOperationException("Cannot delete category that contains products");
            }

            // Soft delete
            category.IsDeleted = true;
            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _unitOfWork.Categories.ExistsAsync(id);
        }

        public async Task<bool> IsCategoryNameUniqueAsync(string name, int? excludeId = null)
        {
            return await _unitOfWork.Categories.IsCategoryNameUniqueAsync(name, excludeId);
        }
    }
}
