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
            //Закомментировать когда БД не удаляется
            model.Entity<SelfRegulatingOrganization>().HasData(
                new {
                    Id = 1,
                    NameFull = "АССОЦИАЦИЯ \"РУССКОЕ ОБЩЕСТВО ОЦЕНЩИКОВ\"",
                    NameShort = "АССОЦИАЦИЯ РОО",
                    NumberRegistration = 0003,
                    Telephone = "+7 (495) 662—74-25",
                    Email = "info@sroroo.ru",
                    Site = "www.sroroo.ru",
                    AddressRegistration = "105066, г. Москва, пер. 1-й Басманный, д. 2А",
                    AddressActual = "105066, Москва, 1-й Басманный переулок, д.2А, офис 5"
                },
                new
                {
                    Id = 2,
                    NameFull = "АССОЦИАЦИЯ \"САМОРЕГУЛИРУЕМАЯ ОРГАНИЗАЦИЯ ОЦЕНЩИКОВ \"ЭКСПЕРТНЫЙ СОВЕТ\"",
                    NameShort = "АССОЦИАЦИЯ СРОО \"ЭКСПЕРТНЫЙ СОВЕТ\"",
                    NumberRegistration = 0011,
                    Telephone = "8 800 200-29-50",
                    Email = "mail@srosovet.ru",
                    Site = "www.srosovet.ru",
                    AddressRegistration = "101000, г. Москва, МО Басманный, пер. Потаповский, д. 16/5, стр. 1, мансарда 3, кабинет 2, 3, 4, 5, 7, 14",
                    AddressActual = "101000, г. Москва, Потаповский пер., д. 16/5, стр. 1"
                },
                new
                {
                    Id = 3,
                    NameFull = "АССОЦИАЦИЯ САМОРЕГУЛИРУЕМАЯ ОРГАНИЗАЦИЯ ОЦЕНЩИКОВ \"СОЮЗ\"",
                    NameShort = "СРО \"СОЮЗ\"",
                    NumberRegistration = 0004,
                    Telephone = "8 800 511-25-78",
                    Email = "srosoyz@srosoyz.ru",
                    Site = "www.srosoyz.ru",
                    AddressRegistration = "101000, г. Москва, ул. Покровка, д. 33, помещ. 10",
                    AddressActual = "101000, г. Москва, ул. Покровка, д. 33, помещ. 10"
                },
                new
                {
                    Id = 4,
                    NameFull = "АССОЦИАЦИЯ \"МЕЖРЕГИОНАЛЬНЫЙ СОЮЗ ОЦЕНЩИКОВ\"",
                    NameShort = "АССОЦИАЦИЯ \"МСО\"",
                    NumberRegistration = 0005,
                    Telephone = "+7 (863) 299-42-29",
                    Email = "sro-mso@mail.ru",
                    Site = "www.sromso.ru",
                    AddressRegistration = "344022, Ростовская область, г. Ростов-На-Дону, ул. Максима Горького, д. 245/26, кв. 606",
                    AddressActual = "344022, ул. Максима Горького, д.245/26, этаж 6, офис 606"
                },
                new
                {
                    Id = 5,
                    NameFull = "АССОЦИАЦИЯ САМОРЕГУЛИРУЕМАЯ ОРГАНИЗАЦИЯ \"НАЦИОНАЛЬНАЯ КОЛЛЕГИЯ СПЕЦИАЛИСТОВ - ОЦЕНЩИКОВ\"",
                    NameShort = "АССОЦИАЦИЯ СРО \"НКСО\"",
                    NumberRegistration = 0006,
                    Telephone = "+7 (495) 951-03-20",
                    Email = "nprko@nprko.ru",
                    Site = "www.nprko.ru",
                    AddressRegistration = "119017, г. Москва, ул. Малая Ордынка, д. 13, стр. 3",
                    AddressActual = "119017, г. Москва, ул. Малая Ордынка, д. 13, стр. 3"
                },
                new
                {
                    Id = 6,
                    NameFull = "АССОЦИАЦИЯ САМОРЕГУЛИРУЕМАЯ ОРГАНИЗАЦИЯ ОЦЕНЩИКОВ \"СВОБОДНЫЙ ОЦЕНОЧНЫЙ ДЕПАРТАМЕНТ\"",
                    NameShort = "АССОЦИАЦИЯ СРОО \"СВОД\"",
                    NumberRegistration = 0014,
                    Telephone = "8 800 333-87-38",
                    Email = "info@srosvod.ru",
                    Site = "www.srosvod.ru",
                    AddressRegistration = "620100, г. Екатеринбург, ул. Ткачей, д. 23 (БЦ «Clever Park»), офис 13",
                    AddressActual = "620100, г. Екатеринбург, ул. Ткачей, д. 23 (БЦ «Clever Park»), 12 этаж, офис 1214"
                },
                new
                {
                    Id = 7,
                    NameFull = "МЕЖРЕГИОНАЛЬНАЯ САМОРЕГУЛИРУЕМАЯ НЕКОММЕРЧЕСКАЯ ОРГАНИЗАЦИЯ - НЕКОММЕРЧЕСКОЕ ПАРТНЕРСТВО \"ОБЩЕСТВО ПРОФЕССИОНАЛЬНЫХ ЭКСПЕРТОВ И ОЦЕНЩИКОВ\"",
                    NameShort = "МСНО-НП \"ОПЭО\"",
                    NumberRegistration = 0007,
                    Telephone = "+7 (495) 797-55-96",
                    Email = "info@opeo.ru",
                    Site = "www.opeo.ru",
                    AddressRegistration = "125167, г. Москва, ул. 4-я 8 Марта, д. 6А",
                    AddressActual = "г. Москва, ул. 5-я Магистральная, д. 12, 4 этаж, офис 410"
                },
                new
                {
                    Id = 8,
                    NameFull = "НЕКОММЕРЧЕСКОЕ ПАРТНЕРСТВО \"САМОРЕГУЛИРУЕМАЯ ОРГАНИЗАЦИЯ АССОЦИАЦИИ РОССИЙСКИХ МАГИСТРОВ ОЦЕНКИ\"",
                    NameShort = "НП \"АРМО\"",
                    NumberRegistration = 0002,
                    Telephone = "+7 (495) 221-04-25",
                    Email = "armo@sroarmo.ru",
                    Site = "sroarmo.ru",
                    AddressRegistration = "115280, г. Москва, ул. Ленинская Слобода, д. 19",
                    AddressActual = "107023, Москва, ул. Буженинова, д. 30 с. 3, 2 этаж"
                },
                new
                {
                    Id = 9,
                    NameFull = "НЕКОММЕРЧЕСКОЕ ПАРТНЕРСТВО САМОРЕГУЛИРУЕМАЯ ОРГАНИЗАЦИЯ \"ДЕЛОВОЙ СОЮЗ ОЦЕНЩИКОВ\"",
                    NameShort = "НП СРО \"ДСО\"",
                    NumberRegistration = 0012,
                    Telephone = "+7 (499) 230 04 50",
                    Email = "org@srodso.ru",
                    Site = "www.srodso.ru",
                    AddressRegistration = "119180, г. Москва, ул. Большая Якиманка, д. 31, 3 этаж пом. I комната 22, 22а, 23, 23а, 24, 24а, 40",
                    AddressActual = "119180, Россия, Москва, Большая Якиманка, 31, офис 205"
                },
                new
                {
                    Id = 10,
                    NameFull = "САМОРЕГУЛИРУЕМАЯ МЕЖРЕГИОНАЛЬНАЯ АССОЦИАЦИЯ ОЦЕНЩИКОВ",
                    NameShort = "СМАО",
                    NumberRegistration = 0001,
                    Telephone = "+7 (495) 604-41-70",
                    Email = "info@smao.ru",
                    Site = "www.smao.ru",
                    AddressRegistration = "119311, г. Москва, пр-т Вернадского, д. 8А, пом. XXIII",
                    AddressActual = "119311, г. Москва, проспект Вернадского, д.8А, 7 этаж"
                },
                new
                {
                    Id = 11,
                    NameFull = "САМОРЕГУЛИРУЕМАЯ ОРГАНИЗАЦИЯ \"СОЮЗ \"ФЕДЕРАЦИЯ СПЕЦИАЛИСТОВ ОЦЕНЩИКОВ\"",
                    NameShort = "СРО \"СФСО\"",
                    NumberRegistration = 0017,
                    Telephone = "+7 (495) 107 93 70",
                    Email = "info@fsosro.ru",
                    Site = "www.fsosro.ru",
                    AddressRegistration = "109147, г. Москва, ул. Марксистская, д. 34, корп. 10",
                    AddressActual = "109147, г. Москва, ул. Марксистская, дом 34, стр. 10, оф. 20а."
                },
                new
                {
                    Id = 12,
                    NameFull = "САМОРЕГУЛИРУЕМАЯ ОРГАНИЗАЦИЯ АССОЦИАЦИЯ ОЦЕНЩИКОВ \"СООБЩЕСТВО ПРОФЕССИОНАЛОВ ОЦЕНКИ\"",
                    NameShort = "СРО АССОЦИАЦИЯ ОЦЕНЩИКОВ \"СПО\"",
                    NumberRegistration = 0009,
                    Telephone = "+7 (812) 245-39-65",
                    Email = "info@cpa-russia.org ",
                    Site = "www.cpa-russia.org",
                    AddressRegistration = "190000, г. Санкт-Петербург, пер. Гривцова, д. 5, литер Б, кабинет 101",
                    AddressActual = "190000, Санкт-Петербург, пер. Гривцова, д. 5, лит. Б, офис 10"
                },
                new
                {
                    Id = 13,
                    NameFull = "САМОРЕГУЛИРУЕМАЯ ОРГАНИЗАЦИЯ РЕГИОНАЛЬНАЯ АССОЦИАЦИЯ ОЦЕНЩИКОВ",
                    NameShort = "СРО РАО",
                    NumberRegistration = 0013,
                    Telephone = "8 (800) 500 61 81",
                    Email = "sro.raoyufo@gmail.com",
                    Site = "www.srorao.ru",
                    AddressRegistration = "350033, Краснодарский край, г. Краснодар, ул. Адыгейская Набережная, д. 98",
                    AddressActual = "350001, г. Краснодар, ул. Адыгейская набережная, д.98"
                }
                );

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
