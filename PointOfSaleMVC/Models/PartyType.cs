using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.Models
{
    public class PartyType
    {
        [Key] public int PartyTypeId { get; set; }
        public string Type { get; set; }
    }
}