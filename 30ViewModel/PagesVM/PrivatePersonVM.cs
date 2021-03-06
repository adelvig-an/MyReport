﻿using _10Model;
using _10Model.Customer;
using _20DbLayer;
using PeterO.Cbor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace _30ViewModel.PagesVM
{
    public class PrivatePersonVM : PageViewModel
    {
        #region Properties (Нужны для валидации данных)
        private string secondName;
        private string firstName;
        private string middleName;
        private string serial;
        private string number;
        private string division;
        private DateTime? divisionDate = DateTime.Today;
        private string addressRegistration;
        private string addresActual;
        public int Id { get; set; }
        [Required(ErrorMessage = "Требуется указать Фамилию закачика")]
        [StringLength(20, ErrorMessage ="Привышение максимально допустимого количества символов")]
        public string SecondName { get => secondName;
            set { ValidateProperty(value); SetProperty(ref secondName, value); } }
        [Required(ErrorMessage = "Требуется указать Имя закачика")]
        [StringLength(20, ErrorMessage = "Привышение максимально допустимого количества символов")]
        public string FirstName { get => firstName;
            set { ValidateProperty(value); SetProperty(ref firstName, value); } }
        [Required(ErrorMessage = "Требуется указать Отчество закачика")]
        [StringLength(20, ErrorMessage = "Привышение максимально допустимого количества символов")]
        public string MiddleName { get => middleName;
            set { ValidateProperty(value); SetProperty(ref middleName, value); } }
        [Required(ErrorMessage = "Требуется указать серию паспорта")]
        [Range(0, int.MaxValue, ErrorMessage = "В серии пасорта только цифры")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Серия паспорта содержит 4 цифры")]
        public string Serial { get => serial;
            set { ValidateProperty(value); SetProperty(ref serial, value); } }
        [Required(ErrorMessage = "Требуется указать номер паспорта")]
        [Range(0, int.MaxValue, ErrorMessage = "В номере пасорта только цифры")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Номер паспорта содержит 6 цифр")]
        public string Number { get => number;
            set { ValidateProperty(value); SetProperty(ref number, value); } }
        [Required(ErrorMessage = "Требуется указать кем выдан паспорт")]
        public string Division { get => division;
            set { ValidateProperty(value); SetProperty(ref division, value); } }
        [Required(ErrorMessage = "Требуется указать дату выдачи паспорта")]
        public DateTime? DivisionDate { get => divisionDate;
            set { ValidateProperty(value); SetProperty(ref divisionDate, value); } }
        [Required(ErrorMessage = "Требуется указать адрес регистрации")]
        [StringLength(255, ErrorMessage = "Привышение максимально допустимого количества символов")]
        public string AddressRegistration { get => addressRegistration;
            set { ValidateProperty(value); SetProperty(ref addressRegistration, value); } }
        [Required(ErrorMessage = "Требуется указать адрес проживания")]
        [StringLength(255, ErrorMessage = "Привышение максимально допустимого количества символов")]
        public string AddressActual { get => addresActual;
            set { ValidateProperty(value); SetProperty(ref addresActual, value); } }
        #endregion Properties

        #region AddressMatch (Совпадение адресов регистрации и проживания)
        private bool isAddressMatch;
        public bool IsAddressMatch { get => isAddressMatch; 
            set { SetProperty(ref isAddressMatch, value);
                if (value == true)
                    ActualToRegistration();
                else
                    ActualToActual();
            }
        }
        public void ActualToRegistration() => AddressActual = AddressRegistration;
        public void ActualToActual() => AddressActual = "";
        #endregion AddressMatch

        private readonly ApplicationContext context;
        public PrivatePersonVM()
        {
            context = new ApplicationContext();
        }

        #region DataBase (Методы и свойства взаимодействующие с Базой данных)
        public PrivatePerson ToPrivatePerson()
        {
            var privatePerson = new PrivatePerson
            {
                Id = Id,
                FullName = SecondName + " " + FirstName + " " + MiddleName, 
                SecondName = SecondName,
                FirstName = FirstName,
                MiddleName = MiddleName,
                Serial = Serial,
                Number = Number,
                Division = Division,
                DivisionDate = DivisionDate,
                AddressRegistration = SelectedAddressRegistration,
                AddressActual = SelectedAddressActual
            };
            return privatePerson;
        }
        public void AddPrivatePerson()
        {
            try
            {
                PrivatePerson privatePerson = ToPrivatePerson();
                context.Add(privatePerson);
                context.SaveChanges();
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
            }
        }
        public bool UpdatePrivatePerson()
        {
            try
            {
                var privatePerson = context.PrivatePeople.First();
                privatePerson = ToPrivatePerson();
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

        #region AutoCompleteAddress (Подсказки заполнения адреса)
        private Address selectedAddressRegistration;
        private Address selectedAddressActual;
        public Address SelectedAddressRegistration { get => selectedAddressRegistration;
            set { if (SetProperty(ref selectedAddressRegistration, value))
                    FillAddressRegistration(selectedAddressRegistration); } }
        public Address SelectedAddressActual { get => selectedAddressActual; 
            set { if (SetProperty(ref selectedAddressActual, value))
                    FillAddressActual(selectedAddressActual); } }
        public void FillAddressRegistration(Address address) => AddressRegistration = address?.AddressFull;
        public void FillAddressActual(Address address) => AddressActual = address?.AddressFull;
        #endregion AutoCompleteAddress

        #region CBOR
        static CBORObject ToCBOR(PrivatePersonVM privatePersonVM)
        {
            return CBORObject.NewArray()
                .Add(privatePersonVM.Id)
                .Add(privatePersonVM.SecondName)
                .Add(privatePersonVM.FirstName)
                .Add(privatePersonVM.MiddleName)
                .Add(privatePersonVM.Serial)
                .Add(privatePersonVM.Number)
                .Add(privatePersonVM.Division)
                .Add(privatePersonVM.DivisionDate.HasValue
                ? CBORObject.NewArray().Add(true).Add(privatePersonVM.DivisionDate.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(privatePersonVM.AddressRegistration)
                .Add(privatePersonVM.IsAddressMatch)
                .Add(privatePersonVM.AddressActual);
        }
        void FromCBOR(CBORObject cbor)
        {
            Id = cbor[0].AsInt32();
            SecondName = cbor[1].AsString();
            FirstName = cbor[2].AsString();
            MiddleName = cbor[3].AsString();
            Serial = cbor[4].AsString();
            Number = cbor[5].AsString();
            Division = cbor[6].AsString();
            DivisionDate = cbor[7][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[7][1].ToObject<long>()))
            : null;
            AddressRegistration = cbor[8].AsString();
            IsAddressMatch = cbor[9].AsBoolean();
            AddressActual = cbor[10].AsString();
        }
        public override byte[] GetCBOR() => ToCBOR(this).EncodeToBytes();
        public override void SetCBOR(byte[] b) => FromCBOR(CBORObject.DecodeFromBytes(b));
        #endregion CBOR
    }
}
