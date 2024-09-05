using ShoeCatalog.Domain.ViewModels.UserVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> Login(LoginVM login);
        Task<bool> Register(RegisterVM register);
        Task SignOut();
    }
}
