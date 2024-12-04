namespace UserManagementApp.Models
{
    public class Order
    {
        public int Id { get; set; } // Айди заказа
        public int Quantity { get; set; } // Количество продукта
        public decimal Price { get; set; } //Цена заказа
        public int UserId { get; set; } // Айдишник пользователя, сделавшего заказ
        public User User { get; set; } // Пользователь, который сделал заказ
        public int AddressId { get; set; } 
        public Address Address { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }//коллекция чтоб сохранить список товаров в заказе
        public Payment Payment { get; set; } //Инфа об оплате, первый payment это объект, второй это название
    }
}