using _10Model;
using _10Model.Helper.Dadata_ru;
using AutoCompleteTextBox.Editors;
using System.Collections;

namespace MyReport.Providers
{
    public class AddressSuggestionProvider : ISuggestionProvider
    {
        public IEnumerable GetSuggestions(string filter)
        {
            var isCorrect = DadataHelper.GetSuggestions(filter, out Address[] address);
            if (isCorrect != false)
            {
                foreach (var adr in address)
                {
                    yield return adr;
                }
            }
        }
    }
}
