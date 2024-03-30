using ShoeCatalog.DataModels.Models;
using ShoeCatalog.DataModels.ViewModels.ShoeVM;
using ShoeCatalog.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Services.Interfaces
{
    public interface IShoeServices
    {
        Task<List<ShoeListVM>> GetShoeListAsync();
        Task<Shoe> GetShoeByIdAsync(string shoeId);
        Task<ShoeDetailsVM> GetShoeDetailsByIdAsync(string shoeId);
        Task<List<ShoeCardVM>> GetShoeListCard();
        // for create
        Task<ResponseData<Shoe>> AddShoeAsync(Shoe shoe);
        Task<ResponseData<Shoe>> UpdateShoeAsync(Shoe shoe);
        // for update

        Task<bool> DeleteShoeById(string shoeId);
    }
}
