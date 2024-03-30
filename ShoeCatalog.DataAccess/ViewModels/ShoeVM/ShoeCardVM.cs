using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.DataModels.ViewModels.ShoeVM
{
    public class ShoeCardVM
    {
        public string? ShoeId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public string? Brand { get; set; }
        public List<string> Categories { get; set; } = new List<string>();

    }
}
