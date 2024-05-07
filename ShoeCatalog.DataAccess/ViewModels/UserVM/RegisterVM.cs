using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Domain.ViewModels.UserVM
{
    public class RegisterVM
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow;
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
