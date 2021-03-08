using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Web.DatabaseModel
{
    public class WebDataContext : DbContext
    {
        public WebDataContext(DbContextOptions<WebDataContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
