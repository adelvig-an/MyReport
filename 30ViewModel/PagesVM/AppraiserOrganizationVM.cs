using _10Model;
using _20DbLayer;
using Microsoft.EntityFrameworkCore;
using PeterO.Cbor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace _30ViewModel.PagesVM
{
    public class AppraiserOrganizationVM : OrganizationVM
    {
        #region Properties (Нужны для валидации данных)
        //Свойства Страхового полиса
        private string insuranceNumber;
        private string insuranceCompany;
        private decimal insuranceMoney;
        private DateTime? insuranceDateFrom;
        private DateTime? insuranceDateBefore;
        //Свойства Страхового полиса
        [Required(ErrorMessage = "Требуется указать номер страхового полиса")]
        public string InsuranceNumber
        {
            get => insuranceNumber;
            set { ValidateProperty(value); SetProperty(ref insuranceNumber, value); }
        }
        [Required(ErrorMessage = "Требуется указать название страховщика")]
        public string InsuranceCompany
        {
            get => insuranceCompany;
            set { ValidateProperty(value); SetProperty(ref insuranceCompany, value); }
        }
        [Required(ErrorMessage = "Требуется указать сумму страхового возмещения")]
        public decimal InsuranceMoney
        {
            get => insuranceMoney;
            set { ValidateProperty(value); SetProperty(ref insuranceMoney, value); }
        }
        [Required(ErrorMessage = "Требуется указать дату начала действия страхового полиса")]
        public DateTime? InsuranceDateFrom
        {
            get => insuranceDateFrom;
            set { ValidateProperty(value); SetProperty(ref insuranceDateFrom, value); ToInsuranceDateBefore(); }
        }
        [Required(ErrorMessage = "Требуется указать дату окончания действия страхового полиса")]
        public DateTime? InsuranceDateBefore
        {
            get => insuranceDateBefore;
            set { ValidateProperty(value); SetProperty(ref insuranceDateBefore, value); }
        }
        #endregion Properties
        public void ToInsuranceDateBefore()
        {
            InsuranceDateBefore = InsuranceDateFrom?.AddDays(-1).AddYears(+1);
        }

        private readonly ApplicationContext context;
        public ObservableCollection<Appraiser> Appraisers { get; set; }
        public AppraiserOrganizationVM()
        {
            context = new ApplicationContext();
            Appraisers = new ObservableCollection<Appraiser>
            {
                new Appraiser { FullName = "Дельвиг Антон Денисович" },
                new Appraiser { FullName = "Шестаов Денис Александрович" },
                new Appraiser { FullName = "Рошка Андрей Ильевич" }
            };
        }

        #region DataBase (Методы и свойства взаимодействующие с Базой данных)
        public AppraiserOrganization ToAppraiserOrg()
        {
            var appraiserOgr = new AppraiserOrganization
            {
                Id = Id,
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
                InsurancePolicie = ToInsurancePolicie(),
                AddressRegistration = SelectedAddressRegistration ?? SelectedOrganization.AddressRegistration,
                AddressActual = SelectedAddressActual
            };
            return appraiserOgr;
        }
        public InsurancePolicie ToInsurancePolicie()
        {
            var police = new InsurancePolicie
            {
                Id = Id,
                Number = InsuranceNumber,
                InsuranceCompany = InsuranceCompany,
                InsuranceMoney = InsuranceMoney,
                DateFrom = InsuranceDateFrom,
                DateBefore = InsuranceDateBefore

            };
            return police;
        }
        public void AddAppraiserOrganization()
        {
            try
            {
                AppraiserOrganization appraiserOrg = ToAppraiserOrg();
                context.Add(appraiserOrg);
                context.SaveChanges();
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
            }
        }
        public bool UpdateAppraiserOrgganization()
        {
            try
            {
                var appraiser = context.AppraiserOrganizations.First();
                appraiser = ToAppraiserOrg();
                context.SaveChanges();
                return true;
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                return false;
            }
        }
        public void ReadAppraiser()
        {
           context.Appraisers.Include(appraiser => appraiser.AppraiserOrganization).ToList();
        }
        #endregion DataBase

        #region CBOR
        static CBORObject ToCBOR(AppraiserOrganizationVM appraiserOrgVM)
        {
            return CBORObject.NewArray()
                .Add(appraiserOrgVM.Id)
                .Add(appraiserOrgVM.NameShortOpf)
                .Add(appraiserOrgVM.Opf)
                .Add(appraiserOrgVM.Ogrn)
                .Add(appraiserOrgVM.OgrnDate.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserOrgVM.OgrnDate.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(appraiserOrgVM.Inn)
                .Add(appraiserOrgVM.Kpp)
                .Add(appraiserOrgVM.Bank)
                .Add(appraiserOrgVM.Bik)
                .Add(appraiserOrgVM.PayAccount)
                .Add(appraiserOrgVM.CorrAccount)
                .Add(appraiserOrgVM.FullName)
                .Add(appraiserOrgVM.Position)
                .Add(appraiserOrgVM.PowerOfAttorney)
                .Add(appraiserOrgVM.PowerOfAttorneyNumber)
                .Add(appraiserOrgVM.PowerOfAttorneyDate.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserOrgVM.PowerOfAttorneyDate.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(appraiserOrgVM.PowerOfAttorneyDateBefore.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserOrgVM.PowerOfAttorneyDateBefore.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(appraiserOrgVM.AddressRegistration)
                .Add(appraiserOrgVM.IsAddressMatch)
                .Add(appraiserOrgVM.AddressActual)
                .Add(appraiserOrgVM.InsuranceNumber)
                .Add(appraiserOrgVM.InsuranceCompany)
                .Add(appraiserOrgVM.InsuranceMoney)
                .Add(appraiserOrgVM.InsuranceDateFrom.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserOrgVM.InsuranceDateFrom.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(appraiserOrgVM.InsuranceDateBefore.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserOrgVM.InsuranceDateBefore.Value.ToBinary())
                : CBORObject.NewArray().Add(false));
        }
        void FromCBOR(CBORObject cbor)
        {
            Id = cbor[0].AsInt32();
            NameShortOpf = cbor[1].AsStringSafe();
            Opf = cbor[2].AsStringSafe();
            Ogrn = cbor[3].AsStringSafe();
            OgrnDate = cbor[4].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[4][1].ToObject<long>()))
            : null;
            Inn = cbor[5].AsStringSafe();
            Kpp = cbor[6].AsStringSafe();
            Bank = cbor[7].AsStringSafe();
            Bik = cbor[8].AsStringSafe();
            PayAccount = cbor[9].AsStringSafe();
            CorrAccount = cbor[10].AsStringSafe();
            FullName = cbor[11].AsStringSafe();
            Position = cbor[12].AsStringSafe();
            PowerOfAttorney = (PowerOfAttorneyType)Enum.Parse(typeof(PowerOfAttorneyType), cbor[13].ToString(), true);
            PowerOfAttorneyNumber = cbor[14].AsStringSafe();
            PowerOfAttorneyDate = cbor[15].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[15][1].ToObject<long>()))
            : null;
            PowerOfAttorneyDateBefore = cbor[16].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[16][1].ToObject<long>()))
            : null;
            AddressRegistration = cbor[17].AsStringSafe();
            IsAddressMatch = cbor[18].AsBoolean();
            AddressActual = cbor[19].AsStringSafe();
            InsuranceNumber = cbor[20].AsStringSafe();
            InsuranceCompany = cbor[21].AsStringSafe();
            InsuranceMoney = cbor[22].AsDecimal();
            InsuranceDateFrom = cbor[23].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[23][1].ToObject<long>()))
            : null;
            InsuranceDateBefore = cbor[24].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[24][1].ToObject<long>()))
            : null;
        }
        public override byte[] GetCBOR() => ToCBOR(this).EncodeToBytes();
        public override void SetCBOR(byte[] b) => FromCBOR(CBORObject.DecodeFromBytes(b));
        #endregion CBOR
    }
}
