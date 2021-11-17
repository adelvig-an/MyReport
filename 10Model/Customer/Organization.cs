using System;

namespace _10Model.Customer
{
    public class Organization
    {
        public int Id { get; set; }
        public string NameFullOpf { get; set; } //Полное наименование с ОПФ
        public string NameShortOpf { get; set; } //Сокращенное наименование с ОПФ
        public string NameFull { get; set; } //полное наименование без ОПФ
        public string NameShort { get; set; } //краткое наименование без ОПФ
        //public string SecondName { get; set; } //Фамилия индивидуального предпринимателя
        //public string FirstName { get; set; } //Имя индивидуального предпринимателя
        //public string MiddleName { get; set; } //Отчество индивидуального предпринимателя
        public string FullOpf { get; set; } //Полное название ОПФ
        public string ShortOpf { get; set; } //Сокращенное ОПФ
        public string Ogrn { get; set; } //ОГРН
        public DateTime? OgrnDate { get; set; } //Дата регистрации
        public string Inn { get; set; } //ИНН
        public string Kpp { get; set; } //КПП
        public string Bank { get; set; } //Название банка
        public string Bik { get; set; } //БИК Банка
        public string PayAccount { get; set; } //Расчетный счет
        public string CorrAccount { get; set; } //Корреспондентский счет
        public int DirectorId { get; set; }
        public virtual Director Director { get; set; }
        public virtual Address AddressRegistration { get; set; }
        public virtual Address AddressActual { get; set; }
    }
}
