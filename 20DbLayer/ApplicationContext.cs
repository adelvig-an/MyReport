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
        public DbSet<AppraiserOrganization> AppraiserOrganizations { get; set; }
        public DbSet<SelfRegulatingOrganization> SRO { get; set; }
        public DbSet<Appraiser> Appraisers { get; set; }
        
        public DbSet<InsurancePolicie> InsurancePolicies { get; set; }
        public DbSet<QualificationCertificate> QualificationCertificates { get; set; }

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
            model.Entity<Organization>().ToTable("Organizations");
            model.Entity<AppraiserOrganization>().ToTable("AppraiserOrganizations");
            model.Entity<Director>().ToTable("Directors");
            model.Entity<Appraiser>().ToTable("Appraisers");

            model.Entity<Contract>()
               .Property(e => e.Target)
               .HasConversion(v => v.ToString(),
               v => (TargetType)Enum.Parse(typeof(TargetType), v));
            model.Entity<Report>();
            model.Entity<QualificationCertificate>()
                .Property(e => e.Speciality)
                .HasConversion(v => v.ToString(),
                v => (SpecialityType)Enum.Parse(typeof(SpecialityType), v));
        }
    }
}
