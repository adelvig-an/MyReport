using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace _10Model
{
    public enum SpecialityType
    {
        [Description("Оценка недвижимости")]
        EstateAppraisal, //оценка недвижимости
        [Description("Оценка движимого имущества")]
        ValuationOfMovableProperty, //оценка движимого имущества
        [Description("Оценка бизнеса")]
        BusinessValuation //оцека бизнеса
    }
    public class QualificationCertificate
    {
        public int Id { get; set; }
        public string Number { get; set; } //Номер квалификационного аттестата
        public DateTime? DateFrom { get; set; } //Дата начала действия
        public DateTime? DateBefore { get; set; } //Дата окончания действия
        public SpecialityType Speciality { get; set; } //Направление оценочной деятельности
        public int AppraiserId { get; set; }
        public virtual Appraiser Appraiser { get; set; }
    }
}
