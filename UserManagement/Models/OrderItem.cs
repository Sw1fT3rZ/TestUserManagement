namespace UserManagementApp.Models
{
	public class OrderItem
	{
		public int Id { get; set; } // ������� ����������� (PK � ��)
		public int OrderId { get; set; } // (FK) ����� � ��������� "Orders"
		public Order Order { get; set; } //
		public int ProductId { get; set; } // (FK) ����� � ��������� "Product"
		public Product Product { get; set; } // 
		public int Quantity { get; set; } // ����� ����������
	}
}
