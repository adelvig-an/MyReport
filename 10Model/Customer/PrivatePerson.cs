using System;
using System.Collections.Generic;
using System.Text;

namespace _10Model.Customer
{
    public class PrivatePerson : Person
    {
        public string Serial { get; set; } //Серия документа
        public string Number { get; set; } //Номер документа
        public string Division { get; set; } //Кем выдан документ
        //public string DivisionCode { get; set; } //Код подразделения
        public DateTime? DivisionDate { get; set; } //Дата выдачи
        public virtual Address AddressRegistration { get; set; }
        public virtual Address AddressActual { get; set; }
    }
}
