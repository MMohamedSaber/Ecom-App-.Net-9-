
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecom.infrastructure
{
    public static class infrastructureRegisteration
    {

        public static IServiceCollection infrastructureConfiguration( this IServiceCollection services,IConfiguration configuration)
        {

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<AppDbContext>(options => 
                                  options.UseSqlServer
                                  (configuration.GetConnectionString("Default")));
            return services;

        }
    }
}
