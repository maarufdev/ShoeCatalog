using Microsoft.AspNetCore.Mvc.Rendering;
using ShoeCatalog.DataModels.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShoeCatalog.DataModels.ViewModels.ShoeVM
{
    public class ShoeUpsertVM
    {
        public ShoeUpsertVM()
        {
            Shoe = new Shoe();
        }

        public Shoe Shoe { get; set; }
        public IEnumerable<SelectListItem>? BrandList { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        public IEnumerable<SelectListItem>? GenderList { get; set; }

        [Required(ErrorMessage = "Shoe categories is required, please select at least one.")]
        [DisplayName("Categories")]
        public string[]? SelectedCategory { get; set; }

    }
}
