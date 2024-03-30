using ShoeCatalog.DataModels.Models;
using ShoeCatalog.Repositories.Interfaces;
using ShoeCatalog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Services
{
    public class ShoeCategoryService : IShoeCategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoeCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ShoeCategory>> GetShoeCategoriesAsync(bool includeCategories = false)
        {
            var shoeCategories = await _unitOfWork.ShoeCategory
                                        .GetAllAsync(includeProperties: includeCategories ? "Category": null, tracked: false);

            return shoeCategories.ToList();
        }

        public async Task<List<ShoeCategory>> GetShoeCategoriesAsyncByShoeIds(string[] shoeIds)
        {
            var shoeCategories = await _unitOfWork.ShoeCategory
                                        .GetAllAsync(x => shoeIds.Contains(x.ShoeId), includeProperties: "Category");
            
            return shoeCategories.ToList();
        }
    }
}
