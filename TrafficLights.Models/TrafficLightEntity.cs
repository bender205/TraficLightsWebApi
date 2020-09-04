using System;

namespace TrafficLights.Models
{
    public class TrafficLightEntity
    {
        public int Id { get; set; }
        public string Color { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
