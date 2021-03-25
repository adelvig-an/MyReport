using System;
using System.Collections.Generic;
using System.Text;

namespace _10Model
{
    public enum SpecialityType
    {
        EstateAppraisal, //оценка недвижимости
        ValuationOfMovableProperty, //оценка движимого имущества
        BusinessValuation //оцека бизнеса
    }
    public class QualificationCertificate
    {
        public int Id { get; set; }
        public int Number { get; set; } //Номер квалификационного аттестата
        public DateTime? DateFrom { get; set; } //Дата начала действия
        public DateTime? DateBefore { get; set; } //Дата окончания действия
        public SpecialityType Speciality { get; set; } //Направление оценочной деятельности
        public int AppraiserId { get; set; }
        public virtual Appraiser Appraiser { get; set; }
    }
}
