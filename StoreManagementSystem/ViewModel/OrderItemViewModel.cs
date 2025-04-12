using System.ComponentModel.DataAnnotations;

namespace StoreManagementSystem.ViewModel
{
    public class OrderItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; } = 1;

        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100")]
        public decimal Discount { get; set; }

        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal LineTotal => (UnitPrice * Quantity) * (1 - Discount / 100);
    }
}
