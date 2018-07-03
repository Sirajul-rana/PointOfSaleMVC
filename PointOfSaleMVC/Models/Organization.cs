using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.Models
{
    public class Organization
    {
        [Key]
        public int OrganizationId { get; set; }
        [Required(ErrorMessage = "Please enter organization name")]
        [DisplayName("Name")]
        public string OrganizationName { get; set; }
        [Required(ErrorMessage = "Please enter organization code")]
        [DisplayName("Code")]
        public string OrganizationCode { get; set; }
        [Required(ErrorMessage = "Please enter organization contact")]
        [DisplayName("Contact No.")]
        [RegularExpression(@"\+?(88)?0?1[56789][0-9]{8}\b", ErrorMessage = "Please enter valid contact no(+88) or (01)")]
        public string OrganizationContactNo { get; set; }
        //public string OrganizationImage { get; set; }
        [Required(ErrorMessage = "Please enter organization address")]
        [DisplayName("Address")]
        public string OrganizationAddress { get; set; }
    }
}