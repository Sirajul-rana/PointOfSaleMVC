using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.Models
{
    public class Branch
    {
        [Key] public int BranchId { get; set; }

        [Required(ErrorMessage = "Please enter organization name")]
        [DisplayName("Name")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Please enter organization code")]
        [DisplayName("Code")]
        public string BranchCode { get; set; }

        [Required(ErrorMessage = "Please enter organization contact")]
        [DisplayName("Contact No.")]
        [RegularExpression(@"\+?(88)?0?1[56789][0-9]{8}\b", ErrorMessage = "Please enter valid contact no(+88) or (01)")]
        public string BranchContactNo { get; set; }

        [Required(ErrorMessage = "Please enter organization address")]
        [DisplayName("Address")]
        public string BranchAddress { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [DisplayName("Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}