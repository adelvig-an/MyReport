﻿using _10Model;
using _10Model.Customer;
using _20DbLayer;
using System;
using System.Diagnostics;
using System.Linq;

namespace _30ViewModel.PagesVM
{
    public class OrganizationVM : PageViewModel
    {
        #region Properties (Нужны для валидации данных)
        //Свойства для организации
        private string nameShortOpf;
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
        //Свойства для организации
        public string NameShortOpf { get => nameShortOpf;
            set { ValidateProperty(value); SetProperty(ref nameShortOpf, value); } }
        public string Ogrn { get => ogrn;
            set { ValidateProperty(value); SetProperty(ref ogrn, value); } }
        public DateTime? OgrnDate { get => ogrnDate;
            set { ValidateProperty(value); SetProperty(ref ogrnDate, value); } }
        public string Inn { get => inn;
            set { ValidateProperty(value); SetProperty(ref inn, value); } }
        public string Kpp { get => kpp;
        set { ValidateProperty(value); SetProperty(ref kpp, value); } }
        public string Bank { get => bank; 
            set { ValidateProperty(value); SetProperty(ref bank, value); } }
        public string Bik { get => bik; 
            set { ValidateProperty(value); SetProperty(ref bik, value); } }
        public string PayAccount { get => payAccount;
            set { ValidateProperty(value); SetProperty(ref payAccount, value); } }
        public string CorrAccount { get => corrAccount; 
            set { ValidateProperty(value); SetProperty(ref corrAccount, value); } }
        //Свойства для директора
        public string FullName { get => fullName;
            set { ValidateProperty(value); SetProperty(ref fullName, value); } }
        public string Position { get => position;
            set { ValidateProperty(value); SetProperty(ref position, value); } }
        public PowerOfAttorneyType PowerOfAttorney { get => powerOfAttorney;
            set { ValidateProperty(value); SetProperty(ref powerOfAttorney, value); ToVisibl(); } }
        public string PowerOfAttorneyNumber { get => powerOfAttorneyNumber;
            set { ValidateProperty(value); SetProperty(ref powerOfAttorneyNumber, value); } }
        public DateTime? PowerOfAttorneyDate { get => powerOfAttorneyDate;
            set { ValidateProperty(value); SetProperty(ref powerOfAttorneyDate, value); } }
        public DateTime? PowerOfAttorneyDateBefore { get => powerOfAttorneyDateBefore;
            set { ValidateProperty(value); SetProperty(ref powerOfAttorneyDateBefore, value); } }
        public string AddressRegistration { get => addressRegistration;
            set { ValidateProperty(value); SetProperty(ref addressRegistration, value); } }
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
        public void ActualToRegistration() => AddressActual = AddressRegistration;
        public void ActualToActual() => AddressActual = null;
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
        public bool ToVisibl()
        {
            if (PowerOfAttorney == PowerOfAttorneyType.Attorney)
                return IsAttorneyValue = true;
            else
                return IsAttorneyValue = false;
        }
        #endregion

        #region DataBase (Методы и свойства взаимодействующие с Базой данных)
        public Organization ToOrganization()
        {
            var organization = new Organization
            {
                Id = Id,
                NameFullOpf = SelectedOrganization.NameFullOpf,
                NameShortOpf = NameShortOpf,
                Opf = SelectedOrganization.Opf,
                Ogrn = Ogrn,
                OgrnDate = OgrnDate,
                Inn = Inn,
                Kpp = Kpp,
                Bank = Bank,
                Bik = Bik,
                PayAccount = PayAccount,
                CorrAccount = CorrAccount,
                Director = ToDirector(),
                AddressRegistration = SelectedAddressRegistration ?? SelectedOrganization.AddressRegistration,
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
            Inn = organization?.Inn.ToString();
            Kpp = organization?.Kpp?.ToString();
            Ogrn = organization?.Ogrn.ToString();
            OgrnDate = organization?.OgrnDate;
            FullName = organization?.Director.FullName;
            Position = organization?.Director.Position;
            AddressRegistration = organization?.AddressRegistration.AddressFull;
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
        public override byte[] GetCBOR()
        {
            throw new NotImplementedException();
        }

        public override void SetCBOR(byte[] b)
        {
            throw new NotImplementedException();
        }
        #endregion CBOR
    }
}