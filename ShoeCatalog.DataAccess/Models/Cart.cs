using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Domain.Models
{
    public class Cart
    {
        [Key]
        public string? Id { get; set; }

        public string? ProductId { get; set; }
        public int Quantity { get; set; }
        public string? OrderedBy { get; set; }
        public DateTime OrderedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
