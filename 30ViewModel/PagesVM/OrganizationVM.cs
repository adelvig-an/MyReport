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
            AddressRegistration = organization?.AddressRegistration?.AddressFull;
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
                .Add(organizationVM.AddressRegistration)
                .Add(organizationVM.IsAddressMatch)
                .Add(organizationVM.AddressActual);
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
            AddressRegistration = cbor[17].AsString();
            IsAddressMatch = cbor[18].AsBoolean();
            AddressActual = cbor[19].AsString();
            SelectedAddressRegistration = new Address()
            { AddressFull = AddressRegistration };
            SelectedAddressActual = new Address()
            { AddressFull = AddressActual };
            SelectedOrganization = ToOrganization(); //Восстановление SelectedOrganization
        }
        public override byte[] GetCBOR() => ToCBOR(this).EncodeToBytes();
        public override void SetCBOR(byte[] b) => FromCBOR(CBORObject.DecodeFromBytes(b));
        #endregion CBOR
    }
}
