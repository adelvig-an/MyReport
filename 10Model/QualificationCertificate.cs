using System;
using System.ComponentModel;

namespace _10Model
{
    public class QualificationCertificate
    {
        public int Id { get; set; }
        public string Number { get; set; } //Номер квалификационного аттестата
        public DateTime? DateFrom { get; set; } //Дата начала действия
        public DateTime? DateBefore { get; set; } //Дата окончания действия
        public SpecialityType Speciality { get; set; } //Направление оценочной деятельности
        public string NameInstitution { get; } = @"""ФБУ"" ""ФРЦ по организации подготовки управленческих кадров""";
        public string PathQualificationCertificateImage { get; set; }
        public int AppraiserId { get; set; }
        public virtual Appraiser Appraiser { get; set; }
    }
}
