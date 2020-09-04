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

        private bool _switchingDown = true;
        public bool IsSwitchingDown
        {
            get => this._switchingDown;
            set => this._switchingDown = value;
        }
        public DateTime? Date { get; set; } = DateTime.UtcNow;
        public Timer ColorSwitchTimer { get; set; }
        public TrafficLight()
        {
        }

    }
}
