namespace UserManagementApp.Models
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal Price { get; set; } //тут decimal чтоб сохранять цены типа 17.99
        public int AvailableQuantity { get; set; } 
    }
}
