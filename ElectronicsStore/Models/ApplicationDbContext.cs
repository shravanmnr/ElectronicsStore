using System.Data.Entity;

namespace ElectronicsStore.Models
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext() : base("DefaultConnection")
		{

		} 

		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }	
	}
}