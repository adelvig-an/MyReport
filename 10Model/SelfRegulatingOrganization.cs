using System;
using System.Collections.Generic;
using System.Text;

namespace _10Model
{
    public class SelfRegulatingOrganization
    {
        public int Id { get; set; }
        public string NameFull { get; set; } //Полное название СРО
        public string NameShort { get; set; } //Сокращенное название СРО
        public int NumberRegistration { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Site { get; set; }
        public string AddressRegistration { get; set; }
        public string AddressActual { get; set; }
        public virtual ICollection<Appraiser> Appraisers { get; set; }
    }
}
