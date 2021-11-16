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
                Id = appraiserOrganization.Id,
                NameFullOpf = appraiserOrganization.NameFullOpf,
                NameShortOpf = appraiserOrganization.NameShortOpf,
                Opf = appraiserOrganization.Opf,
                Ogrn = appraiserOrganization.Ogrn,
                OgrnDate = appraiserOrganization.OgrnDate,
                Inn = appraiserOrganization.Inn,
                Kpp = appraiserOrganization.Kpp,
                Bank = appraiserOrganization.Bank,
                Bik = appraiserOrganization.Bik,
                PayAccount = appraiserOrganization.PayAccount,
                CorrAccount = appraiserOrganization.CorrAccount,
                FullName = appraiserOrganization.Director.FullName,
                Position = appraiserOrganization.Director.Position,
                PowerOfAttorney = appraiserOrganization.Director.PowerOfAttorney,
                PowerOfAttorneyNumber = appraiserOrganization.Director.PowerOfAttorneyNumber,
                PowerOfAttorneyDate = appraiserOrganization.Director.PowerOfAttorneyDate,
                PowerOfAttorneyDateBefore = appraiserOrganization.Director.PowerOfAttorneyDateBefore,
                InsuranceNumber = appraiserOrganization.InsurancePolicie.Number,
                InsuranceCompany = appraiserOrganization.InsurancePolicie.InsuranceCompany,
                InsuranceMoney = appraiserOrganization.InsurancePolicie.InsuranceMoney,
                InsuranceDateFrom = appraiserOrganization.InsurancePolicie.DateFrom,
                InsuranceDateBefore = appraiserOrganization.InsurancePolicie.DateBefore,
                PathInsurancePolicieCollection = JsonConvert.DeserializeObject<ObservableCollection<string>>(appraiserOrganization.InsurancePolicie.PathInsurancePolicieImage),
                AddressRegistration = appraiserOrganization.AddressRegistration.AddressFull,
                AddressActual = appraiserOrganization.AddressActual.AddressFull
            };
            return appraiserOrganizationVM;
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
            NameShortOpf = cbor[1].AsStringSafe();
            Opf = cbor[2].AsStringSafe();
            Ogrn = cbor[3].AsStringSafe();
            OgrnDate = cbor[4][0].AsBoolean()
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
            PowerOfAttorneyDate = cbor[15][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[15][1].ToObject<long>()))
            : null;
            PowerOfAttorneyDateBefore = cbor[16][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[16][1].ToObject<long>()))
            : null;
            AddressRegistration = cbor[18].AsStringSafe();
            IsAddressMatch = cbor[63].AsBoolean();
            AddressActual = cbor[65].AsStringSafe();
            InsuranceNumber = cbor[110].AsStringSafe();
            InsuranceCompany = cbor[111].AsStringSafe();
            InsuranceMoney = cbor[112].ToObject<decimal>();
            InsuranceDateFrom = cbor[113][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[113][1].ToObject<long>()))
            : null;
            InsuranceDateBefore = cbor[114][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[114][1].ToObject<long>()))
            : null;
            PathInsurancePolicieCollection = new ObservableCollection<string>(
                cbor[115].Values.Select(cbor =>
                    {
                        var pipi = cbor.AsStringSafe();
                        return pipi;
                    }));
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
