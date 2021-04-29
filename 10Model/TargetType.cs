using System.ComponentModel;

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
}
