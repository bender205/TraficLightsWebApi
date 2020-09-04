using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TrafficLights.Models;

namespace TrafficLights.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
}

        public DbSet<TrafficLightEntity> TrafficLights { get; set; }
    }
}
