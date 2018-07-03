using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.Models
{
    public class ExpenseCategory
    {
        [Key]
        public int ExpenseCategoryId { get; set; }
        [Required(ErrorMessage = "Please enter category name")]
        [DisplayName("Name")]
        public string ExpenseName { get; set; }
        [Required(ErrorMessage = "Please enter category code")]
        [DisplayName("Code")]
        public string ExpenseCode { get; set; }
        [Required(ErrorMessage = "Please enter category description")]
        [DisplayName("Description")]
        public string ExpenseDescription { get; set; }
        [Required(ErrorMessage = "Please select an option")]
        [DisplayName("Parent Category")]
        public int? RootExpenseCategoryId { get; set; }
        public ExpenseCategory RootExpenseCategory { get; set; }
    }
}