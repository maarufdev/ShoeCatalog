using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.DataModels.Models
{
    public class Category
    {
        public Category()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public List<ShoeCategory>? ShoeCategories { get; set; }
    }
}


