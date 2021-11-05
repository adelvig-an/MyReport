using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _10Model
{
    public class Appraiser : Person
    {
        public DateTime? StartedDate { get; set; } //Год начала работы
        //Информация об образовании
        public string Specialization { get; set; } //Название специальности
        public string Number { get; set; } //Номер диплома
        public DateTime? DiplomDate { get; set; } //Дата выдачи диплома
        public string Universety { get; set; } //Название Университета
        public string PathDiplomImage { get; set; } //Путь к изображениям диплома в JSON
        //Информация о СРО
        public string Sro { get; set; } //Название СРО
        public int SroNumber { get; set; } //Регистрационный номер
        public DateTime? SroDate { get; set; } //Дата регистрации в СРО
        public string PathSroCertificateImage { get; set; } //Путь к изображениям свидетельства СРО в JSON
        public virtual ICollection<QualificationCertificate> QualificationCertificates { get; set; }
            = new ObservableCollection<QualificationCertificate>();
        public int InsurancePolicieId { get; set; }
        public virtual InsurancePolicie InsurancePolicie { get; set; }
        //public int AppraiserOrganizationId { get; set; }
        //public virtual AppraiserOrganization AppraiserOrganization { get; set; }
    }
}
