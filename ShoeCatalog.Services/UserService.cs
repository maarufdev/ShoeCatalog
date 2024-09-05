using Microsoft.AspNetCore.Identity;
using ShoeCatalog.Domain.Models;
using ShoeCatalog.Domain.ViewModels.UserVM;
using ShoeCatalog.Services.Interfaces;

namespace ShoeCatalog.Services
{
    public class UserService : IUserService
    {
        private protected readonly UserManager<AppUser> _userManager;
        private protected readonly SignInManager<AppUser> _signInManager;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> Login(LoginVM login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email!);

            bool isSignInSuccess = false;

            if (user is not null) 
            { 
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, login.Password!);

                if (isPasswordValid)
                {
                    var checkSigningIn = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);
                    isSignInSuccess = checkSigningIn.Succeeded;
                }
                else
                {
                    isSignInSuccess = isPasswordValid;
                }
            }

            return isSignInSuccess;
        }

        public async Task<bool> Register(RegisterVM register)
        {
            bool isRegistrationSuccess = false;

            var registerUser = new AppUser
            {
                FirstName = register.FirstName,
                MiddleName = register.MiddleName,
                LastName = register.LastName,
                Email = register.Email,
                UserName = register.Email,
                DateOfBirth = register.DateOfBirth,
                CreatedOn = DateTime.UtcNow
            };

            if(await _userManager.FindByEmailAsync(register.Email!) == null)
            {
                var newUser = await _userManager.CreateAsync(registerUser, register.Password!);
                var registeredUserRole = await _userManager.AddToRoleAsync(registerUser, AppUserRoles.User.ToString());

                isRegistrationSuccess = (newUser.Succeeded && registeredUserRole.Succeeded);
                
            }
            else
            {
                isRegistrationSuccess =  false;
            }

            return isRegistrationSuccess;
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
