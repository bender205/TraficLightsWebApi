using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TraficLightsRazorPages.Models.Interfaces;

namespace TraficLightsRazorPages.Models
{
    public class TrafficLight : ITrafficLight
    {
        public int Id { get; set; } = 0;
        public Colors Color { get; set; } = Colors.Red;
        public bool IsSwitchingDown { get; set; } = true;
        public DateTime? Date { get; set; } = DateTime.UtcNow;
        public TrafficLight()
        {
        }

    }
}
