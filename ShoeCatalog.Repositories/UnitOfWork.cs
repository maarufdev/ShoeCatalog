using ShoeCatalog.DataModels.Data;
using ShoeCatalog.DataModels.Models;
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
            //Shoe = new GenericRepository<Shoe>(_context);
            //Category = new GenericRepository<Category>(_context);
            //Brand = new GenericRepository<Brand>(_context);
            //ShoeCategory = new GenericRepository<ShoeCategory>(_context);
        }

        //public IGenericRepository<Shoe> Shoe { get; private set; }
        public IShoeRepository Shoe => new ShoeRepository(_context);
        //public IGenericRepository<Category> Category { get; private set; }
        public IGenericRepository<Category> Category => new GenericRepository<Category>(_context);
        //public IGenericRepository<Brand> Brand { get; private set; }
        public IGenericRepository<Brand> Brand => new GenericRepository<Brand>(_context);
        //public IGenericRepository<ShoeCategory> ShoeCategory { get; private set; }
        public IGenericRepository<ShoeCategory> ShoeCategory => new GenericRepository<ShoeCategory>(_context);

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
