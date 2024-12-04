namespace UserManagementApp.Models
{
	public class OrderItem
	{
		public int Id { get; set; } // Айдишка Ордерайтема (PK в бд)
		public int OrderId { get; set; } // (FK) Связь с табличкой "Orders"
		public Order Order { get; set; } //
		public int ProductId { get; set; } // (FK) Связь с табличкой "Product"
		public Product Product { get; set; } // 
		public int Quantity { get; set; } // Задаём количество
	}
}
