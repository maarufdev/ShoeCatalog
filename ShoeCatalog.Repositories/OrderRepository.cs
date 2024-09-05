using ShoeCatalog.DataModels.Data;
using ShoeCatalog.Domain.Models;
using ShoeCatalog.Domain.ViewModels.Order;
using ShoeCatalog.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ShoeDbContext _context;
        public OrderRepository(ShoeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddOrder(OrderVM orderVM)
        {

            await Task.CompletedTask;
        }
    }
}
