using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using _10Model;
using _10Model.Customer;

namespace _20DbLayer
{
    public class ApplicationContext : DbContext
    {
        public DbSet<TempData> TempDatas { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<PrivatePerson> PrivatePeople { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnConfiguring (DbContextOptionsBuilder options)
        {
            options.UseSqlite("DataSource=MyReportDb.db");
            options.UseLazyLoadingProxies();
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Person>().ToTable("People");
            model.Entity<PrivatePerson>().ToTable("PrivatePersons");
            model.Entity<Director>().ToTable("Directors");

            model.Entity<Contract>()
               .Property(e => e.Target)
               .HasConversion(v => v.ToString(),
               v => (TargetType)Enum.Parse(typeof(TargetType), v));
            model.Entity<Report>();
        }
    }
}
