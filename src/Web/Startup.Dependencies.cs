using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.DatabaseModel.Repository;

namespace Web
{
    public partial class Startup
    {
        void AddDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ProductRepository>();
            services.AddScoped<CategoryRepository>();
        }
    }
}
