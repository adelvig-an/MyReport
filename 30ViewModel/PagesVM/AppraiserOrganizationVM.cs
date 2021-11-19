using _10Model;
using _10Model.Customer;
using _20DbLayer;
using Microsoft.EntityFrameworkCore;
using PeterO.Cbor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows.Input;
using Newtonsoft.Json;

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
        private string pathInsurancePolicieImage;
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
        public string PathInsurancePolicieImage 
        { 
            get => pathInsurancePolicieImage; 
            set { ValidateProperty(value); SetProperty(ref pathInsurancePolicieImage, value); } 
        }
        #endregion Properties
        public void ToInsuranceDateBefore()
        {
            InsuranceDateBefore = InsuranceDateFrom?.AddDays(-1).AddYears(+1);
        }

        private readonly ApplicationContext context;
        public ObservableCollection<Appraiser> Appraisers { get; set; }
        public ObservableCollection<string> PathInsurancePolicieCollection { get; set; }
        public AppraiserOrganizationVM()
        {
            context = new ApplicationContext();

            Appraisers = new ObservableCollection<Appraiser>(context.Appraisers.Include(a => a.AppraiserOrganization).ToList());
            PathInsurancePolicieCollection = new ObservableCollection<string>();
            AddInsurancePolicieImageCommand = new RelayCommand(_ => AddInsurancePolicieImage());
            RemoveInsurancePolicieImageCommand = new RelayCommand(p => RemoveInsurancePolicieImage(p.ToString()));
        }

        #region Добавление и удаление файла
        public ICommand AddInsurancePolicieImageCommand { get; }
        public void AddInsurancePolicieImage()
        {
            var path = GetAndCopyImage.CopyImage();
            if (path != null)
            {
                PathInsurancePolicieCollection.Add(path);
            }
        }
        public ICommand RemoveInsurancePolicieImageCommand { get; }
        public void RemoveInsurancePolicieImage(string s)
        {
            PathInsurancePolicieCollection.Remove(s);
            File.Delete(s);
        }
        #endregion Добавление и удаление файла

        #region DataBase (Методы и свойства взаимодействующие с Базой данных)
        public AppraiserOrganization ToAppraiserOrganization()
        {
            var appraiserOrganization = new AppraiserOrganization
            {
                Id = Id,
                NameFullOpf = NameFullOpf,
                NameShortOpf = NameShortOpf,
                NameFull = NameFull,
                NameShort = NameShort,
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
                InsurancePolicie = ToInsurancePolicie(),
                AddressRegistration = SelectedAddressRegistration ?? SelectedOrganization?.AddressRegistration,
                AddressActual = SelectedAddressActual
            };
            return appraiserOrganization;
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
                DateBefore = InsuranceDateBefore,
                PathInsurancePolicieImage = JsonConvert.SerializeObject(PathInsurancePolicieCollection)
            };
            return police;
        }
        public void AddAppraiserOrganization()
        {
            try
            {
                var appraiserOrg = ToAppraiserOrganization();
                context.Add(appraiserOrg);
                context.SaveChanges();
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
            }
        }
        public bool UpdateAppraiserOrganization()
        {
            try
            {
                var appraiserOrganization = ToAppraiserOrganization();
                context.Update(appraiserOrganization);
                context.SaveChanges();
                return true;
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                return false;
            }
        }
        public void AddOrUpdateAppraiserOrganization(AppraiserOrganizationVM appraiserOrganizationVM)
        {
            if (context.AppraiserOrganizations.Any(ao => ao.Id == appraiserOrganizationVM.Id))
                UpdateAppraiserOrganization();
            else
                AddAppraiserOrganization();
        }
        public AppraiserOrganizationVM LoadAppraiserOrganization()
        {
            try
            {
                var appraiserOrganization = context.AppraiserOrganizations.Single(ao => ao.Id == 1);
                context.Entry(appraiserOrganization).Reference(d => d.Director)
                    .Load();
                context.Entry(appraiserOrganization)
                    .Reference(ip => ip.InsurancePolicie)
                    .Load();
                context.Entry(appraiserOrganization)
                    .Reference(ar => ar.AddressRegistration)
                    .Load();
                context.Entry(appraiserOrganization)
                    .Reference(ac => ac.AddressActual)
                    .Load();
                //context.Entry(appraiserOrganization)
                //    .Reference(a => a.Appraisers)
                //    .Load();
                return GetAppraiserOrganizationVM(appraiserOrganization);
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                return new AppraiserOrganizationVM();
            }
        }
        public static AppraiserOrganizationVM GetAppraiserOrganizationVM(AppraiserOrganization appraiserOrganization)
        {
            var appraiserOrganizationVM = new AppraiserOrganizationVM()
            {
                Id = appraiserOrganization.Id,
                SelectedOrganization = appraiserOrganization,
                Bik = appraiserOrganization?.Bik,
                Bank = appraiserOrganization?.Bank,
                PayAccount = appraiserOrganization?.PayAccount,
                CorrAccount = appraiserOrganization?.CorrAccount,
                InsuranceNumber = appraiserOrganization?.InsurancePolicie?.Number,
                InsuranceCompany = appraiserOrganization?.InsurancePolicie?.InsuranceCompany,
                InsuranceMoney = appraiserOrganization.InsurancePolicie.InsuranceMoney,
                InsuranceDateFrom = appraiserOrganization?.InsurancePolicie?.DateFrom,
                InsuranceDateBefore = appraiserOrganization?.InsurancePolicie?.DateBefore,
                PathInsurancePolicieCollection = JsonConvert.DeserializeObject<ObservableCollection<string>>(appraiserOrganization?.InsurancePolicie?.PathInsurancePolicieImage),
                SelectedAddressRegistration = appraiserOrganization?.AddressRegistration,
                SelectedAddressActual = appraiserOrganization?.AddressActual
            };
            return appraiserOrganizationVM;
        }
        #endregion DataBase

        #region CBOR
        static CBORObject ToCBOR(AppraiserOrganizationVM appraiserOrgVM)
        {
            return CBORObject.NewArray()
                .Add(appraiserOrgVM.Id)
                .Add(appraiserOrgVM.NameFullOpf)
                .Add(appraiserOrgVM.NameShortOpf)
                .Add(appraiserOrgVM.NameFull)
                .Add(appraiserOrgVM.NameShort)
                .Add(appraiserOrgVM.FullOpf)
                .Add(appraiserOrgVM.ShortOpf)
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
                .Add(appraiserOrgVM.SelectedAddressRegistration?.Id)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.AddressFull)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.Index)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.Country)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.FederalDistrict)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.RegionKladrId)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.RegionWthType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.RegionType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.RegionTypeFull)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.Region)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.AreaKladrId)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.AreaWithType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.AreaType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.AreaTypeFull)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.Area)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.CityKladrId)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.CityWithType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.CityType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.CityTypeFull)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.City)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.CityDistrictWithType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.CityDistrictType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.CityDistrictTypeFull)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.CityDistrict)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.SettlementKladrId)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.SettlemenWithType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.SettlemenType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.SettlemenTypeFull)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.Settlemen)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.StreetKladrId)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.StreetWithType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.StreetType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.StreetTypeFull)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.Street)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.HouseKladrId)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.HouseType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.HouseTypeFull)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.House)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.BlockType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.BloctTypeFull)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.Block)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.Entrance)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.Floor)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.FlatType)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.FlatTypeFull)
                .Add(appraiserOrgVM.SelectedAddressRegistration?.Flat)
                .Add(appraiserOrgVM.IsAddressMatch)
                .Add(appraiserOrgVM.SelectedAddressActual?.Id)
                .Add(appraiserOrgVM.SelectedAddressActual?.AddressFull)
                .Add(appraiserOrgVM.SelectedAddressActual?.Index)
                .Add(appraiserOrgVM.SelectedAddressActual?.Country)
                .Add(appraiserOrgVM.SelectedAddressActual?.FederalDistrict)
                .Add(appraiserOrgVM.SelectedAddressActual?.RegionKladrId)
                .Add(appraiserOrgVM.SelectedAddressActual?.RegionWthType)
                .Add(appraiserOrgVM.SelectedAddressActual?.RegionType)
                .Add(appraiserOrgVM.SelectedAddressActual?.RegionTypeFull)
                .Add(appraiserOrgVM.SelectedAddressActual?.Region)
                .Add(appraiserOrgVM.SelectedAddressActual?.AreaKladrId)
                .Add(appraiserOrgVM.SelectedAddressActual?.AreaWithType)
                .Add(appraiserOrgVM.SelectedAddressActual?.AreaType)
                .Add(appraiserOrgVM.SelectedAddressActual?.AreaTypeFull)
                .Add(appraiserOrgVM.SelectedAddressActual?.Area)
                .Add(appraiserOrgVM.SelectedAddressActual?.CityKladrId)
                .Add(appraiserOrgVM.SelectedAddressActual?.CityWithType)
                .Add(appraiserOrgVM.SelectedAddressActual?.CityType)
                .Add(appraiserOrgVM.SelectedAddressActual?.CityTypeFull)
                .Add(appraiserOrgVM.SelectedAddressActual?.City)
                .Add(appraiserOrgVM.SelectedAddressActual?.CityDistrictWithType)
                .Add(appraiserOrgVM.SelectedAddressActual?.CityDistrictType)
                .Add(appraiserOrgVM.SelectedAddressActual?.CityDistrictTypeFull)
                .Add(appraiserOrgVM.SelectedAddressActual?.CityDistrict)
                .Add(appraiserOrgVM.SelectedAddressActual?.SettlementKladrId)
                .Add(appraiserOrgVM.SelectedAddressActual?.SettlemenWithType)
                .Add(appraiserOrgVM.SelectedAddressActual?.SettlemenType)
                .Add(appraiserOrgVM.SelectedAddressActual?.SettlemenTypeFull)
                .Add(appraiserOrgVM.SelectedAddressActual?.Settlemen)
                .Add(appraiserOrgVM.SelectedAddressActual?.StreetKladrId)
                .Add(appraiserOrgVM.SelectedAddressActual?.StreetWithType)
                .Add(appraiserOrgVM.SelectedAddressActual?.StreetType)
                .Add(appraiserOrgVM.SelectedAddressActual?.StreetTypeFull)
                .Add(appraiserOrgVM.SelectedAddressActual?.Street)
                .Add(appraiserOrgVM.SelectedAddressActual?.HouseKladrId)
                .Add(appraiserOrgVM.SelectedAddressActual?.HouseType)
                .Add(appraiserOrgVM.SelectedAddressActual?.HouseTypeFull)
                .Add(appraiserOrgVM.SelectedAddressActual?.House)
                .Add(appraiserOrgVM.SelectedAddressActual?.BlockType)
                .Add(appraiserOrgVM.SelectedAddressActual?.BloctTypeFull)
                .Add(appraiserOrgVM.SelectedAddressActual?.Block)
                .Add(appraiserOrgVM.SelectedAddressActual?.Entrance)
                .Add(appraiserOrgVM.SelectedAddressActual?.Floor)
                .Add(appraiserOrgVM.SelectedAddressActual?.FlatType)
                .Add(appraiserOrgVM.SelectedAddressActual?.FlatTypeFull)
                .Add(appraiserOrgVM.SelectedAddressActual?.Flat)
                .Add(appraiserOrgVM.InsuranceNumber)
                .Add(appraiserOrgVM.InsuranceCompany)
                .Add(appraiserOrgVM.InsuranceMoney)
                .Add(appraiserOrgVM.InsuranceDateFrom.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserOrgVM.InsuranceDateFrom.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(appraiserOrgVM.InsuranceDateBefore.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserOrgVM.InsuranceDateBefore.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(CBORObject.FromObject(appraiserOrgVM.PathInsurancePolicieCollection
                    .Select(pip => CBORObject.FromObject(pip)).ToArray()
                    )
                );
        }
        void FromCBOR(CBORObject cbor)
        {
            Id = cbor[0].AsInt32();
            NameFullOpf = cbor[1].AsStringSafe();
            NameShortOpf = cbor[2].AsStringSafe();
            NameFull = cbor[3].AsStringSafe();
            NameShort = cbor[4].AsStringSafe();
            FullOpf = cbor[5].AsStringSafe();
            ShortOpf = cbor[6].AsStringSafe();
            Ogrn = cbor[7].AsStringSafe();
            OgrnDate = cbor[8][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[8][1].ToObject<long>()))
            : null;
            Inn = cbor[9].AsStringSafe();
            Kpp = cbor[10].AsStringSafe();
            Bank = cbor[11].AsStringSafe();
            Bik = cbor[12].AsStringSafe();
            PayAccount = cbor[13].AsStringSafe();
            CorrAccount = cbor[14].AsStringSafe();
            FullName = cbor[15].AsStringSafe();
            Position = cbor[16].AsStringSafe();
            PowerOfAttorney = (PowerOfAttorneyType)Enum.Parse(typeof(PowerOfAttorneyType), cbor[17].ToString(), true);
            PowerOfAttorneyNumber = cbor[18].AsStringSafe();
            PowerOfAttorneyDate = cbor[19][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[19][1].ToObject<long>()))
            : null;
            PowerOfAttorneyDateBefore = cbor[20][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[20][1].ToObject<long>()))
            : null;
            AddressRegistration = cbor[22].AsStringSafe();
            IsAddressMatch = cbor[67].AsBoolean();
            AddressActual = cbor[69].AsStringSafe();
            InsuranceNumber = cbor[114].AsStringSafe();
            InsuranceCompany = cbor[115].AsStringSafe();
            InsuranceMoney = cbor[116].ToObject<decimal>();
            InsuranceDateFrom = cbor[117][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[117][1].ToObject<long>()))
            : null;
            InsuranceDateBefore = cbor[118][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[118][1].ToObject<long>()))
            : null;
            PathInsurancePolicieCollection = new ObservableCollection<string>(
                cbor[119].Values.Select(cbor =>
                {
                    var pipi = cbor.AsStringSafe();
                    return pipi;
                }));
            SelectedAddressRegistration = new Address()
            {
                Id = cbor[21].AsInt32(),
                AddressFull = cbor[22].AsStringSafe(),
                Index = cbor[23].AsStringSafe(),
                Country = cbor[24].AsStringSafe(),
                FederalDistrict = cbor[25].AsStringSafe(),
                RegionKladrId = cbor[26].AsStringSafe(),
                RegionWthType = cbor[27].AsStringSafe(),
                RegionType = cbor[28].AsStringSafe(),
                RegionTypeFull = cbor[29].AsStringSafe(),
                Region = cbor[30].AsStringSafe(),
                AreaKladrId = cbor[31].AsStringSafe(),
                AreaWithType = cbor[32].AsStringSafe(),
                AreaType = cbor[33].AsStringSafe(),
                AreaTypeFull = cbor[34].AsStringSafe(),
                Area = cbor[35].AsStringSafe(),
                CityKladrId = cbor[36].AsStringSafe(),
                CityWithType = cbor[37].AsStringSafe(),
                CityType = cbor[38].AsStringSafe(),
                CityTypeFull = cbor[39].AsStringSafe(),
                City = cbor[40].AsStringSafe(),
                CityDistrictWithType = cbor[41].AsStringSafe(),
                CityDistrictType = cbor[42].AsStringSafe(),
                CityDistrictTypeFull = cbor[43].AsStringSafe(),
                CityDistrict = cbor[44].AsStringSafe(),
                SettlementKladrId = cbor[45].AsStringSafe(),
                SettlemenWithType = cbor[46].AsStringSafe(),
                SettlemenType = cbor[47].AsStringSafe(),
                SettlemenTypeFull = cbor[48].AsStringSafe(),
                Settlemen = cbor[49].AsStringSafe(),
                StreetKladrId = cbor[50].AsStringSafe(),
                StreetWithType = cbor[51].AsStringSafe(),
                StreetType = cbor[52].AsStringSafe(),
                StreetTypeFull = cbor[53].AsStringSafe(),
                Street = cbor[54].AsStringSafe(),
                HouseKladrId = cbor[55].AsStringSafe(),
                HouseType = cbor[56].AsStringSafe(),
                HouseTypeFull = cbor[57].AsStringSafe(),
                House = cbor[58].AsStringSafe(),
                BlockType = cbor[59].AsStringSafe(),
                BloctTypeFull = cbor[60].AsStringSafe(),
                Block = cbor[61].AsStringSafe(),
                Entrance = cbor[62].AsStringSafe(),
                Floor = cbor[63].AsStringSafe(),
                FlatType = cbor[64].AsStringSafe(),
                FlatTypeFull = cbor[65].AsStringSafe(),
                Flat = cbor[66].AsStringSafe()
            };
            SelectedAddressActual = new Address()
            {
                Id = cbor[68].AsInt32(),
                AddressFull = cbor[69].AsStringSafe(),
                Index = cbor[70].AsStringSafe(),
                Country = cbor[71].AsStringSafe(),
                FederalDistrict = cbor[72].AsStringSafe(),
                RegionKladrId = cbor[73].AsStringSafe(),
                RegionWthType = cbor[74].AsStringSafe(),
                RegionType = cbor[75].AsStringSafe(),
                RegionTypeFull = cbor[76].AsStringSafe(),
                Region = cbor[77].AsStringSafe(),
                AreaKladrId = cbor[78].AsStringSafe(),
                AreaWithType = cbor[79].AsStringSafe(),
                AreaType = cbor[80].AsStringSafe(),
                AreaTypeFull = cbor[81].AsStringSafe(),
                Area = cbor[82].AsStringSafe(),
                CityKladrId = cbor[83].AsStringSafe(),
                CityWithType = cbor[84].AsStringSafe(),
                CityType = cbor[85].AsStringSafe(),
                CityTypeFull = cbor[86].AsStringSafe(),
                City = cbor[87].AsStringSafe(),
                CityDistrictWithType = cbor[88].AsStringSafe(),
                CityDistrictType = cbor[89].AsStringSafe(),
                CityDistrictTypeFull = cbor[90].AsStringSafe(),
                CityDistrict = cbor[91].AsStringSafe(),
                SettlementKladrId = cbor[92].AsStringSafe(),
                SettlemenWithType = cbor[93].AsStringSafe(),
                SettlemenType = cbor[94].AsStringSafe(),
                SettlemenTypeFull = cbor[95].AsStringSafe(),
                Settlemen = cbor[96].AsStringSafe(),
                StreetKladrId = cbor[97].AsStringSafe(),
                StreetWithType = cbor[98].AsStringSafe(),
                StreetType = cbor[99].AsStringSafe(),
                StreetTypeFull = cbor[100].AsStringSafe(),
                Street = cbor[101].AsStringSafe(),
                HouseKladrId = cbor[102].AsStringSafe(),
                HouseType = cbor[103].AsStringSafe(),
                HouseTypeFull = cbor[104].AsStringSafe(),
                House = cbor[105].AsStringSafe(),
                BlockType = cbor[106].AsStringSafe(),
                BloctTypeFull = cbor[107].AsStringSafe(),
                Block = cbor[108].AsStringSafe(),
                Entrance = cbor[109].AsStringSafe(),
                Floor = cbor[110].AsStringSafe(),
                FlatType = cbor[111].AsStringSafe(),
                FlatTypeFull = cbor[112].AsStringSafe(),
                Flat = cbor[113].AsStringSafe()
            };
            SelectedOrganization = ToOrganization(); //Восстановление SelectedOrganization
        }
        public override byte[] GetCBOR() => ToCBOR(this).EncodeToBytes();
        public override void SetCBOR(byte[] b) => FromCBOR(CBORObject.DecodeFromBytes(b));
        #endregion CBOR
    }
}
