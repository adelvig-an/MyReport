using System;

namespace _10Model
{
    public class Contract
    {
        public int Id { get; set; }
        public string Number { get; set; } //Номер договора
        public DateTime? ContractDate { get; set; } //Дата договора
        public TargetType Target { get; set; }
        public string IntendedUse { get; set; } //Предполагаемый вид использования
    }
}
