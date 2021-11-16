using _10Model;
using _10Model.Customer;
using _20DbLayer;
using PeterO.Cbor;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace _30ViewModel.PagesVM
{
    public class OrganizationVM : PageViewModel
    {
        #region Properties (Нужны для валидации данных)
        //Свойства для организации
        private string nameShortOpf;
        private string nameFullOpf;
        private string opf;
        private string ogrn;
        private DateTime? ogrnDate = DateTime.Today;
        private string inn;
        private string kpp;
        private string bank;
        private string bik;
        private string payAccount;
        private string corrAccount;
        //Свойства для директора
        private string fullName;
        private string position;
        private PowerOfAttorneyType powerOfAttorney;
        private string powerOfAttorneyNumber;
        private DateTime? powerOfAttorneyDate = DateTime.Today;
        private DateTime? powerOfAttorneyDateBefore = DateTime.Today;
        private string addressRegistration;
        private string addresActual;
        public int Id { get; set; }
        [Required(ErrorMessage = "Требуется указать полное название организации")]
        public string NameFullOpf { get => nameFullOpf;
            set { ValidateProperty(value); SetProperty(ref nameFullOpf, value); } }
        //Свойства для организации
        [Required(ErrorMessage = "Требуется указать сокращенное название организации")]
        public string NameShortOpf { get => nameShortOpf;
            set { ValidateProperty(value); SetProperty(ref nameShortOpf, value); } }
        [Required(ErrorMessage = "Требуется указать организационно-правовую форму")]
        public string Opf { get => opf; 
            set { ValidateProperty(value); SetProperty(ref opf, value); } }
        [Required(ErrorMessage = "Требуется указать ОГРН")]
        [Range(0, ulong.MaxValue, ErrorMessage = "В ОГРН только цифры")]
        [LengthOnOtherPropertyValue("Opf", "ИП", 15, 13, ErrorMessage = "Не верное количество символов для ОГРН")]
        //Если ИП = 15
        //Иначе = 13
        public string Ogrn { get => ogrn;
            set { ValidateProperty(value); SetProperty(ref ogrn, value); } }
        [Required(ErrorMessage = "Требуется указать дату регистрации")]
        public DateTime? OgrnDate { get => ogrnDate;
            set { ValidateProperty(value); SetProperty(ref ogrnDate, value); } }
        [Required(ErrorMessage = "Требуется указать ИНН")]
        [Range(0, ulong.MaxValue, ErrorMessage = "В ИНН только цифры")]
        //Если ИП = 12
        //Иначе = 10
        [LengthOnOtherPropertyValue("Opf", "ИП", 12, 10, ErrorMessage = "Не верное количество символов для ИНН")]
        public string Inn { get => inn;
            set { ValidateProperty(value); SetProperty(ref inn, value); } }
        [Required(ErrorMessage = "Требуется указать КПП")]
        [Range(0, ulong.MaxValue, ErrorMessage = "В КПП только цифры")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Не верное количество символов в КПП")]
        //У ИП КПП нет!!! 
        public string Kpp { 
            get => kpp;
            set 
            {
                if (Opf == "ИП")
                    ClearValidation();
                else
                    ValidateProperty(value);
                SetProperty(ref kpp, value); 
            } 
        }
        [Required(ErrorMessage = "Требуется указать название банка")]
        public string Bank { get => bank; 
            set { ValidateProperty(value); SetProperty(ref bank, value); } }
        [Required(ErrorMessage = "Требуется указать БИК банка")]
        [Range(0, int.MaxValue, ErrorMessage = "В БИК банка только цифры")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Не верное количество символов в БИК банка")]
        public string Bik { get => bik; 
            set { ValidateProperty(value); SetProperty(ref bik, value); } }
        [Required(ErrorMessage = "Требуется указать расчетный счет")]
        [Range(0, ulong.MaxValue, ErrorMessage = "В расчетном счете только цифры")]
        [StringLength(20, MinimumLength = 20, ErrorMessage = "Не верное количество символов в расчетном счете")]
        public string PayAccount { get => payAccount;
            set { ValidateProperty(value); SetProperty(ref payAccount, value); } }
        [Required(ErrorMessage = "Требуется указать корреспондентский счет")]
        [Range(0, ulong.MaxValue, ErrorMessage = "В корреспондентском счете только цифры")]
        [StringLength(20, MinimumLength = 20, ErrorMessage = "Не верное количество символов в корреспондентском счете")]
        public string CorrAccount { get => corrAccount; 
            set { ValidateProperty(value); SetProperty(ref corrAccount, value); } }
        //Свойства для директора
        [Required(ErrorMessage = "Требуется указать ФИО руководителя")]
        public string FullName { get => fullName;
            set { ValidateProperty(value); SetProperty(ref fullName, value); } }
        [Required(ErrorMessage = "Требуется указать должность руководителя")]
        public string Position { get => position;
            set { ValidateProperty(value); SetProperty(ref position, value); } }
        public PowerOfAttorneyType PowerOfAttorney { get => powerOfAttorney;
            set { ValidateProperty(value); SetProperty(ref powerOfAttorney, value); ToVisibl(); } }
        [Required(ErrorMessage = "Требуется указать номер доверенности")]
        public string PowerOfAttorneyNumber { get => powerOfAttorneyNumber;
            set { ValidateProperty(value); SetProperty(ref powerOfAttorneyNumber, value); } }
        [Required(ErrorMessage = "Требуется указать дату доверения")]
        public DateTime? PowerOfAttorneyDate { get => powerOfAttorneyDate;
            set { ValidateProperty(value); SetProperty(ref powerOfAttorneyDate, value); } }
        [Required(ErrorMessage = "Требуется указать дату окончания действия доверенности")]
        public DateTime? PowerOfAttorneyDateBefore { get => powerOfAttorneyDateBefore;
            set { ValidateProperty(value); SetProperty(ref powerOfAttorneyDateBefore, value); } }
        [Required(ErrorMessage = "Требуется указать адрес регистрации")]
        [StringLength(255, ErrorMessage = "Привышение максимально допустимого количества символов")]
        public string AddressRegistration { get => addressRegistration;
            set { ValidateProperty(value); SetProperty(ref addressRegistration, value); } }
        [Required(ErrorMessage = "Требуется указать адрес местонахождения")]
        [StringLength(255, ErrorMessage = "Привышение максимально допустимого количества символов")]
        public string AddressActual { get => addresActual;
            set { ValidateProperty(value); SetProperty(ref addresActual, value); } }
        #endregion Properties

        #region AddressMatch (Совпадение адресов регистрации и проживания)
        private bool isAddressMatch;
        public bool IsAddressMatch
        {
            get => isAddressMatch;
            set
            {
                SetProperty(ref isAddressMatch, value);
                if (value == true)
                    ActualToRegistration();
                else
                    ActualToActual();
            }
        }
        public void ActualToRegistration() => SelectedAddressActual = SelectedAddressRegistration;
        public void ActualToActual() => SelectedAddressActual = null;
        #endregion AddressMatch

        private readonly ApplicationContext context;
        public OrganizationVM()
        {
            context = new ApplicationContext();
        }

        #region Visibility
        private bool isAttorneyValue;
        public bool IsAttorneyValue
        {
            get => isAttorneyValue;
            set =>
                SetProperty(ref isAttorneyValue, value);
        }
        public void ToVisibl()
        {
            if (PowerOfAttorney == PowerOfAttorneyType.Attorney)
                IsAttorneyValue = true;
            else
                IsAttorneyValue = false;
        }
        #endregion

        #region DataBase (Методы и свойства взаимодействующие с Базой данных)
        public Organization ToOrganization()
        {
            var organization = new Organization
            {
                Id = Id,
                NameFullOpf = NameFullOpf,
                NameShortOpf = NameShortOpf,
                Opf = Opf,
                Ogrn = Ogrn,
                OgrnDate = OgrnDate,
                Inn = Inn,
                Kpp = Kpp,
                Bank = Bank,
                Bik = Bik,
                PayAccount = PayAccount,
                CorrAccount = CorrAccount,
                Director = ToDirector(),
                AddressRegistration = SelectedAddressRegistration ?? SelectedOrganization?.AddressRegistration,
                AddressActual = SelectedAddressActual
            };
            return organization;
        }
        public Director ToDirector()
        {
            var director = new Director
            {
                Id = Id,
                FullName = FullName,
                Position = Position,
                PowerOfAttorney = PowerOfAttorney,
                PowerOfAttorneyNumber = PowerOfAttorneyNumber,
                PowerOfAttorneyDate = PowerOfAttorneyDate,
                PowerOfAttorneyDateBefore = PowerOfAttorneyDateBefore
            };
            return director;
        }
        public void AddOrganization()
        {
            try
            {
                Organization organization = ToOrganization();
                context.Add(organization);
                context.SaveChanges();
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
            }
        }
        public bool UpdateOrganization()
        {
            try
            {
                var organization = context.Organizations.First();
                organization = ToOrganization();
                context.SaveChanges();
                return true;
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                return false;
            }
        }
        #endregion DataBase

        #region AutoCompleteOrganization
        private Organization selectedOrganization;
        public Organization SelectedOrganization
        {
            get => selectedOrganization;
            set
            {
                if (SetProperty(ref selectedOrganization, value))
                    FillOrganization(selectedOrganization);
            }
        }
        public void FillOrganization(Organization organization) 
        {
            NameShortOpf = organization?.NameShortOpf;
            Opf = organization?.Opf;
            Inn = organization?.Inn;
            Kpp = organization?.Kpp;
            Ogrn = organization?.Ogrn;
            OgrnDate = organization?.OgrnDate;
            FullName = organization?.Director?.FullName;
            Position = organization?.Director?.Position;
            SelectedAddressRegistration = organization?.AddressRegistration;
        }
        #endregion AutoCompleteOrganization

        #region AutoCompleteAddress (Подсказки заполнения адреса)
        private Address selectedAddressRegistration;
        private Address selectedAddressActual;
        public Address SelectedAddressRegistration
        {
            get => selectedAddressRegistration;
            set
            {
                if (SetProperty(ref selectedAddressRegistration, value))
                    FillAddressRegistration(selectedAddressRegistration);
            }
        }
        public Address SelectedAddressActual
        {
            get => selectedAddressActual;
            set
            {
                if (SetProperty(ref selectedAddressActual, value))
                    FillAddressActual(selectedAddressActual);
            }
        }
        public void FillAddressRegistration(Address address) => AddressRegistration = address?.AddressFull;
        public void FillAddressActual(Address address) => AddressActual = address?.AddressFull;
        #endregion AutoCompleteAddress

        #region CBOR
        static CBORObject ToCBOR(OrganizationVM organizationVM)
        {
            return CBORObject.NewArray()
                .Add(organizationVM.Id)
                .Add(organizationVM.NameShortOpf)
                .Add(organizationVM.Opf)
                .Add(organizationVM.Ogrn)
                .Add(organizationVM.OgrnDate.HasValue
                ? CBORObject.NewArray().Add(true).Add(organizationVM.OgrnDate.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(organizationVM.Inn)
                .Add(organizationVM.Kpp)
                .Add(organizationVM.Bank)
                .Add(organizationVM.Bik)
                .Add(organizationVM.PayAccount)
                .Add(organizationVM.CorrAccount)
                .Add(organizationVM.FullName)
                .Add(organizationVM.Position)
                .Add(organizationVM.PowerOfAttorney)
                .Add(organizationVM.PowerOfAttorneyNumber)
                .Add(organizationVM.PowerOfAttorneyDate.HasValue
                ? CBORObject.NewArray().Add(true).Add(organizationVM.PowerOfAttorneyDate.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(organizationVM.PowerOfAttorneyDateBefore.HasValue
                ? CBORObject.NewArray().Add(true).Add(organizationVM.PowerOfAttorneyDateBefore.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(organizationVM.SelectedAddressRegistration?.Id)
                .Add(organizationVM.SelectedAddressRegistration?.AddressFull)
                .Add(organizationVM.SelectedAddressRegistration?.Index)
                .Add(organizationVM.SelectedAddressRegistration?.Country)
                .Add(organizationVM.SelectedAddressRegistration?.FederalDistrict)
                .Add(organizationVM.SelectedAddressRegistration?.RegionKladrId)
                .Add(organizationVM.SelectedAddressRegistration?.RegionWthType)
                .Add(organizationVM.SelectedAddressRegistration?.RegionType)
                .Add(organizationVM.SelectedAddressRegistration?.RegionTypeFull)
                .Add(organizationVM.SelectedAddressRegistration?.Region)
                .Add(organizationVM.SelectedAddressRegistration?.AreaKladrId)
                .Add(organizationVM.SelectedAddressRegistration?.AreaWithType)
                .Add(organizationVM.SelectedAddressRegistration?.AreaType)
                .Add(organizationVM.SelectedAddressRegistration?.AreaTypeFull)
                .Add(organizationVM.SelectedAddressRegistration?.Area)
                .Add(organizationVM.SelectedAddressRegistration?.CityKladrId)
                .Add(organizationVM.SelectedAddressRegistration?.CityWithType)
                .Add(organizationVM.SelectedAddressRegistration?.CityType)
                .Add(organizationVM.SelectedAddressRegistration?.CityTypeFull)
                .Add(organizationVM.SelectedAddressRegistration?.City)
                .Add(organizationVM.SelectedAddressRegistration?.CityDistrictWithType)
                .Add(organizationVM.SelectedAddressRegistration?.CityDistrictType)
                .Add(organizationVM.SelectedAddressRegistration?.CityDistrictTypeFull)
                .Add(organizationVM.SelectedAddressRegistration?.CityDistrict)
                .Add(organizationVM.SelectedAddressRegistration?.SettlementKladrId)
                .Add(organizationVM.SelectedAddressRegistration?.SettlemenWithType)
                .Add(organizationVM.SelectedAddressRegistration?.SettlemenType)
                .Add(organizationVM.SelectedAddressRegistration?.SettlemenTypeFull)
                .Add(organizationVM.SelectedAddressRegistration?.Settlemen)
                .Add(organizationVM.SelectedAddressRegistration?.StreetKladrId)
                .Add(organizationVM.SelectedAddressRegistration?.StreetWithType)
                .Add(organizationVM.SelectedAddressRegistration?.StreetType)
                .Add(organizationVM.SelectedAddressRegistration?.StreetTypeFull)
                .Add(organizationVM.SelectedAddressRegistration?.Street)
                .Add(organizationVM.SelectedAddressRegistration?.HouseKladrId)
                .Add(organizationVM.SelectedAddressRegistration?.HouseType)
                .Add(organizationVM.SelectedAddressRegistration?.HouseTypeFull)
                .Add(organizationVM.SelectedAddressRegistration?.House)
                .Add(organizationVM.SelectedAddressRegistration?.BlockType)
                .Add(organizationVM.SelectedAddressRegistration?.BloctTypeFull)
                .Add(organizationVM.SelectedAddressRegistration?.Block)
                .Add(organizationVM.SelectedAddressRegistration?.Entrance)
                .Add(organizationVM.SelectedAddressRegistration?.Floor)
                .Add(organizationVM.SelectedAddressRegistration?.FlatType)
                .Add(organizationVM.SelectedAddressRegistration?.FlatTypeFull)
                .Add(organizationVM.SelectedAddressRegistration?.Flat)
                .Add(organizationVM.IsAddressMatch)
                .Add(organizationVM.SelectedAddressActual?.Id)
                .Add(organizationVM.SelectedAddressActual?.AddressFull)
                .Add(organizationVM.SelectedAddressActual?.Index)
                .Add(organizationVM.SelectedAddressActual?.Country)
                .Add(organizationVM.SelectedAddressActual?.FederalDistrict)
                .Add(organizationVM.SelectedAddressActual?.RegionKladrId)
                .Add(organizationVM.SelectedAddressActual?.RegionWthType)
                .Add(organizationVM.SelectedAddressActual?.RegionType)
                .Add(organizationVM.SelectedAddressActual?.RegionTypeFull)
                .Add(organizationVM.SelectedAddressActual?.Region)
                .Add(organizationVM.SelectedAddressActual?.AreaKladrId)
                .Add(organizationVM.SelectedAddressActual?.AreaWithType)
                .Add(organizationVM.SelectedAddressActual?.AreaType)
                .Add(organizationVM.SelectedAddressActual?.AreaTypeFull)
                .Add(organizationVM.SelectedAddressActual?.Area)
                .Add(organizationVM.SelectedAddressActual?.CityKladrId)
                .Add(organizationVM.SelectedAddressActual?.CityWithType)
                .Add(organizationVM.SelectedAddressActual?.CityType)
                .Add(organizationVM.SelectedAddressActual?.CityTypeFull)
                .Add(organizationVM.SelectedAddressActual?.City)
                .Add(organizationVM.SelectedAddressActual?.CityDistrictWithType)
                .Add(organizationVM.SelectedAddressActual?.CityDistrictType)
                .Add(organizationVM.SelectedAddressActual?.CityDistrictTypeFull)
                .Add(organizationVM.SelectedAddressActual?.CityDistrict)
                .Add(organizationVM.SelectedAddressActual?.SettlementKladrId)
                .Add(organizationVM.SelectedAddressActual?.SettlemenWithType)
                .Add(organizationVM.SelectedAddressActual?.SettlemenType)
                .Add(organizationVM.SelectedAddressActual?.SettlemenTypeFull)
                .Add(organizationVM.SelectedAddressActual?.Settlemen)
                .Add(organizationVM.SelectedAddressActual?.StreetKladrId)
                .Add(organizationVM.SelectedAddressActual?.StreetWithType)
                .Add(organizationVM.SelectedAddressActual?.StreetType)
                .Add(organizationVM.SelectedAddressActual?.StreetTypeFull)
                .Add(organizationVM.SelectedAddressActual?.Street)
                .Add(organizationVM.SelectedAddressActual?.HouseKladrId)
                .Add(organizationVM.SelectedAddressActual?.HouseType)
                .Add(organizationVM.SelectedAddressActual?.HouseTypeFull)
                .Add(organizationVM.SelectedAddressActual?.House)
                .Add(organizationVM.SelectedAddressActual?.BlockType)
                .Add(organizationVM.SelectedAddressActual?.BloctTypeFull)
                .Add(organizationVM.SelectedAddressActual?.Block)
                .Add(organizationVM.SelectedAddressActual?.Entrance)
                .Add(organizationVM.SelectedAddressActual?.Floor)
                .Add(organizationVM.SelectedAddressActual?.FlatType)
                .Add(organizationVM.SelectedAddressActual?.FlatTypeFull)
                .Add(organizationVM.SelectedAddressActual?.Flat);
        }
        void FromCBOR(CBORObject cbor)
        {
            Id = cbor[0].AsInt32();
            NameShortOpf = cbor[1].AsString();
            Opf = cbor[2].AsString();
            Ogrn = cbor[3].AsString();
            OgrnDate = cbor[4][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[4][1].ToObject<long>()))
            : null;
            Inn = cbor[5].AsString();
            Kpp = cbor[6].AsString();
            Bank = cbor[7].AsString();
            Bik = cbor[8].AsString();
            PayAccount = cbor[9].AsString();
            CorrAccount = cbor[10].AsString();
            FullName = cbor[11].AsString();
            Position = cbor[12].AsString();
            PowerOfAttorney = (PowerOfAttorneyType)Enum.Parse(typeof(PowerOfAttorneyType), cbor[13].ToString(), true);
            PowerOfAttorneyNumber = cbor[14].AsString();
            PowerOfAttorneyDate = cbor[15][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[15][1].ToObject<long>()))
            : null;
            PowerOfAttorneyDateBefore = cbor[16][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[16][1].ToObject<long>()))
            : null;
            AddressRegistration = cbor[18].AsString();
            IsAddressMatch = cbor[63].AsBoolean();
            AddressActual = cbor[65].AsString();
            SelectedAddressRegistration = new Address() 
            { 
                Id = cbor[17].AsInt32(),
                AddressFull = cbor[18].AsStringSafe(),
                Index = cbor[19].AsStringSafe(),
                Country = cbor[20].AsStringSafe(),
                FederalDistrict = cbor[21].AsStringSafe(),
                RegionKladrId = cbor[22].AsStringSafe(),
                RegionWthType = cbor[23].AsStringSafe(),
                RegionType = cbor[24].AsStringSafe(),
                RegionTypeFull = cbor[25].AsStringSafe(),
                Region = cbor[26].AsStringSafe(),
                AreaKladrId = cbor[27].AsStringSafe(),
                AreaWithType = cbor[28].AsStringSafe(),
                AreaType = cbor[29].AsStringSafe(),
                AreaTypeFull = cbor[30].AsStringSafe(),
                Area = cbor[31].AsStringSafe(),
                CityKladrId = cbor[32].AsStringSafe(),
                CityWithType = cbor[33].AsStringSafe(),
                CityType = cbor[34].AsStringSafe(),
                CityTypeFull = cbor[35].AsStringSafe(),
                City = cbor[36].AsStringSafe(),
                CityDistrictWithType = cbor[37].AsStringSafe(),
                CityDistrictType = cbor[38].AsStringSafe(),
                CityDistrictTypeFull = cbor[39].AsStringSafe(),
                CityDistrict = cbor[40].AsStringSafe(),
                SettlementKladrId = cbor[41].AsStringSafe(),
                SettlemenWithType = cbor[42].AsStringSafe(),
                SettlemenType = cbor[43].AsStringSafe(),
                SettlemenTypeFull = cbor[44].AsStringSafe(),
                Settlemen = cbor[45].AsStringSafe(),
                StreetKladrId = cbor[46].AsStringSafe(),
                StreetWithType = cbor[47].AsStringSafe(),
                StreetType = cbor[48].AsStringSafe(),
                StreetTypeFull = cbor[49].AsStringSafe(),
                Street = cbor[50].AsStringSafe(),
                HouseKladrId = cbor[51].AsStringSafe(),
                HouseType = cbor[52].AsStringSafe(),
                HouseTypeFull = cbor[53].AsStringSafe(),
                House = cbor[54].AsStringSafe(),
                BlockType = cbor[55].AsStringSafe(),
                BloctTypeFull = cbor[56].AsStringSafe(),
                Block = cbor[57].AsStringSafe(),
                Entrance = cbor[58].AsStringSafe(),
                Floor = cbor[59].AsStringSafe(),
                FlatType = cbor[60].AsStringSafe(),
                FlatTypeFull = cbor[61].AsStringSafe(),
                Flat = cbor[62].AsStringSafe()
            };
            SelectedAddressActual = new Address()
            {
                Id = cbor[64].AsInt32(),
                AddressFull = cbor[65].AsStringSafe(),
                Index = cbor[66].AsStringSafe(),
                Country = cbor[67].AsStringSafe(),
                FederalDistrict = cbor[68].AsStringSafe(),
                RegionKladrId = cbor[69].AsStringSafe(),
                RegionWthType = cbor[70].AsStringSafe(),
                RegionType = cbor[71].AsStringSafe(),
                RegionTypeFull = cbor[72].AsStringSafe(),
                Region = cbor[73].AsStringSafe(),
                AreaKladrId = cbor[74].AsStringSafe(),
                AreaWithType = cbor[75].AsStringSafe(),
                AreaType = cbor[76].AsStringSafe(),
                AreaTypeFull = cbor[77].AsStringSafe(),
                Area = cbor[78].AsStringSafe(),
                CityKladrId = cbor[79].AsStringSafe(),
                CityWithType = cbor[80].AsStringSafe(),
                CityType = cbor[81].AsStringSafe(),
                CityTypeFull = cbor[82].AsStringSafe(),
                City = cbor[83].AsStringSafe(),
                CityDistrictWithType = cbor[84].AsStringSafe(),
                CityDistrictType = cbor[85].AsStringSafe(),
                CityDistrictTypeFull = cbor[86].AsStringSafe(),
                CityDistrict = cbor[87].AsStringSafe(),
                SettlementKladrId = cbor[88].AsStringSafe(),
                SettlemenWithType = cbor[89].AsStringSafe(),
                SettlemenType = cbor[90].AsStringSafe(),
                SettlemenTypeFull = cbor[91].AsStringSafe(),
                Settlemen = cbor[92].AsStringSafe(),
                StreetKladrId = cbor[93].AsStringSafe(),
                StreetWithType = cbor[94].AsStringSafe(),
                StreetType = cbor[95].AsStringSafe(),
                StreetTypeFull = cbor[96].AsStringSafe(),
                Street = cbor[97].AsStringSafe(),
                HouseKladrId = cbor[98].AsStringSafe(),
                HouseType = cbor[99].AsStringSafe(),
                HouseTypeFull = cbor[100].AsStringSafe(),
                House = cbor[101].AsStringSafe(),
                BlockType = cbor[102].AsStringSafe(),
                BloctTypeFull = cbor[103].AsStringSafe(),
                Block = cbor[104].AsStringSafe(),
                Entrance = cbor[105].AsStringSafe(),
                Floor = cbor[106].AsStringSafe(),
                FlatType = cbor[107].AsStringSafe(),
                FlatTypeFull = cbor[108].AsStringSafe(),
                Flat = cbor[109].AsStringSafe()
            };
            SelectedOrganization = ToOrganization(); //Восстановление SelectedOrganization
        }
        public override byte[] GetCBOR() => ToCBOR(this).EncodeToBytes();
        public override void SetCBOR(byte[] b) => FromCBOR(CBORObject.DecodeFromBytes(b));
        #endregion CBOR
    }
}
