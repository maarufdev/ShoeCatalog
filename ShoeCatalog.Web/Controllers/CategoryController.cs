using Microsoft.AspNetCore.Mvc;
using ShoeCatalog.DataModels.Models;
using ShoeCatalog.Helpers;
using ShoeCatalog.Services;
using ShoeCatalog.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace ShoeCatalog.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();

            if (result.Status == RequestStatus.Failed)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(result.Model);
        }

        [HttpGet(Name = "Get")]
        public async Task<IActionResult> GetOnById(string id)
        {

            var response = await _categoryService.GetOneAsync(id);

            if (response.Status == RequestStatus.Success)
            {
                return Ok(response.Model);
            }

            if (response.Model == null)
            {
                return NotFound();
            }

            return BadRequest("Request is invalid");
        }


        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Id = Guid.NewGuid().ToString();

                var result = await _categoryService.AddAsync(category);

                if(result.Status == RequestStatus.Success)
                {
                    return Ok(category);
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return BadRequest("Request is invalid.");
        }


        [HttpPut]
        public async Task<IActionResult> Update(Category category)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(category.Id))
            {
                var result = await _categoryService.UpdateAsync(category);

                if (result.Status == RequestStatus.Success)
                {
                    return Ok(category);
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return BadRequest("BrandRepository Id is not found");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _categoryService.GetOneAsync(id);

            if(result.Status == RequestStatus.Success && result.Model != null)
            {
                var deleteResult = await _categoryService.DeleteAsync(result.Model);

                if(deleteResult.Status == RequestStatus.Success)
                {
                    return Ok("Successfully deleted.");
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NotFound();
        }
    }
}
