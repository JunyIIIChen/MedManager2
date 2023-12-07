using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedManager.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        public int ClinicId { get; set; }

        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required] 
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }


    }
}