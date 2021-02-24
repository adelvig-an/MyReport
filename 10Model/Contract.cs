using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace _10Model
{
    public enum TargetType
    {
        [Description("Рыночная стоимость")]
        MarketValue = 0,
        [Description("Рыночная и ликвидационная стоимость")]
        MarketAndLiquidationValue = 1,
        [Description("Ликвидационная стоимость")]
        LiquidationValue = 2,
        [Description("Инвестиционная стоимость")]
        InvestmentValue = 3,
    }
    public class Contract
    {
        public int Id { get; set; }
        public string Number { get; set; } //Номер договора
        public DateTime ContractDate { get; set; } //Дата договора
        public TargetType Target { get; set; }
        public string IntendedUse { get; set; } //Предполагаемый вид использования
    }
}
