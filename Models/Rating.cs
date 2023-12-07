using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedManager.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }


        [Required]
        [Range(0,10)]
        public int Rate { get; set; }
        public string Comment { get; set; }
    }
}