using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedManager.Models
{
    public class Clinic
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Clinic Name")]
        public string ClinicName { get; set; }
        [Required]
        [DisplayName("Clinic Address")]
        public string ClinicAddress { get; set; }
        [Required]
        [DisplayName("Clinic Phone")]
        public int ClinicPhone { get; set; }

        public string ClinicDescription { get; set; }

        public List<Rating> Ratings { get; set; }

        [NotMapped]
        [DisplayName("Average Rating Score")]
        public decimal AverageRating { get; set; } = 0;
    }
}