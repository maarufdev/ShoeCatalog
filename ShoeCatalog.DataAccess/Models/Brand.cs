using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.DataModels.Models
{
    public class Brand
    {

        [Key]
        public string? Id { get; set; }
        [Required] 
        public string? Name { get; set; }

        [Required]
        public List<Shoe>? Shoes { get; set; }

    }
}
