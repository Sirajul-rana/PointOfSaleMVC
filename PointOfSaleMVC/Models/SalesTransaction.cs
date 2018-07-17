using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.Models
{
    public class SalesTransaction
    {
        [Key]
        public int SalesTransactionId { get; set; }

        [DisplayName(displayName: "Sub Total")]
        public decimal SubTotal { get; set; }

        [Required(ErrorMessage = "Please enter vat")]
        [DisplayName(displayName: "Vat")]
        public decimal Vat { get; set; }

        [Required(ErrorMessage = "Please enter discount")]
        [DisplayName(displayName: "Discount")]
        public float Discount { get; set; }

        [Required(ErrorMessage = "Please enter total price")]
        [DisplayName(displayName: "Total")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Please enter pain amount")]
        [DisplayName(displayName: "Pain Amount")]
        public decimal PaidAmount { get; set; }

        [Required(ErrorMessage = "Please enter return amount")]
        [DisplayName(displayName: "Return Amount")]
        public decimal ReturnAmount { get; set; }
    }
}