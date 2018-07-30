using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.Models
{
    public class UserLogin
    {
        [Key]
        public int UserLoginId { get; set; }

        public int EmployeeId { get; set; }
        public int UserTypeId { get; set; }
        public Employee Employee { get; set; }
        public UserType UserType { get; set; }
    }
}