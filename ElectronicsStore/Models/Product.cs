
namespace ElectronicsStore.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public string ImageUrl { get; set; }

		
		public int? CategoryId { get; set; } //Foreign key property And ALSO THE ? chcecks if its null so if any entiries alredy exist in the database it will not cause an error and just enter it as null
		public virtual Category Category { get; set; }
	}
}