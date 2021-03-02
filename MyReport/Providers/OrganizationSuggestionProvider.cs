using _10Model;
using _10Model.Customer;
using _10Model.Helper.Dadata_ru;
using AutoCompleteTextBox.Editors;
using System.Collections;

namespace MyReport.Providers
{
    class OrganizationSuggestionProvider : ISuggestionProvider
    {
        public IEnumerable GetSuggestions(string filter)
        {
            var isCorrect = DadataHelper.GetSuggestions(filter, out Organization[] organizations, out Director[] directors, out Address[] addresses);
            if (isCorrect != false)
            {
                foreach (var org in organizations)
                {
                    yield return org;
                }
                foreach (var dir in directors)
                {
                    yield return directors;
                }
                foreach (var adr in addresses)
                {
                    yield return adr;
                }
            }
        }
    }
}
