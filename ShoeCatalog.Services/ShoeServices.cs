using ShoeCatalog.DataModels.Models;
using ShoeCatalog.DataModels.ViewModels.ShoeVM;
using ShoeCatalog.Helpers;
using ShoeCatalog.Repositories;
using ShoeCatalog.Repositories.Interfaces;
using ShoeCatalog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Services
{
    public class ShoeServices : IShoeServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoeServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Shoe> GetShoeByIdAsync(string shoeId)
        {
            var shoe = await _unitOfWork.Shoe.GetFirstOrDefaultAsync(x => x.Id == shoeId, tracked: false);

            return shoe;
        }

        public async Task<List<ShoeListVM>> GetShoeListAsync()
        {
            List<ShoeListVM> shoeListVM = new();

            var shoes = await _unitOfWork.Shoe.GetAllAsync(includeProperties: "ShoeCategories,Brand", tracked: false);

            if (shoes != null && shoes.Any())
            {
                // get all shoeIds
                var shoeIds = shoes.Select(x => x.Id).ToArray();

                var shoeCategories = await _unitOfWork.ShoeCategory
                                        .GetAllAsync(x => shoeIds.Contains(x.ShoeId), includeProperties: "Category");

                shoeListVM = shoes.Select(x => new ShoeListVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Brand = x.Brand?.Name,
                    Category = string.Join(", ", shoeCategories
                                                    .Where(sc => sc.ShoeId == x.Id)
                                                    .Select(c => c.Category?.Name)
                                                    .ToArray())
                }).ToList();
            }


            return shoeListVM;
        }

        public async Task<ShoeDetailsVM> GetShoeDetailsByIdAsync(string shoeId)
        {
            var shoeDetails = await _unitOfWork.Shoe.
                              GetFirstOrDefaultAsync(
                                    x => x.Id == shoeId,
                                    includeProperties: "Brand,ShoeCategories",
                                    tracked: false
                                    );

            var shoeCategories = shoeDetails.ShoeCategories?
                                .Select(x => x.CategoryId).ToList();

            var selectedCategories = await _unitOfWork.Category
                                    .GetAllAsync(x => shoeCategories!.Contains(x.Id));


            ShoeDetailsVM shoe = new()
            {
                Name = shoeDetails.Name,
                BrandName = shoeDetails.Brand?.Name,
                ShortDescription = shoeDetails.ShortDescription,
                FullDescription = shoeDetails.FullDescription,
                ImageFileName = shoeDetails.ImageFileName,
                Color = shoeDetails.Color,
                Size = shoeDetails.Size,
                Gender = shoeDetails.Gender,
                Price = shoeDetails.Price,
                IsActive = shoeDetails.IsActive,
                ShoeCategories = selectedCategories.Select(x => x.Name!).ToList(),
            };

            return shoe;
        }

        public async Task<List<ShoeCardVM>> GetShoeListCard()
        {
            List<ShoeCardVM> shoeCards = new();

            var shoes = await _unitOfWork.Shoe.GetAllAsync(includeProperties: "Brand,ShoeCategories");

            if (shoes != null)
            {
                var categories = await _unitOfWork.Category.GetAllAsync();

                shoeCards = shoes.Select(x => new ShoeCardVM
                {
                    ShoeId = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Image = x.ImageFileName,
                    Brand = x.Brand?.Name,
                    Categories = categories
                                    .Where(
                                            c => x.ShoeCategories!
                                            .Select(sc => sc.CategoryId!)!
                                            .Contains(x.Id)!
                                            )
                                    .Select(c => c.Name)
                                    .ToList()!
                }).ToList();
            }

            return shoeCards;
        }

        public async Task<bool> DeleteShoeById(string shoeId)
        {
            var shoeToDelete = await _unitOfWork.Shoe.GetFirstOrDefaultAsync(x => x.Id == shoeId, tracked: false);

            if(shoeToDelete != null)
            {
                await _unitOfWork.Shoe.RemoveAsync(shoeToDelete);

                return await _unitOfWork.SaveAsync();
            }

            return false;
        }

        public Task<ResponseData<Shoe>> AddShoeAsync(Shoe shoe)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Shoe>> UpdateShoeAsync(Shoe shoe)
        {
            throw new NotImplementedException();
        }
    }
}
