using ShoeCatalog.DataModels.Data;
using ShoeCatalog.Domain.Models;

namespace ShoeCatalog.Web.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            //services.AddDefaultIdentity<AppUser>


            return services;
        }
    }
}
