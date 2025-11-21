using Microsoft.AspNetCore.Identity;
using WebAppEmployeeApi.Domain.Models;
using WebAppEmployeeApi.Domain.Services;
using WebAppEmployeeApi.Services.JwtToken;
using WebAppEmployeeApi.Services.RepServices;

namespace WebAppEmployeeApi.StartUpDI
{
    public static class AppServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<JwtTokenCreator>();
            services.AddSingleton<EmpCacheService>();
            services.AddSingleton<AddressApiClient>();
            services.AddHttpContextAccessor();
            //services.AddScoped<EmpCacheService>();

            services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();
            return services;
        }
    }
}
