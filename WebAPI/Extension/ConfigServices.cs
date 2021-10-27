using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Model;
using WebAPI.Repository;
using WebAPI.Repository.Interface;
using WebAPI.RepositoryService.Interface;
using WebAPI.RepositoryService.Service;
using WebAPI.UnitOfWorks;
using WebAPI.UploadImageUtils;
using WebAPI.Utils;

namespace WebAPI.Extension
{
    public static class ConfigServices
    {
        public static void ConfigureMSSQLContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<PTStoreContext>(options =>
                options.UseSqlServer(config.GetConnectionString("PTStoreDatabase")));
        }

        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped<IJwtUtils, JwtUtils>();

            services.AddScoped<PTStoreContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBrandService, BrandService>();


            services.AddScoped<IUploadImage, UploadImageWithCloudinary>();
        }
    }
}
