using AutoMapper;
using Repositories.Interfaces;
using Repositories.Models;
using Services.DTOs;
using Services.Interfaces;

namespace Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetProductsWithCategoryAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetProductWithCategoryAsync(id);
            return product != null ? _mapper.Map<ProductDto>(product) : null;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _unitOfWork.Products.GetProductsByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            var products = await _unitOfWork.Products.SearchProductsAsync(searchTerm);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            // Validate category exists
            if (!await _unitOfWork.Categories.ExistsAsync(createProductDto.CategoryId))
            {
                throw new ArgumentException("Category does not exist");
            }

            // Validate unique name
            if (!await _unitOfWork.Products.IsProductNameUniqueAsync(createProductDto.Name))
            {
                throw new ArgumentException("Product name already exists");
            }

            var product = _mapper.Map<Product>(createProductDto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return await GetProductByIdAsync(product.Id) ?? throw new InvalidOperationException("Failed to create product");
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return null;
            }

            // Validate category exists
            if (!await _unitOfWork.Categories.ExistsAsync(updateProductDto.CategoryId))
            {
                throw new ArgumentException("Category does not exist");
            }

            // Validate unique name
            if (!await _unitOfWork.Products.IsProductNameUniqueAsync(updateProductDto.Name, id))
            {
                throw new ArgumentException("Product name already exists");
            }

            _mapper.Map(updateProductDto, existingProduct);
            await _unitOfWork.Products.UpdateAsync(existingProduct);
            await _unitOfWork.SaveChangesAsync();

            return await GetProductByIdAsync(id);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }

            // Soft delete
            product.IsDeleted = true;
            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _unitOfWork.Products.ExistsAsync(id);
        }

        public async Task<bool> IsProductNameUniqueAsync(string name, int? excludeId = null)
        {
            return await _unitOfWork.Products.IsProductNameUniqueAsync(name, excludeId);
        }
    }
}
