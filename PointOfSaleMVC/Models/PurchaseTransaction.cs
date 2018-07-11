using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.Models
{
    public class PurchaseTransaction
    {
        [Key]
        public int PurchaseTransactionId { get; set; }

        [Required(ErrorMessage = "Please enter total")]
        [DisplayName("Total")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Please enter paid amount")]
        [DisplayName("Paid Amount")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Paid { get; set; }

        [Required(ErrorMessage = "Please enter Return")]
        [DisplayName("Return")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Return { get; set; }
    }
}