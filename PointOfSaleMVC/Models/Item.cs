using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        [Required(ErrorMessage = "Please select an option")]
        [DisplayName(displayName:"Category")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        [DisplayName(displayName: "Name")]
        public string ItemName { get; set; }
        [Required(ErrorMessage = "Please enter code")]
        [DisplayName(displayName: "Code")]
        public string ItemCode { get; set; }
        [Required(ErrorMessage = "Please enter description")]
        [DisplayName(displayName: "Decription")]
        public string ItemDescription { get; set; }
        [Required(ErrorMessage = "Please enter purchase price")]
        [RegularExpression(@"(\d+[\/\d. ]*|\d)", ErrorMessage = "Enter a valid price")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [DisplayName(displayName: "Purchase Price")]
        public decimal CostPrice { get; set; }
        [Required(ErrorMessage = "Please enter sale price")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [DisplayName(displayName: "Sale Price")]
        public decimal SalePrice { get; set; }

        [Required(ErrorMessage = "Please enter a quantity")]
        [DisplayName("Qty")]
        public int Quantity { get; set; }
        public Category Category { get; set; }
        //public string ImagePath { get; set; }

    }
}