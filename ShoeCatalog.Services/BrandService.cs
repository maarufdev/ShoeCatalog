using ShoeCatalog.DataModels.Models;
using ShoeCatalog.Helpers;
using ShoeCatalog.Repositories.Interfaces;
using ShoeCatalog.Services.Interfaces;

namespace ShoeCatalog.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BrandService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<Brand>> AddAsync(Brand brand)
        {
            try
            {
                await _unitOfWork.BrandRepository.AddAsync(brand);
                await _unitOfWork.SaveAsync();

                return new ResponseData<Brand>
                {
                    Model = brand,
                    Message = "Successfully added.",
                };

            } catch (Exception ex)
            {
                return new ResponseData<Brand>
                {
                    Message = ex.Message,
                    Status = RequestStatus.Failed,
                };
            }
        }

        public async Task<ResponseData<Brand>> DeleteAsync(Brand brand)
        {
            try
            {
                await _unitOfWork.BrandRepository.RemoveAsync(brand);
                await _unitOfWork.SaveAsync();
                
                return new ResponseData<Brand>
                {
                    Model = brand,
                    Message = "Success"
                };

            }
            catch (Exception ex) 
            {
                return new ResponseData<Brand>
                {
                    Message = ex.Message,
                    Status = RequestStatus.Failed,
                };
            }
        }

        public async Task<ResponseData<IEnumerable<Brand>>> GetAllAsync()
        {
            try
            {
                return new ResponseData<IEnumerable<Brand>>
                {
                    Model = await _unitOfWork.BrandRepository.GetAllAsync(tracked: false),
                    Message = "Success",
                };
            
            } catch(Exception ex)
            {
                return new ResponseData<IEnumerable<Brand>>
                {
                    Model = null,
                    Message = ex.Message,
                    Status = RequestStatus.Failed
                };
            }
        }

        public async Task<ResponseData<Brand>> GetOneAsync(string id)
        {
            try
            {
                var brand = await _unitOfWork.BrandRepository.GetFirstOrDefaultAsync(x => x.Id == id, tracked: false);
                
                return new ResponseData<Brand>
                {
                    Model = brand,
                    Status = (brand != null) ? RequestStatus.Success : RequestStatus.Failed,
                    Message = (brand != null) ? "Success" : $"BrandRepository with id: {id} was not found."
                };

            } catch(Exception ex)
            {
                return new ResponseData<Brand>
                {
                    Model = null,
                    Status = RequestStatus.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseData<Brand>> UpdateAsync(Brand brand)
        {
            try
            {
                await _unitOfWork.BrandRepository.Update(brand);
                await _unitOfWork.SaveAsync();
                
                return new ResponseData<Brand>
                {
                    Model = brand,
                    Message = "Successfully updated."
                };

            } catch(Exception ex)
            {
                return new ResponseData<Brand>
                {
                    Model = null,
                    Message = ex.Message,
                    Status = RequestStatus.Failed,
                };
            }
        }
    }
}