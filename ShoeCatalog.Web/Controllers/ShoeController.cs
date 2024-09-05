using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoeCatalog.DataModels.Models;
using ShoeCatalog.DataModels.ViewModels.Common;
using ShoeCatalog.DataModels.ViewModels.ShoeVM;
using ShoeCatalog.Repositories.Interfaces;
using ShoeCatalog.Services;
using ShoeCatalog.Services.Interfaces;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;

namespace ShoeCatalog.Web.Controllers
{
    public class ShoeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ImageUploadService _imageUploadService;
        private readonly IShoeServices _shoeService;
        private readonly IShoeCategoryServices _shoeCategoryServices;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        public ShoeController(
            IUnitOfWork unitOfWork, 
            ImageUploadService imageUploadService, 
            IShoeServices shoeService,
            IShoeCategoryServices shoeCategoryServices,
            IBrandService brandService,
            ICategoryService categoryService)
        {
            _unitOfWork = unitOfWork;
            _imageUploadService = imageUploadService;
            _shoeService = shoeService;
            _shoeCategoryServices = shoeCategoryServices;
            _brandService = brandService;
            _categoryService = categoryService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<ShoeListVM> shoeListVM = new();

            // retrieve all shoes
            var result = await _shoeService.GetShoeListAsync();
            
            if(result != null)
            {
                return Ok(result);
            }

            return Ok(shoeListVM);
        }

        [HttpGet]
        public IActionResult Index() => View();

        private async Task<ShoeUpsertVM> InitializeModel(string? id)
        {
            ShoeUpsertVM shoe = new();

            //var brands = await _unitOfWork.BrandRepository.GetAllAsync();
            var brandsResponse = await _brandService.GetAllAsync();
            var brands = brandsResponse.Model;

            shoe.BrandList = brands?.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id
            });

            //var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            var categoryResponse = await _categoryService.GetAllAsync();
            var categories = categoryResponse.Model;

            if (id == null)
            {
                shoe.CategoryList = categories?.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id
                });
            }
            else
            {
                shoe.Shoe = await _unitOfWork.ShoeRepository
                            .GetFirstOrDefaultAsync(x => x.Id == id, 
                                                    includeProperties: "ShoeCategories", 
                                                    tracked: false);

                var selectedCategories = shoe.Shoe.ShoeCategories?.Select(x => x.CategoryId).ToList();

                shoe.CategoryList = categories!.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id,
                    Selected = selectedCategories!.Contains(x.Id)
                });
            }

            shoe.GenderList = Enum.GetValues(typeof(Gender)).Cast<Gender>()
                             .Select(s => new SelectListItem
                             {
                                 Text = s.ToString(),
                                 Value = s.ToString()
                             });

            return shoe;
        }
        [HttpGet]
        public async Task<IActionResult> Upsert(string? id)
        {
            ShoeUpsertVM shoeVM = await InitializeModel(id);

            if (id == null)
            {
                if (!ModelState.IsValid)
                {
                    shoeVM = await InitializeModel(null);

                    return View(shoeVM);
                }

                return View(shoeVM);
            }

            return View(shoeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(ShoeUpsertVM shoeVM, IFormFile? file)
        {
            string uploadedFile = string.Empty;

            if (file != null)
            {
                uploadedFile = await _imageUploadService.UploadFile(file);
            }

            if (ModelState.IsValid && shoeVM.SelectedCategory != null)
            {
                if (string.IsNullOrEmpty(shoeVM.Shoe.Id))
                {
                    shoeVM.Shoe.Id = Guid.NewGuid().ToString();

                    if (!string.IsNullOrEmpty(uploadedFile))
                    {
                        shoeVM.Shoe.ImageFileName = uploadedFile;
                    }

                    var categories = await _unitOfWork.CategoryRepository.GetAllAsync(x => shoeVM.SelectedCategory
                                                                                 .Contains(x.Id));

                    var shoeCategoryToAdd = categories
                                           .Select(x => new ShoeCategory { ShoeId = shoeVM.Shoe.Id, CategoryId = x.Id })
                                           .ToList();

                    shoeVM.Shoe.ShoeCategories = shoeCategoryToAdd;

                    //await _unitOfWork.ShoeRepository.AddAsync(shoeVM.ShoeRepository);
                    await _unitOfWork.ShoeRepository.AddShoeAsync(shoeVM.Shoe);

                    var result = await _unitOfWork.SaveAsync();

                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    
                    // get existing shoe that needs to be updated
                    var existingShoe = await _unitOfWork.ShoeRepository
                                       .GetFirstOrDefaultAsync(x => x.Id == shoeVM.Shoe.Id, 
                                                               includeProperties: "ShoeCategories", 
                                                               tracked: true);
                    
                    var oldCategoryIds = existingShoe.ShoeCategories!.Select(c => c.CategoryId).ToList();
                    var newCategoryIds = shoeVM.SelectedCategory
                                        .Where(c => !oldCategoryIds.Contains(c)).ToList();

                    if (newCategoryIds.Any())
                    {
                        // get new categories
                        var newCategories = await _unitOfWork.CategoryRepository.GetAllAsync(c => newCategoryIds.Contains(c.Id!),
                                                                                        tracked: false);

                        // perform add the new categories.
                        if (newCategories.Any())
                        {
                            foreach(var item in newCategories.ToList())
                            {
                                var newShoeCategoryToAdd = new ShoeCategory
                                {
                                    CategoryId = item.Id,
                                    ShoeId = shoeVM.Shoe.Id
                                };

                                existingShoe.ShoeCategories.Add(newShoeCategoryToAdd);
                            }
                        }
                    }

                    var categoryToRemove = existingShoe?.ShoeCategories!
                                                       .Where(sc => !shoeVM.SelectedCategory.Contains(sc.CategoryId));

                   if(categoryToRemove is not null)
                    {
                        if (categoryToRemove.Any())
                        {
                            foreach(var item in categoryToRemove.ToList())
                            {
                                existingShoe.ShoeCategories.Remove(item);
                            }
                        }
                    }


                    existingShoe!.Name = shoeVM.Shoe.Name;
                    existingShoe.Price = shoeVM.Shoe.Price;
                    existingShoe.ShortDescription = shoeVM.Shoe.ShortDescription;
                    existingShoe.FullDescription = shoeVM.Shoe.FullDescription;
                    existingShoe.Gender = shoeVM.Shoe.Gender;
                    existingShoe.Size = shoeVM.Shoe.Size;
                    existingShoe.BrandId = shoeVM.Shoe.BrandId;
                    existingShoe.DateModified = DateTime.Now;
                    existingShoe.Color = shoeVM.Shoe.Color;

                    if (!string.IsNullOrEmpty(uploadedFile))
                    {
                        existingShoe.ImageFileName = uploadedFile;
                    }

                    await _unitOfWork.ShoeRepository.Update(existingShoe);

                    var result = await _unitOfWork.SaveAsync();

                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

            }

            var reInitialized = await InitializeModel(shoeVM.Shoe.Id);
            shoeVM.BrandList = reInitialized.BrandList;
            shoeVM.CategoryList = reInitialized.CategoryList;
            shoeVM.GenderList = reInitialized.GenderList;

            return View(shoeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var result = await _shoeService.GetShoeDetailsByIdAsync(id);

            if(result!= null)
            {
                return View(result);
            }

            return RedirectToAction(nameof(Index));

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _shoeService.DeleteShoeById(id);
            
            if (result)
            {
                return Ok("Successfully deleted");
            }

            return NotFound("ShoeRepository was not found");
        }

        [HttpGet]
        public IActionResult ShoeList() =>  View();


        [HttpGet]
        public async Task<IActionResult> ShoeListApi()
        {
            var shoeCards = await _shoeService.GetShoeListCard();

            if (shoeCards != null) return Ok(shoeCards); 
            
            return Ok(new List<ShoeCardVM>());
        }
    }
}
