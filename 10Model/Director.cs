using _10Model.Customer;
using System;
using System.ComponentModel;

namespace _10Model
{
    public enum PowerOfAttorneyType
    {
        [Description("Устав компаниии")]
        ArticlesOfAssociation = 0, //Устав компаниии
        [Description("Доверенность")]
        Attorney = 1, //Доверенность
        [Description("Законоательство")]
        Legislation = 2 //Законоательство
    }
    public class Director : Person
    {
        public string Position { get; set; } // Должность руководителя
        public PowerOfAttorneyType PowerOfAttorney { get; set; } //Действующий на основании (Устав, Доверенность, Закон)
        public string PowerOfAttorneyNumber { get; set; } //Номер доверенности
        public DateTime PowerOfAttorneyDate { get; set; } //Дата доверения
        public DateTime PowerOfAttorneyDateBefore { get; set; } //Дата "действует до"
        public virtual Organization Organization { get; set; }
    }
}
