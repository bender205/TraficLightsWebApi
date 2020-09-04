using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using TraficLightsRazorPages.Models.Interfaces;

namespace TraficLightsRazorPages.Models
{
    
    public class TrafficLightEntity : ITrafficLight
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Color")]
        [MaxLength(10)]
        public Colors Color { get; set; } = Colors.Red;
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="0:yyyy\\MM\\dd HH:mm")]
        public DateTime? Date { get; set; } = DateTime.UtcNow;
      
    }
}
