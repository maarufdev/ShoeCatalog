using ShoeCatalog.DataModels.Models;
using ShoeCatalog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Repositories.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        //IGenericRepository<ShoeRepository> ShoeRepository { get; }
        IShoeRepository ShoeRepository { get; }
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<Brand> BrandRepository { get; }
        IGenericRepository<ShoeCategory> ShoeCategory { get; }
        IGenericRepository<Cart> CartRepository { get; }
        IOrderRepository OrderRepository { get; }
        Task<bool> SaveAsync();
    }
}
