using ShoeCatalog.DataModels.Models;
using ShoeCatalog.Helpers;

namespace ShoeCatalog.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ResponseData<IEnumerable<Category>>> GetAllAsync();
        Task<ResponseData<Category>> GetOneAsync(string id);

        Task<ResponseData<Category>> AddAsync(Category category);
        Task<ResponseData<Category>> UpdateAsync(Category category);
        Task<ResponseData<Category>> DeleteAsync(Category category);
    }
}
