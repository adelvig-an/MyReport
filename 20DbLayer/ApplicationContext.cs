﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using _10Model;

namespace _20DbLayer
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Report> Reports { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        protected override void OnConfiguring (DbContextOptionsBuilder options)
        {
            options.UseSqlite("DataSource=MyReportDb.db");
            options.UseLazyLoadingProxies();
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Contract>()
               .Property(e => e.Target)
               .HasConversion(v => v.ToString(),
               v => (TargetType)Enum.Parse(typeof(TargetType), v));
            model.Entity<Report>();
        }
    }
}
