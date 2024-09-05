using ShoeCatalog.DataModels.Data;
using ShoeCatalog.DataModels.Models;
using ShoeCatalog.Domain.Models;
using ShoeCatalog.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShoeDbContext _context;

        public UnitOfWork(ShoeDbContext context)
        {
            _context = context;
        }
        public IShoeRepository ShoeRepository => new ShoeRepository(_context);
        public IGenericRepository<Category> CategoryRepository => new GenericRepository<Category>(_context);
        public IGenericRepository<Brand> BrandRepository => new GenericRepository<Brand>(_context);
        public IGenericRepository<ShoeCategory> ShoeCategory => new GenericRepository<ShoeCategory>(_context);
        public IGenericRepository<Cart> CartRepository => new GenericRepository<Cart>(_context);
        public IOrderRepository OrderRepository => new OrderRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<bool> SaveAsync()
        {
            var result = await _context.SaveChangesAsync() > 0;

            if (result)
            {
                this.Dispose();
            }

            return result;
        }
    }
}
