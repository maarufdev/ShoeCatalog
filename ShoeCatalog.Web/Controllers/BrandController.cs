using Microsoft.AspNetCore.Mvc;
using ShoeCatalog.DataModels.Models;
using ShoeCatalog.Services.Interfaces;
using ShoeCatalog.Helpers;
using System;

namespace ShoeCatalog.Web.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var result = await _brandService.GetAllAsync();

            if (result.Status == RequestStatus.Failed)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(result.Model);
        }

        [HttpGet(Name = "Get")]
        public async Task<IActionResult> GetOneById(string id)
        {
            var response = await _brandService.GetOneAsync(id);

            if (response.Status == RequestStatus.Success)
            {
                return Ok(response.Model);
            }

            if(response.Model == null)
            {
                return NotFound();
            }

            return BadRequest("Request is invalid");

        }

        [HttpPost]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (string.IsNullOrEmpty(brand.Name))
            {
                return BadRequest("BrandRepository Name is required.");
            }

            brand.Id = Guid.NewGuid().ToString();
            await _brandService.AddAsync(brand);

            return Ok(brand);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Brand brand)
        {
            if(!string.IsNullOrEmpty(brand.Id))
            {
                var result = await _brandService.UpdateAsync(brand);

                if(result.Status == RequestStatus.Success)
                {
                    return Ok(brand);
                }

                return StatusCode(StatusCodes.Status500InternalServerError);

            }

            return BadRequest("BrandRepository Id is null");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {

            var result = await _brandService.GetOneAsync(id);

            if(result.Model == null)
            {
                return NotFound(result.Message);
            }

            var deleteResult = await _brandService.DeleteAsync(result.Model);

            if (deleteResult.Status == RequestStatus.Success)
            {
                return Ok(deleteResult.Message);
            }


            return BadRequest(result.Message);
        }
    }
}
