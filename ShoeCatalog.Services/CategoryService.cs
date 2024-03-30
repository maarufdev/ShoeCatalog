using ShoeCatalog.DataModels.Models;
using ShoeCatalog.Helpers;
using ShoeCatalog.Repositories.Interfaces;
using ShoeCatalog.Services.Interfaces;

namespace ShoeCatalog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData<Category>> AddAsync(Category category)
        {
            try
            {
                await _unitOfWork.Category.AddAsync(category);
                await _unitOfWork.SaveAsync();

                return new ResponseData<Category>
                {
                    Model = category,
                    Message = "Successfully created"
                };
            } catch(Exception ex)
            {
                return new ResponseData<Category>
                {
                    Message = ex.Message,
                    Status = RequestStatus.Failed,
                };
            }

        }

        public async Task<ResponseData<Category>> DeleteAsync(Category category)
        {
            try
            {
                await _unitOfWork.Category.RemoveAsync(category);
                await _unitOfWork.SaveAsync();

                return new ResponseData<Category>
                {
                    Message = "Successfully created.",
                    Model = category
                };

            } catch(Exception ex)
            {
                return new ResponseData<Category>
                {
                    Message = ex.Message,
                    Status = RequestStatus.Failed,
                };
            }
        }

        public async Task<ResponseData<IEnumerable<Category>>> GetAllAsync()
        {
            try
            {
                return new ResponseData<IEnumerable<Category>>
                {
                    Model = await _unitOfWork.Category.GetAllAsync(tracked: false),
                };

            } catch(Exception ex)
            {
                return new ResponseData<IEnumerable<Category>>
                {
                    Message = ex.Message,
                    Status = RequestStatus.Failed,
                };
            }
                
        }

        public async Task<ResponseData<Category>> GetOneAsync(string id)
        {
            try
            {
                return new ResponseData<Category>
                {
                    Model =  await _unitOfWork.Category.GetFirstOrDefaultAsync(x=> x.Id == id, tracked: false)
                };

            } catch(Exception ex)
            {
                return new ResponseData<Category>
                {
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseData<Category>> UpdateAsync(Category category)
        {
            try
            {
                await _unitOfWork.Category.Update(category);
                await _unitOfWork.SaveAsync();

                return new ResponseData<Category>
                {
                    Model = category
                };


            } catch(Exception ex)
            {
                return new ResponseData<Category>
                {
                    Message = ex.Message,
                    Status = RequestStatus.Failed
                };
            }
        }
    }
}
