using System;

namespace _10Model
{
    public class InsurancePolicie
    {
        public int Id { get; set; }
        public string InsuranceCompany { get; set; } //Название страховой компании
        public string Number { get; set; } //Номер полиса
        public decimal InsuranceMoney { get; set; } //Страховое возмещение (Застрахованная сумма)
        public DateTime? DateFrom { get; set; } //Дата начала действия страхового полиса
        public DateTime? DateBefore { get; set; } //Дата окончания действия страхового полиса
        public string PathInsurancePolicieImage { get; set; }
        public virtual Appraiser Appraiser { get; set; }
        public virtual AppraiserOrganization AppraiserOrganization { get; set; }
    }
}
