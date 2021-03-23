using System;
using System.Collections.Generic;
using System.Text;

namespace _10Model
{
    public class Appraiser : Person
    {
        public DateTime StartedDate { get; set; } //Год начала работы
        //Информация об образовании
        public string Specialization { get; set; } //Название специальности
        public string Serial { get; set; } //Сери диплома
        public int Number { get; set; } //Номер диплома
        public DateTime DiplomDate { get; set; } //Дата выдачи диплома
        public string Universety { get; set; } //Название Университета
        //Информация о СРО
        public string Sro { get; set; } //Название СРО
        public int SroNamder { get; set; } //Регистрационный номер
        public DateTime SroDate { get; set; } //Дата регистрации в СРО
    }
}
