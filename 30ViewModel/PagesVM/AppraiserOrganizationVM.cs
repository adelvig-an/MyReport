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
                AppraiserOrganization appraiserOrg = ToAppraiserOrganization();
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
            if (context.Appraisers.Any(a => a.Id == appraiserOrganizationVM.Id))
                UpdateAppraiserOrganization();
            else
                AddAppraiserOrganization();
        }
        public AppraiserOrganizationVM LoadAppraiserOrganization()
        {
            try
            {
                var appraiserOrganization = context.AppraiserOrganizations.Single(a => a.Id == 1);
                context.Entry(appraiserOrganization).Reference(d => d.Director)
                    .Load();
                context.Entry(appraiserOrganization)
                    .Reference(ip => ip.InsurancePolicie)
                    .Load();
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
                NameFullOpf = appraiserOrganization?.NameFullOpf,
                NameShortOpf = appraiserOrganization?.NameShortOpf,
                NameFull = appraiserOrganization?.NameFull,
                NameShort = appraiserOrganization?.NameShort,
                FullOpf = appraiserOrganization?.FullOpf,
                ShortOpf = appraiserOrganization?.ShortOpf,
                Ogrn = appraiserOrganization?.Ogrn,
                OgrnDate = appraiserOrganization?.OgrnDate,
                Inn = appraiserOrganization?.Inn,
                Kpp = appraiserOrganization?.Kpp,
                Bank = appraiserOrganization?.Bank,
                Bik = appraiserOrganization?.Bik,
                PayAccount = appraiserOrganization?.PayAccount,
                CorrAccount = appraiserOrganization?.CorrAccount,
                FullName = appraiserOrganization?.Director?.FullName,
                Position = appraiserOrganization?.Director?.Position,
                PowerOfAttorney = appraiserOrganization.Director.PowerOfAttorney,
                PowerOfAttorneyNumber = appraiserOrganization?.Director?.PowerOfAttorneyNumber,
                PowerOfAttorneyDate = appraiserOrganization?.Director?.PowerOfAttorneyDate,
                PowerOfAttorneyDateBefore = appraiserOrganization?.Director?.PowerOfAttorneyDateBefore,
                InsuranceNumber = appraiserOrganization?.InsurancePolicie?.Number,
                InsuranceCompany = appraiserOrganization?.InsurancePolicie?.InsuranceCompany,
                InsuranceMoney = appraiserOrganization.InsurancePolicie.InsuranceMoney,
                InsuranceDateFrom = appraiserOrganization?.InsurancePolicie?.DateFrom,
                InsuranceDateBefore = appraiserOrganization?.InsurancePolicie?.DateBefore,
                PathInsurancePolicieCollection = JsonConvert.DeserializeObject<ObservableCollection<string>>(appraiserOrganization?.InsurancePolicie?.PathInsurancePolicieImage),
                AddressRegistration = appraiserOrganization?.AddressRegistration?.AddressFull,
                AddressActual = appraiserOrganization?.AddressActual?.AddressFull
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
            InsuranceNumber = cbor[114].AsString();
            InsuranceCompany = cbor[115].AsString();
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
                        var pipi = cbor.AsString();
                        return pipi;
                    }));
            SelectedOrganization = ToOrganization(); //Восстановление SelectedOrganization
        }
        public override byte[] GetCBOR() => ToCBOR(this).EncodeToBytes();
        public override void SetCBOR(byte[] b) => FromCBOR(CBORObject.DecodeFromBytes(b));
        #endregion CBOR
    }
}
