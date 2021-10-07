using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.RepositoryService.Interface;
using WebAPI.RepositoryService.Service;
using WebAPI.Utils;

namespace WebAPI.Extension
{
    public static class ConfigServices
    {
        //public static void ConfigureMSSQLContext(this IServiceCollection services, IConfiguration config)
        //{
        //    services.AddDbContext<ExerciseContext>(options =>
        //        options.UseSqlServer(config.GetConnectionString("ExerciseDatabase")));
        //}

        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtUtils, JwtUtils>();
        }
    }
}
