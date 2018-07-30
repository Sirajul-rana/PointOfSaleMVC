using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.Models
{
    public class StockOut
    {
        [Key]
        public int StockOutId { get; set; }
        [Required(ErrorMessage = "Please select an option")]
        [DisplayName("Stock Qty")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [DisplayName("Branch")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [DisplayName("Employee")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please select a date")]
        [DisplayName("Date")]
        public DateTime SaleDate { get; set; }
        public Item Item { get; set; }
        public List<Item> Items { get; set; }
        public SalesTransaction SalesTransaction { get; set; }
        
    }
}