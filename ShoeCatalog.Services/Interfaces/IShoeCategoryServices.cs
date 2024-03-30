using ShoeCatalog.Helpers;
using ShoeCatalog.DataModels.Models;

namespace ShoeCatalog.Services.Interfaces
{
    public interface IShoeCategoryServices
    {
        Task<List<ShoeCategory>> GetShoeCategoriesAsync(bool includeCategories = false);
        Task<List<ShoeCategory>> GetShoeCategoriesAsyncByShoeIds(string[] shoeIds);
    }
}
