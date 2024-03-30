using ShoeCatalog.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Repositories.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        //IGenericRepository<Shoe> Shoe { get; }
        IShoeRepository Shoe { get; }
        IGenericRepository<Category> Category { get; }
        IGenericRepository<Brand> Brand { get; }
        IGenericRepository<ShoeCategory> ShoeCategory { get; }

        Task<bool> SaveAsync();
    }
}
