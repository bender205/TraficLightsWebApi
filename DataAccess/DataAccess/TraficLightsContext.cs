﻿
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TraficLightsRazorPages.Models;

namespace DataAccess.Context
{
    public class TraficLightsContext : DbContext
    {
        public TraficLightsContext(DbContextOptions<TraficLightsContext> options) : base(options)
        {
        }
        public DbSet<TrafficLightEntity> Lights { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrafficLightEntity>().ToTable("TraficLights");            
        }
    }

}
