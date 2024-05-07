using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Domain.Models
{
    public class AppUser : IdentityUser
    {

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? MiddleName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? ImageProfile { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
