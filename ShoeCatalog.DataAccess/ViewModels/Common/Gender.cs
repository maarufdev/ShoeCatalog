using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.DataModels.ViewModels.Common
{
    public enum Gender
    {
        [Display(Name = "Male")]
        Male = 1,

        [Display(Name = "Female")]
        Female = 2,

        [Display(Name = "UniSex")]
        UniSex = 3
    }
}
