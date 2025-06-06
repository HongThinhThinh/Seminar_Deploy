using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesWithProductsAsync();
        Task<Category?> GetCategoryWithProductsAsync(int id);
        Task<bool> IsCategoryNameUniqueAsync(string name, int? excludeId = null);
    }
}
