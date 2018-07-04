using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.Models
{
    public class Party
    {
        [Key] public int PartyId { get; set; }

        [Required(ErrorMessage = "Please enter party name")]
        [DisplayName("Name")]
        public string PartyName { get; set; }

        [Required(ErrorMessage = "Please enter party code")]
        [DisplayName("Code")]
        public string PartyCode { get; set; }

        [Required(ErrorMessage = "Please enter party contact")]
        [DisplayName("Contact No.")]
        [RegularExpression(@"\+?(88)?0?1[56789][0-9]{8}\b", ErrorMessage = "Please enter valid contact no(+88) or (01)")]
        public string PartyContactNo { get; set; }

        [Required(ErrorMessage = "Please enter party email address")]
        [DisplayName("Eamil")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Please enter a valid email")]
        public string PartyEmail { get; set; }

        [Required(ErrorMessage = "Please enter party address")]
        [DisplayName("Address")]
        public string PartyAddress { get; set; }
        [Required(ErrorMessage = "Please select an option")]
        [DisplayName(displayName: "Party Type")]
        public int PartyTypeId { get; set; }
        public PartyType PartyType { get; set; }
    }
}