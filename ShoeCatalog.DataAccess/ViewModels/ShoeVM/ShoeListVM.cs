using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.DataModels.ViewModels.ShoeVM
{
    public class ShoeListVM
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }

    }
}
