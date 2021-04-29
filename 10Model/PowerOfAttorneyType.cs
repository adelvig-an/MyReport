using System.ComponentModel;

namespace _10Model
{
    public enum PowerOfAttorneyType
    {
        [Description("Устава компании")]
        ArticlesOfAssociation = 0, //Устав компаниии
        [Description("Доверенности")]
        Attorney = 1, //Доверенность
        [Description("Законоательства")]
        Legislation = 2 //Законоательство
    }
}
