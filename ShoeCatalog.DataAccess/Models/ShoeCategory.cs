using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeCatalog.DataModels.Models
{
    public class ShoeCategory
    {
        [Required]
        public string? ShoeId { get; set; }
        [ForeignKey(nameof(ShoeId))]
        [ValidateNever]
        public Shoe? Shoe { get; set; }

        [Required]
        public string? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [ValidateNever]
        public Category? Category { get; set; }
    }
}