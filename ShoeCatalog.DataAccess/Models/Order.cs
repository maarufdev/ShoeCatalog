using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using ShoeCatalog.DataModels.Models;
using ShoeCatalog.Domain.ViewModels.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ShoeCatalog.Domain.Models
{
    public class Order
    {
        [Key]
        public Guid? Id { get; set; }
        
        [Required]
        public string? ShoeId { get; set; }
        
        [ForeignKey("ShoeId")]
        [ValidateNever]
        public Shoe? Shoe { get; set; }

        [Required]
        public string? AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        [ValidateNever]
        public AppUser? AppUser { get; set; }

        [Required]
        [Range(1, 1_000_000)]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        
        [Required]
        public int Quantity { get; set; }

        [Required]
        [Range(1, 1_000_000)]
        [Precision(18, 2)]
        public decimal Total { get; set; }

        [Required]
        public Guid? TrackingNumber { get; set; }
        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string? StreetAddress { get; set; }
        
        [Required]
        public string? Building { get; set; }
        
        [AllowNull]
        public string? District { get; set; } = string.Empty;

        [AllowNull]
        public string? PostalCode { get; set; }
        
        [Required]
        public string? City { get; set; }
        
        [Required]
        public string? State { get; set; }
        
        [AllowNull]
        public string? Country { get; set; }
        
        [AllowNull]
        public string? CountryCode { get; set; }
        
        [Required(ErrorMessage = "Contact Number is required.")]
        public string? ContactNumber { get; set; }

        [AllowNull]
        public string? Carrier { get; set; } = string.Empty;

        [Required]
        public OrderStatus OrderStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}