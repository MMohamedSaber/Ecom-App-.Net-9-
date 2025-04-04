
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Ecom.infrastructure
{
    public static class infrastructureRegisteration
    {

        public static IServiceCollection infrastructureConfiguration( this IServiceCollection services,IConfiguration configuration)
        {

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IImageManagementService, ImageManagementService>();

            
            services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
                    );
            services.AddDbContext<AppDbContext>(options =>
                                  options.UseSqlServer
                                  (configuration.GetConnectionString("Default")));

            return services;

        }
    }
}
