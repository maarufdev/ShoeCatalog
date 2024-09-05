using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Domain.ViewModels.Cart
{
    public class CartUpsertVM
    {
        public string? Id { get; set; }
        public string? ShoeId { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public bool IsActive { get; set; }
    }
}
