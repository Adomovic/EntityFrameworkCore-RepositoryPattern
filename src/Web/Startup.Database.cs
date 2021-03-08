using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.DatabaseModel;

namespace Web
{
    public partial class Startup
    {
        void AddDataSources(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<WebDataContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("WebDataConnectionString")));
        }
    }
}
