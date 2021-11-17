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
        private string nameShort;
        private string nameFull;
        private string fullOpf;
        private string shortOpf;
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
        [Required(ErrorMessage = "Требуется указать сокращенное название организации")]
        public string NameShortOpf { get => nameShortOpf;
            set { ValidateProperty(value); SetProperty(ref nameShortOpf, value); } }
        [Required(ErrorMessage = "Требуется указать полное название организации")]
        public string NameFull
        {
            get => nameFull;
            set { ValidateProperty(value); SetProperty(ref nameFull, value); }
        }
        public string NameShort
        {
            get => nameShort;
            set { ValidateProperty(value); SetProperty(ref nameShort, value); }
        }
        [Required(ErrorMessage = "Требуется указать организационно-правовую форму")]
        public string FullOpf { get => fullOpf; 
            set { ValidateProperty(value); SetProperty(ref fullOpf, value); } }
        public string ShortOpf
        {
            get => shortOpf;
            set { ValidateProperty(value); SetProperty(ref shortOpf, value); }
        }
        [Required(ErrorMessage = "Требуется указать ОГРН")]
        [Range(0, ulong.MaxValue, ErrorMessage = "В ОГРН только цифры")]
        [LengthOnOtherPropertyValue("ShortOpf", "ИП", 15, 13, ErrorMessage = "Не верное количество символов для ОГРН")]
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
        [LengthOnOtherPropertyValue("ShortOpf", "ИП", 12, 10, ErrorMessage = "Не верное количество символов для ИНН")]
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
                if (ShortOpf == "ИП")
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
                NameFull = NameFull,
                NameShort= NameShort,
                FullOpf = FullOpf,
                ShortOpf = ShortOpf,
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
            NameFullOpf = organization?.NameFullOpf;
            NameShortOpf = organization?.NameShortOpf;
            NameFull = organization?.NameFull;
            NameShort = organization?.NameShort;
            FullOpf = organization?.FullOpf;
            ShortOpf = organization?.ShortOpf;
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
                .Add(organizationVM.NameFullOpf)
                .Add(organizationVM.NameShortOpf)
                .Add(organizationVM.NameFull)
                .Add(organizationVM.NameShort)
                .Add(organizationVM.FullOpf)
                .Add(organizationVM.ShortOpf)
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
            NameFullOpf = cbor[1].AsString();
            NameShortOpf = cbor[2].AsString();
            NameFull = cbor[3].AsString();
            NameShort = cbor[4].AsString();
            FullOpf = cbor[5].AsString();
            ShortOpf = cbor[6].AsString();
            Ogrn = cbor[7].AsString();
            OgrnDate = cbor[8][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[8][1].ToObject<long>()))
            : null;
            Inn = cbor[9].AsString();
            Kpp = cbor[10].AsString();
            Bank = cbor[11].AsString();
            Bik = cbor[12].AsString();
            PayAccount = cbor[13].AsString();
            CorrAccount = cbor[14].AsString();
            FullName = cbor[15].AsString();
            Position = cbor[16].AsString();
            PowerOfAttorney = (PowerOfAttorneyType)Enum.Parse(typeof(PowerOfAttorneyType), cbor[17].ToString(), true);
            PowerOfAttorneyNumber = cbor[18].AsString();
            PowerOfAttorneyDate = cbor[19][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[19][1].ToObject<long>()))
            : null;
            PowerOfAttorneyDateBefore = cbor[20][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[20][1].ToObject<long>()))
            : null;
            AddressRegistration = cbor[22].AsString();
            IsAddressMatch = cbor[67].AsBoolean();
            AddressActual = cbor[69].AsString();
            SelectedAddressRegistration = new Address() 
            { 
                Id = cbor[21].AsInt32(),
                AddressFull = cbor[22].AsString(),
                Index = cbor[23].AsString(),
                Country = cbor[24].AsString(),
                FederalDistrict = cbor[25].AsString(),
                RegionKladrId = cbor[26].AsString(),
                RegionWthType = cbor[27].AsString(),
                RegionType = cbor[28].AsString(),
                RegionTypeFull = cbor[29].AsString(),
                Region = cbor[30].AsString(),
                AreaKladrId = cbor[31].AsString(),
                AreaWithType = cbor[32].AsString(),
                AreaType = cbor[33].AsString(),
                AreaTypeFull = cbor[34].AsString(),
                Area = cbor[35].AsString(),
                CityKladrId = cbor[36].AsString(),
                CityWithType = cbor[37].AsString(),
                CityType = cbor[38].AsString(),
                CityTypeFull = cbor[39].AsString(),
                City = cbor[40].AsString(),
                CityDistrictWithType = cbor[41].AsString(),
                CityDistrictType = cbor[42].AsString(),
                CityDistrictTypeFull = cbor[43].AsString(),
                CityDistrict = cbor[44].AsString(),
                SettlementKladrId = cbor[45].AsString(),
                SettlemenWithType = cbor[46].AsString(),
                SettlemenType = cbor[47].AsString(),
                SettlemenTypeFull = cbor[48].AsString(),
                Settlemen = cbor[49].AsString(),
                StreetKladrId = cbor[50].AsString(),
                StreetWithType = cbor[51].AsString(),
                StreetType = cbor[52].AsString(),
                StreetTypeFull = cbor[53].AsString(),
                Street = cbor[54].AsString(),
                HouseKladrId = cbor[55].AsString(),
                HouseType = cbor[56].AsString(),
                HouseTypeFull = cbor[57].AsString(),
                House = cbor[58].AsString(),
                BlockType = cbor[59].AsString(),
                BloctTypeFull = cbor[60].AsString(),
                Block = cbor[61].AsString(),
                Entrance = cbor[62].AsString(),
                Floor = cbor[63].AsString(),
                FlatType = cbor[64].AsString(),
                FlatTypeFull = cbor[65].AsString(),
                Flat = cbor[66].AsString()
            };
            SelectedAddressActual = new Address()
            {
                Id = cbor[68].AsInt32(),
                AddressFull = cbor[69].AsString(),
                Index = cbor[70].AsString(),
                Country = cbor[71].AsString(),
                FederalDistrict = cbor[72].AsString(),
                RegionKladrId = cbor[73].AsString(),
                RegionWthType = cbor[74].AsString(),
                RegionType = cbor[75].AsString(),
                RegionTypeFull = cbor[76].AsString(),
                Region = cbor[77].AsString(),
                AreaKladrId = cbor[78].AsString(),
                AreaWithType = cbor[79].AsString(),
                AreaType = cbor[80].AsString(),
                AreaTypeFull = cbor[81].AsString(),
                Area = cbor[82].AsString(),
                CityKladrId = cbor[83].AsString(),
                CityWithType = cbor[84].AsString(),
                CityType = cbor[85].AsString(),
                CityTypeFull = cbor[86].AsString(),
                City = cbor[87].AsString(),
                CityDistrictWithType = cbor[88].AsString(),
                CityDistrictType = cbor[89].AsString(),
                CityDistrictTypeFull = cbor[90].AsString(),
                CityDistrict = cbor[91].AsString(),
                SettlementKladrId = cbor[92].AsString(),
                SettlemenWithType = cbor[93].AsString(),
                SettlemenType = cbor[94].AsString(),
                SettlemenTypeFull = cbor[95].AsString(),
                Settlemen = cbor[96].AsString(),
                StreetKladrId = cbor[97].AsString(),
                StreetWithType = cbor[98].AsString(),
                StreetType = cbor[99].AsString(),
                StreetTypeFull = cbor[100].AsString(),
                Street = cbor[101].AsString(),
                HouseKladrId = cbor[102].AsString(),
                HouseType = cbor[103].AsString(),
                HouseTypeFull = cbor[104].AsString(),
                House = cbor[105].AsString(),
                BlockType = cbor[106].AsString(),
                BloctTypeFull = cbor[107].AsString(),
                Block = cbor[108].AsString(),
                Entrance = cbor[109].AsString(),
                Floor = cbor[110].AsString(),
                FlatType = cbor[111].AsString(),
                FlatTypeFull = cbor[112].AsString(),
                Flat = cbor[113].AsString()
            };
            SelectedOrganization = ToOrganization(); //Восстановление SelectedOrganization
        }
        public override byte[] GetCBOR() => ToCBOR(this).EncodeToBytes();
        public override void SetCBOR(byte[] b) => FromCBOR(CBORObject.DecodeFromBytes(b));
        #endregion CBOR
    }
}
