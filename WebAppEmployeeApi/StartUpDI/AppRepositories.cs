using WebAppEmployeeApi.Data.Repositories;
using WebAppEmployeeApi.Domain.Repositories;

namespace WebAppEmployeeApi.StartUpDI
{
    public static class AppRepositories
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            return services;
        }
    }
}
