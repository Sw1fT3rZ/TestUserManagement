namespace UserManagementApp.Models
{
    public class Order
    {
        public int Id { get; set; } // Айди заказа
        public string ProductName { get; set; } // Название продукта
        public int Quantity { get; set; } // Количество продукта
        public decimal Price { get; set; } //Цена заказа
        public int UserId { get; set; } // Айдишник пользователя, сделавшего заказ
        public User User { get; set; } // Пользователь, который сделал заказ
    }
}