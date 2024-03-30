using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using ShoeCatalog.DataModels.ViewModels.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeCatalog.DataModels.Models
{
    public class Shoe
    {
        public Shoe()
        {
            Gender = Gender.UniSex;
            DateModified = DateTime.Now;
        }

        [Key]
        public string? Id { get; set; }
        
        [Required(ErrorMessage = "Shoe item is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Brand of shoe is required.")]
        public string? BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        [ValidateNever]
        public Brand? Brand { get; set; }

        //[Required(ErrorMessage = "Category is required.")]
        //public string? CategoryId { get; set; }
        //[ForeignKey(nameof(CategoryId))]
        //[ValidateNever]
        //public Category? Category { get; set; }


        [Required(ErrorMessage = "Shoe short description is required.")]
        [DisplayName("Short Description")]
        public string? ShortDescription { get; set; }

        [Required(ErrorMessage = "Shoe full description is required.")]
        [DisplayName("Full Description")]
        public string? FullDescription { get; set; }
        public string? ImageFileName { get; set; }

        [Required(ErrorMessage = "Shoe color is required.")]
        [DisplayName("Color")]
        public string? Color { get; set; }

        [Required(ErrorMessage = "Shoe size is required.")]
        [DisplayName("Size")]
        [Precision(18, 2)]
        [Range(7, 20)]
        public decimal Size { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Shoe's price is required. ")]
        [Precision(18, 2)]
        [Range(100, 1000000)]
        public decimal Price { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; } 

        public bool IsActive { get; set; }
        public List<ShoeCategory>? ShoeCategories { get; set; }

    }
}
