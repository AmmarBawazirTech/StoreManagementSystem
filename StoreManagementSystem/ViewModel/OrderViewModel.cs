using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace StoreManagementSystem.ViewModel
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Employee is required")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Display(Name = "Order Items")]
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();

        // For dropdown lists
        public SelectList? CustomerSelectList { get; set; }
        public SelectList? EmployeeSelectList { get; set; }
        public SelectList? ProductSelectList { get; set; }
    }
}
