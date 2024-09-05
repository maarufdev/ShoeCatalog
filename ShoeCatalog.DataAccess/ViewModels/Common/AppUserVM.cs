using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Domain.ViewModels.Common
{
    public class AppUserVM
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }

        public DeliveryAddressVM? DeliveryAddress { get; set; } = new DeliveryAddressVM();
    }
}
