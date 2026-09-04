using System;
namespace CoffeeStoreApi.Models
{
	public class Category
	{
		public int Id { set; get; }

		public string Name { set; get; } = string.Empty;

        public string? ImageUrl { get; set; }

        public ICollection<Product> Products { get; set; }
                = new List<Product>();
    }

}


