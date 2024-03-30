using ShoeCatalog.DataModels.Models;
using ShoeCatalog.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Services.Interfaces
{
    public interface IBrandService
    {
        Task<ResponseData<IEnumerable<Brand>>> GetAllAsync();
        Task<ResponseData<Brand>> GetOneAsync(string id);
        Task<ResponseData<Brand>> AddAsync(Brand brand);
        Task<ResponseData<Brand>> UpdateAsync(Brand brand);
        Task<ResponseData<Brand>> DeleteAsync(Brand brand);
    }
}
