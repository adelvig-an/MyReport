using _10Model.Customer;
using System;

namespace _10Model
{
    public class Director : Person
    {
        public string Position { get; set; } // Должность руководителя
        public PowerOfAttorneyType PowerOfAttorney { get; set; } //Действующий на основании (Устав, Доверенность, Закон)
        public string PowerOfAttorneyNumber { get; set; } //Номер доверенности
        public DateTime? PowerOfAttorneyDate { get; set; } //Дата доверения
        public DateTime? PowerOfAttorneyDateBefore { get; set; } //Дата "действует до"
        public virtual Organization Organization { get; set; }
    }
}
