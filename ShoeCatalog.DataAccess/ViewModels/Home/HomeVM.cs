using ShoeCatalog.DataModels.ViewModels.ShoeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.DataModels.ViewModels.Home
{
    public class HomeVM
    {
        public List<ShoeCardVM> Cards { get; set; } = new List<ShoeCardVM>();
        public List<ShoeCardVM> Carousels { get; set; } = new List<ShoeCardVM>();
    }
}
