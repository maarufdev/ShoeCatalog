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
    public sealed class ShoeRepository : GenericRepository<Shoe>, IShoeRepository
    {
        private readonly ShoeDbContext _context;
        public ShoeRepository(ShoeDbContext context):base(context)
        {
            _context = context;
        }

        public async Task AddShoeAsync(Shoe shoe)
        {
            await _context.AddAsync(shoe);
        }
    }
}
