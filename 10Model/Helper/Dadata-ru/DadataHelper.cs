using _10Model.Customer;
using Dadata;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace _10Model.Helper.Dadata_ru
{
    public class DadataHelper
    {
        public static bool GetSuggestions(string fullAddress, out Address[] addresses)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("Config.json");

            IConfigurationRoot configurationRoot = configuration.Build();

            DadataConf dadataConf = new DadataConf();
            configurationRoot.GetSection(nameof(DadataConf)).Bind(dadataConf);

            var token = dadataConf.Token;

            var client = new SuggestClientSync(token);
            try
            {
                var adr = client.SuggestAddress(fullAddress);
                addresses = adr.suggestions.Select(address => ToAddress(address)).ToArray();
                return true;
            }
            catch
            {
                addresses = null;
                return false;
            }
        }
        public static Address ToAddress(Dadata.Model.Address address)
        {
            return new Address
            {
                AddressFull = address.result,
                Index = address.postal_code,
                Country = address.country,
                FederalDistrict = address.federal_district,
                RegionKladrId = address.region_kladr_id,
                RegionWthType = address.region_with_type,
                RegionType = address.region_type,
                RegionTypeFull = address.region_type_full,
                Region = address.region,
                AreaKladrId = address.area_kladr_id,
                AreaWithType = address.area_with_type,
                AreaType = address.area_type,
                AreaTypeFull = address.area_type_full,
                Area = address.area,
                CityKladrId = address.city_district_kladr_id,
                CityWithType = address.city_with_type,
                CityType = address.city_type,
                CityTypeFull = address.city_type_full,
                City = address.city,
                CityDistrictWithType = address.city_district_with_type,
                CityDistrictType = address.city_district_type,
                CityDistrictTypeFull = address.city_district_type_full,
                CityDistrict = address.city_district,
                SettlementKladrId = address.settlement_kladr_id,
                SettlemenWithType = address.settlement_with_type,
                SettlemenType = address.settlement_type,
                SettlemenTypeFull = address.settlement_type_full,
                Settlemen = address.settlement,
                StreetKladrId = address.street_kladr_id,
                StreetWithType = address.street_with_type,
                StreetType = address.street_type,
                StreetTypeFull = address.street_type_full,
                Street = address.street,
                HouseKladrId = address.house_kladr_id,
                HouseType = address.house_type,
                HouseTypeFull = address.house_type_full,
                House = address.house,
                BlockType = address.block_type,
                BloctTypeFull = address.block_type_full,
                Block = address.block,
                FlatType = address.flat_type,
                FlatTypeFull = address.flat_type_full,
                Flat = address.flat
            };
        }
        public static Address ToAddress(Dadata.Model.Suggestion<Dadata.Model.Address> suggestion)
        {
            // ToDo: проверить что содержится в suggestion.data.result изначально
            suggestion.data.result = suggestion.value;
            return ToAddress(suggestion.data);
        }
        

        public static bool GetSuggestions(string query, out Organization[] organizations)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("Config.json");

            IConfigurationRoot configurationRoot = configuration.Build();

            DadataConf dadataConf = new DadataConf();
            configurationRoot.GetSection(nameof(DadataConf)).Bind(dadataConf);

            var token = dadataConf.Token;

            var client = new SuggestClientSync(token);
            try
            {
                var org = client.SuggestParty(query);
                organizations = org.suggestions.Select(party => ToOrganization(party)).ToArray();
                return true;
            }
            catch
            {
                organizations = null;
                return false;
            }
        }
        public static Organization ToOrganization(Dadata.Model.Party party)
        {
            return new Organization
            {
                NameFullOpf = party.name.full_with_opf,
                NameShortOpf = party.name.short_with_opf,
                Opf = party.opf.@short,
                Ogrn = long.Parse(party.ogrn),
                OgrnDate = (System.DateTime)party.ogrn_date,
                Inn = int.Parse(party.inn),
                Kpp = int.Parse(party.kpp)
            };
        }
        public static Organization ToOrganization(Dadata.Model.Suggestion<Dadata.Model.Party> suggestion)
        {
            suggestion.value = suggestion.value;
            return ToOrganization(suggestion.data);
        }
        public static Director ToDirector(Dadata.Model.Party party)
        {
            return new Director
            {
                FullName = party.management.name,
                Position = party.management.post
            };
        }
        public static Address ToAddressRegistration(Dadata.Model.Party party)
        {
            return new Address { AddressFull = party.address.unrestricted_value };
        }
    }
}
