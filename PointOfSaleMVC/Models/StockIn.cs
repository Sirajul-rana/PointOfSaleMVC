﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.Models
{
    public class StockIn
    {
        [Key] public int StockInId { get; set; }

        public int ItemId { get; set; }
        [Required(ErrorMessage = "Please select an option")]
        [DisplayName("Branch")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [DisplayName("Supplier")]
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [DisplayName("Employee")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please select a date")]
        [DataType(DataType.Date)]
        [DisplayName("Date")]
        public DateTime PurchaseDateTime { get; set; }

        public decimal PurchaseTotal { get; set; }
        public PurchaseTransaction PurchaseTransaction { get; set; }
        public Branch Branch { get; set; }
        public Party Party { get; set; }
        public Employee Employee { get; set; }
        public Item Item { get; set; }
        public List<Item> Items { get; set; }
    }
}