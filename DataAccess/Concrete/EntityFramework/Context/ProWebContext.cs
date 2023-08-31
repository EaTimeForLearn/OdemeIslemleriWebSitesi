using Core.Entities.Concrete;
using Core.Utilities.IoC;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.Context
{
    public class ProWebContext : DbContext
    {
        IHttpContextAccessor _httpContextAccessor;

        public ProWebContext()
        {
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            var DBName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DBName")?.Value ?? "PROWEB30";
            optionsBuilder.UseSqlServer(connectionString: String.Format(String.Format(configuration.GetConnectionString("WebApiDatabase"), DBName), DBName));
        }

        public DbSet<Musteri> Musteriler { get; set; }
    }
}

