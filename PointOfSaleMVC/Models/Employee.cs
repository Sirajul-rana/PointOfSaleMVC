using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.Models
{
    public class Employee
    {
        [Key] public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please enter your name")]
        [DisplayName("Name")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Please enter your father's name")]
        [DisplayName("Father's Name")]
        public string EmployeeFatherName { get; set; }

        [Required(ErrorMessage = "Please enter your Mother's name")]
        [DisplayName("Mother's Name")]
        public string EmployeeMotherName { get; set; }

        [Required(ErrorMessage = "Please enter code")]
        [DisplayName("Code")]
        public string EmployeeCode { get; set; }

        [Required(ErrorMessage = "Please select a date")]
        [DisplayName("Join Date")]
        public DateTime EmployeeJoinDate { get; set; }
        //public string EmployeeImage { get; set; }

        [Required(ErrorMessage = "Please enter your contact no.")]
        [DisplayName("Contact No.")]
        [RegularExpression(@"\+?(88)?0?1[56789][0-9]{8}\b", ErrorMessage = "Please enter valid contact no(+88) or (01)")]
        public string EmployeeContactNo { get; set; }

        [Required(ErrorMessage = "Please enter your emergency contact")]
        [DisplayName("Emergency Contact No.")]
        [RegularExpression(@"\+?(88)?0?1[56789][0-9]{8}\b", ErrorMessage = "Please enter valid contact no(+88) or (01)")]
        public string EmployeeEmergencyContactNo { get; set; }

        [Required(ErrorMessage = "Please enter your national id")]
        [DisplayName("National Id")]
        public string EmployeeNId { get; set; }

        [Required(ErrorMessage = "Please enter your username")]
        [DisplayName("Username")]
        public string EmployeeUsername { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DisplayName("Password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage = "Minimum eight characters, at least one letter, one number and one special character")]
        public string EmployeePassword { get; set; }

        [Required(ErrorMessage = "Please enter your password again")]
        [DisplayName("Confirm Password")]
        [Compare("EmployeePassword",ErrorMessage = "Password do not match")]
        public string EmployeeConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [DisplayName("Eamil")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Please enter a valid email")]
        public string EmployeeEmail { get; set; }

        [Required(ErrorMessage = "Please enter your present address")]
        [DisplayName("Present Address")]
        public string EmployeePresentAddress { get; set; }

        [Required(ErrorMessage = "Please enter your Permanent address")]
        [DisplayName("Permanent Address")]
        public string EmployeePermanentAddress { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [DisplayName(displayName: "Branch")]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}