using _10Model;
using _20DbLayer;
using PeterO.Cbor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace _30ViewModel.PagesVM
{
    public class AppraiserVM : PageViewModel
    {
        #region Properties (Нужны для валидации данных)
        //Свойства Оценщика
        private string secondName;
        private string firstName;
        private string middleName;
        private DateTime? startedDate;
        private string specialization;
        private string number;
        private DateTime? diplomDate;
        private string universety;
        private string sro;
        private int sroNumber;
        private DateTime? sroDate;
        //Свойства Страхового полиса
        private string insuranceNumber;
        private string insuranceCompany;
        private decimal insuranceMoney;
        private DateTime? insuranceDateFrom;
        private DateTime? insuranceDateBefore;
        //Свойства Квалификационного аттестата
        private int certificateNumber;
        private DateTime? certificateDateFrom;
        private DateTime? certificateDateBefore;
        private SpecialityType speciality;
        //Свойства Оценщика
        public int Id { get; set; }
        public string SecondName { get => secondName;
            set { ValidateProperty(value); SetProperty(ref secondName, value); } }
        public string FirstName { get => firstName;
            set { ValidateProperty(value); SetProperty(ref firstName, value); } }
        public string MiddleName { get => middleName;
            set { ValidateProperty(value); SetProperty(ref middleName, value); } }
        public DateTime? StartedDate { get => startedDate;
            set { ValidateProperty(value); SetProperty(ref startedDate, value); } }
        public string Specialization { get => specialization;
            set { ValidateProperty(value); SetProperty(ref specialization, value); } }
        public string Number { get => number;
            set { ValidateProperty(value); SetProperty(ref number, value); } }
        public DateTime? DiplomDate { get => diplomDate;
            set { ValidateProperty(value); SetProperty(ref diplomDate, value); } }
        public string Universety { get => universety;
            set { ValidateProperty(value); SetProperty(ref universety, value); } }
        public string Sro { get => sro;
            set { ValidateProperty(value); SetProperty(ref sro, value); } }
        public int SroNumber { get => sroNumber;
            set { ValidateProperty(value); SetProperty(ref sroNumber, value); } }
        public DateTime? SroDate { get => sroDate;
            set { ValidateProperty(value); SetProperty(ref sroDate, value); } }
        //Свойства Страхового полиса
        public string InsuranceNumber { get => insuranceNumber;
            set { ValidateProperty(value); SetProperty(ref insuranceNumber, value); } }
        public string InsuranceCompany { get=>insuranceCompany;
            set { ValidateProperty(value); SetProperty(ref insuranceCompany, value); } }
        public decimal InsuranceMoney { get => insuranceMoney;
            set { ValidateProperty(value); SetProperty(ref insuranceMoney, value); } }
        public DateTime? InsuranceDateFrom { get => insuranceDateFrom;
            set { ValidateProperty(value); SetProperty(ref insuranceDateFrom, value); } }
        public DateTime? InsuranceDateBefore { get => insuranceDateBefore;
            set { ValidateProperty(value); SetProperty(ref insuranceDateBefore, value); } }
        //Свойства Квалификационного аттестата
        public int CertificateNumber { get => certificateNumber;
            set { ValidateProperty(value); SetProperty(ref certificateNumber, value); } }
        public DateTime? CertificateDateFrom { get => certificateDateFrom;
            set { ValidateProperty(value); SetProperty(ref certificateDateFrom, value); } }
        public DateTime? CertificateDateBefore { get => certificateDateBefore;
            set { ValidateProperty(value); SetProperty(ref certificateDateBefore, value); } }
        public SpecialityType Speciality { get => speciality;
            set { ValidateProperty(value); SetProperty(ref speciality, value); } }
        #endregion Properties
        
        private readonly ApplicationContext context;
        public AppraiserVM()
        {
            context = new ApplicationContext();
            Certificates = new ObservableCollection<QualificationCertificate>();
            AddCommand = new RelayCommand(_ => AddCertificate());
            RemoveCommand = new RelayCommand(certificate => RemoveCertificate(certificate as QualificationCertificate));
        }
        public void AddCertificate()
        {
            if (Certificates.Count < 3)
            {
                Certificates.Add(new QualificationCertificate());
            }
        }
        public void RemoveCertificate(QualificationCertificate certificate)
        {
            Certificates.Remove(certificate);
        }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ObservableCollection<QualificationCertificate> Certificates { get; private set; }

        #region DataBase (Методы и свойства взаимодействующие с Базой данных)
        public Appraiser ToAppraiser()
        {
            var appraiser = new Appraiser
            {
                Id = Id,
                SecondName = SecondName,
                FirstName = FirstName,
                MiddleName = MiddleName,
                StartedDate = StartedDate,
                Specialization = Specialization,
                Number = Number,
                DiplomDate = DiplomDate,
                Universety = Universety,
                Sro = Sro,
                SroNumber = SroNumber,
                SroDate = SroDate,
                InsurancePolicie = ToInsurancePolicie(),
            };
            return appraiser;
        }
        public InsurancePolicie ToInsurancePolicie()
        {
            var insurancePolicie = new InsurancePolicie
            {
                Id = Id,
                InsuranceCompany = InsuranceCompany,
                Number = InsuranceNumber,
                InsuranceMoney = InsuranceMoney,
                DateFrom = InsuranceDateFrom,
                DateBefore = InsuranceDateBefore
            };
            return insurancePolicie;
        }
        public QualificationCertificate ToCertificate()
        {
            var qualificationCertificate = new QualificationCertificate
            {
                Id = Id,
                Number = CertificateNumber,
                DateFrom = CertificateDateFrom,
                DateBefore = CertificateDateBefore,
                Speciality = Speciality
            };
            return qualificationCertificate;
        }
        public void AddAppraiser()
        {
            try
            {
                Appraiser appraiser = ToAppraiser();
                context.Add(appraiser);
                context.SaveChanges();
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
            }
        }
        public bool UpdateContract()
        {
            try
            {
                var appraiser = context.Appraisers.First();
                appraiser = ToAppraiser();
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

        #region CBOR
        static CBORObject ToCBOR(AppraiserVM appraiserVM)
        {
            return CBORObject.NewArray()
                .Add(appraiserVM.Id)
                .Add(appraiserVM.SecondName)
                .Add(appraiserVM.FirstName)
                .Add(appraiserVM.MiddleName)
                .Add(appraiserVM.StartedDate.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserVM.StartedDate.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(appraiserVM.Specialization)
                .Add(appraiserVM.Number)
                .Add(appraiserVM.DiplomDate.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserVM.DiplomDate.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(appraiserVM.Universety)
                .Add(appraiserVM.Sro)
                .Add(appraiserVM.SroNumber)
                .Add(appraiserVM.SroDate.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserVM.SroDate.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(appraiserVM.InsuranceNumber)
                .Add(appraiserVM.InsuranceCompany)
                .Add(appraiserVM.InsuranceMoney)
                .Add(appraiserVM.InsuranceDateFrom.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserVM.InsuranceDateFrom.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(appraiserVM.InsuranceDateBefore.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserVM.InsuranceDateBefore.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(appraiserVM.CertificateNumber)
                .Add(appraiserVM.CertificateDateFrom.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserVM.CertificateDateFrom.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(appraiserVM.CertificateDateBefore.HasValue
                ? CBORObject.NewArray().Add(true).Add(appraiserVM.CertificateDateBefore.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(appraiserVM.Speciality);
        }
        void FromCBOR(CBORObject cbor)
        {
            Id = cbor[0].AsInt32();
            SecondName = cbor[1].AsString();
            FirstName = cbor[2].AsString();
            MiddleName = cbor[3].AsString();
            StartedDate = cbor[4][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[4][1].ToObject<long>()))
            : null;
            Specialization = cbor[5].AsString();
            Number = cbor[6].AsString();
            DiplomDate = cbor[7][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[7][1].ToObject<long>()))
            : null;
            Universety = cbor[8].AsString();
            Sro = cbor[9].AsString();
            SroNumber = cbor[10].AsInt32();
            SroDate = cbor[11][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[11][1].ToObject<long>()))
            : null;
            InsuranceNumber = cbor[12].AsString();
            InsuranceCompany = cbor[13].AsString();
            InsuranceMoney = cbor[14].AsDecimal();
            InsuranceDateFrom = cbor[15][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[15][1].ToObject<long>()))
            : null;
            InsuranceDateBefore = cbor[16][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[16][1].ToObject<long>()))
            : null;
            CertificateNumber = cbor[17].AsInt32();
            CertificateDateFrom = cbor[18][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[18][1].ToObject<long>()))
            : null;
            CertificateDateBefore = cbor[19][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[19][1].ToObject<long>()))
            : null;
        }
        public override byte[] GetCBOR() => ToCBOR(this).EncodeToBytes();
        public override void SetCBOR(byte[] b) => FromCBOR(CBORObject.DecodeFromBytes(b));
        #endregion CBOR
    }
}
