using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using ShoeCatalog.DataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Domain.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        
        public string? ShoeId { get; set; }
        
        [ForeignKey("ShoeId")]
        [ValidateNever]
        public Shoe? Shoe { get; set; }
        
        [Range(1, 1000, ErrorMessage = "Please enter value between 1 and 1000")]
        public int Quantity { get; set; }
        
        [Precision(18, 2)]
        [Range(50, 1_000_000)]
        public double Price { get; set; }
        
        [Range(1, 1_000_000)]
        public double Total { get; set; }
        public string? AppUserId { get; set; }
        
        [ForeignKey("AppUserId")]
        [NotMapped]
        public AppUser? AppUser { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
