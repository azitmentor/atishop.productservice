using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Atishop.Dataprovider.Model
{
	public class MainDbContext : DbContext
	{
		public DbSet<t_product> t_product { get; set; }
		public DbSet<t_customer> t_customer { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder.UseNpgsql("Host=172.17.0.1;Database=mydb");

		public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
		{

		}
	}
}
