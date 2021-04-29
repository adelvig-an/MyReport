using System.ComponentModel;

namespace _10Model
{
    public enum SpecialityType
    {
        [Description("Оценка недвижимости")]
        EstateAppraisal, //оценка недвижимости
        [Description("Оценка движимого имущества")]
        ValuationOfMovableProperty, //оценка движимого имущества
        [Description("Оценка бизнеса")]
        BusinessValuation //оцека бизнеса
    }
}
