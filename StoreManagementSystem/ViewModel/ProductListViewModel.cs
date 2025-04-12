namespace StoreManagementSystem.ViewModel
{
    public class ProductListViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string StockStatus { get; set; }
        public bool IsActive { get; set; }
    }
}
