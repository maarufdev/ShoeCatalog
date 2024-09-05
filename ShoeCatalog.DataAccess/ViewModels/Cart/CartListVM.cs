using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Domain.ViewModels.Cart
{
    public class CartListVM
    {
        public Guid Id { get; set; }
        public string? ShoeId { get; set; }
        public string? Name { get; set; }
        public string? ImageFileName { get; set; }
        public string? Brand { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public bool IsActive { get; set; }
    }
}
