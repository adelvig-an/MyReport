using _10Model;
using _20DbLayer;
using PeterO.Cbor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.Win32;
using System.Windows.Input;
using System.IO;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

////Валидация данных, проверка на корректность внесеных данных пользователем +
///////////////////////////////////////////////////////////////////////////////////////////////////
////Временное сохранение в CBOR +
////Восстановление данных из CBOR +
///////////////////////////////////////////////////////////////////////////////////////////////////
////Постоянное сохранение в БД +
////Редактирование данных из БД +
////Восстановление данных из БД
///////////////////////////////////////////////////////////////////////////////////////////////////
////Добавление изображений для InsurancePolicie +
////Создание папки для изображений InsurancePolicie
////Отображение наименования добавленного файла изображения для InsurancePolicie +
////Открытие изображения в диалоговом окне по нажатию на название
////Удаление изображений для InsurancePolicie +
///////////////////////////////////////////////////////////////////////////////////////////////////
////Добавление изображений для Appraiser.PathSroCertificateImage +
////Создание папки для изображений Appraiser.PathSroCertificateImage 
////Отображение наименования добавленного файла изображения для Appraiser.PathSroCertificateImage +
////Открытие изображения в диалоговом окне по нажатию на название 
////Удаление изображений для Appraiser.PathSroCertificateImage +
///////////////////////////////////////////////////////////////////////////////////////////////////
////Добавление изображений для Appraiser.PatnDiplomImage +
////Создание папки для изображений Appraiser.PathSroCertificateImage 
////Отображение наименования добавленного файла изображения для Appraiser.PatnDiplomImage +
////Открытие изображения в диалоговом окне по нажатию на название 
////Удаение изображений для Appraiser.PatnDiplomImage +
///////////////////////////////////////////////////////////////////////////////////////////////////
////Добавление полей QualificationCertificate (не более 3-х) +
////Удаление полей QualificationCertificate +
////Добавление изображений для QualificationCertificate +
////Создание папки для изображений Appraiser.PathSroCertificateImage 
////Отображение наименования добавленного файла изображения для QualificationCertificate +
////Открытие изображения в диалоговом окне по нажатию на название
////Удаление изображений для QualificationCertificate
///////////////////////////////////////////////////////////////////////////////////////////////////
////Расчет стажа работы от даты начала оценочной деятельности +
///////////////////////////////////////////////////////////////////////////////////////////////////
////Правила создания директории для хранения создаваемых/загружаемых файлов: data/IdUser/NameClass or IdEntity/NameProperty/...

namespace _30ViewModel.PagesVM
{
    public class AppraiserVM : PageViewModel
    {
        #region Properties (Нужны для валидации данных)
        //Свойства Оценщика
        private string secondName;
        private string firstName;
        private string middleName;
        private string email;
        private string phone;
        private DateTime? startedDate;
        private string specialization;
        private string number;
        private DateTime? diplomDate;
        private string universety;
        private string pathDiplomImage;
        private string sro;
        private int sroNumber;
        private DateTime? sroDate;
        private string pathSroCertificateImage;
        //Свойства Страхового полиса
        private string insuranceNumber;
        private string insuranceCompany;
        private decimal insuranceMoney;
        private DateTime? insuranceDateFrom;
        private DateTime? insuranceDateBefore;
        private string pathInsurancePolicieImage;
        //Свойства Квалиффикационного аттестата
        private string pathQualificationCertificateImage;
        //Свойства Оценщика
        public int Id { get; set; }
        public string SecondName { get => secondName;
            set { ValidateProperty(value); SetProperty(ref secondName, value); } }
        public string FirstName { get => firstName;
            set { ValidateProperty(value); SetProperty(ref firstName, value); } }
        public string MiddleName { get => middleName;
            set { ValidateProperty(value); SetProperty(ref middleName, value); } }
        public string Email { get => email;
            set { ValidateProperty(value); SetProperty(ref email, value); } }
        public string Phone { get=>phone;
            set { ValidateProperty(value); SetProperty(ref phone, value); } }
        public DateTime? StartedDate { get => startedDate;
            set { ValidateProperty(value); SetProperty(ref startedDate, value); ExperienceResult(); } }
        public string Specialization { get => specialization;
            set { ValidateProperty(value); SetProperty(ref specialization, value); } }
        public string Number { get => number;
            set { ValidateProperty(value); SetProperty(ref number, value); } }
        public DateTime? DiplomDate { get => diplomDate;
            set { ValidateProperty(value); SetProperty(ref diplomDate, value); } }
        public string Universety { get => universety;
            set { ValidateProperty(value); SetProperty(ref universety, value); } }
        public string PatnDiplomImage { get => pathDiplomImage;
            set { ValidateProperty(value); SetProperty(ref pathDiplomImage, value); } }
        public string Sro { get => sro;
            set { ValidateProperty(value); SetProperty(ref sro, value); } }
        public int SroNumber { get => sroNumber;
            set { ValidateProperty(value); SetProperty(ref sroNumber, value); } }
        public DateTime? SroDate { get => sroDate;
            set { ValidateProperty(value); SetProperty(ref sroDate, value); } }
        public string PathSroCertificateImage { get => pathSroCertificateImage;
            set { ValidateProperty(value); SetProperty(ref pathSroCertificateImage, value); } }
        //Свойства Страхового полиса
        public string InsuranceNumber { get => insuranceNumber;
            set { ValidateProperty(value); SetProperty(ref insuranceNumber, value); } }
        public string InsuranceCompany { get=>insuranceCompany;
            set { ValidateProperty(value); SetProperty(ref insuranceCompany, value); } }
        public decimal InsuranceMoney { get => insuranceMoney;
            set { ValidateProperty(value); SetProperty(ref insuranceMoney, value); } }
        public DateTime? InsuranceDateFrom { get => insuranceDateFrom;
            set { ValidateProperty(value); SetProperty(ref insuranceDateFrom, value); ToInsuranceDateBefore(); } }
        public DateTime? InsuranceDateBefore { get => insuranceDateBefore;
            set { ValidateProperty(value); SetProperty(ref insuranceDateBefore, value); } }
        public string PathInsurancePolicieImage { get => pathInsurancePolicieImage;
            set { ValidateProperty(value); SetProperty(ref pathInsurancePolicieImage, value); } }
        //Свойства Квалиффикационного аттестата размещенны в своей ViewModel
        public string PathQualificationCertificateImage { get => pathQualificationCertificateImage;
            set { ValidateProperty(value); SetProperty(ref pathQualificationCertificateImage, value); } }
        #endregion Properties
        public void ToInsuranceDateBefore()
        {
            InsuranceDateBefore = InsuranceDateFrom?.AddDays(-1).AddYears(+1);
        }
        public ObservableCollection<QualificationCertificateVM> Certificates { get; private set; }
        public ObservableCollection<string> PathInsurancePolicieCollection { get; set; }
        public ObservableCollection<string> PathSroCertificateCollection { get; set; }
        public ObservableCollection<string> PathDiplomCollection { get; set; }
        public ObservableCollection<SelfRegulatingOrganization> SelfRegulatingOrganizations { get; set; }

        private readonly ApplicationContext context;
        public AppraiserVM()
        {
            context = new ApplicationContext();
            
            context.SRO.Load();
            SelfRegulatingOrganizations = new ObservableCollection<SelfRegulatingOrganization>(context.SRO.Local.ToBindingList());

            Certificates = new ObservableCollection<QualificationCertificateVM>()
            { new QualificationCertificateVM() };

            PathInsurancePolicieCollection = new ObservableCollection<string>();
            PathSroCertificateCollection = new ObservableCollection<string>();
            PathDiplomCollection = new ObservableCollection<string>();

            AddCertificateCommand = new RelayCommand(_ => AddCertificate());
            RemoveCertificateCommand = new RelayCommand(certificate => RemoveCertificate(certificate as QualificationCertificateVM));

            AddInsurancePolicieImageCommand = new RelayCommand(_ => AddInsurancePolicieImage());
            RemoveInsurancePolicieImageCommand = new RelayCommand(p => RemoveInsurancePolicieImage(p.ToString()));
            AddSroCertificateImageCommand = new RelayCommand(_ => AddSroCertificateImage());
            RemoveSroCertificateImageCommand = new RelayCommand(p => RemoveSroCertificateImage(p.ToString()));
            AddDiplomImageCommand = new RelayCommand(_ => AddDiplomImage());
            RemoveDiplomImageCommand = new RelayCommand(p => RemoveDiplomImage(p.ToString()));
        }

        #region Добавление Кваллификационного сетификата
        public void AddCertificate()
        {
            if (Certificates.Count < 3)
            {
                Certificates.Add(new QualificationCertificateVM());
            }
        }
        public void RemoveCertificate(QualificationCertificateVM certificate)
        {
            Certificates.Remove(certificate);
            DeleteQualificationCertificate(certificate.Id);
        }
        public ICommand AddCertificateCommand { get; }
        public ICommand RemoveCertificateCommand { get; }
        #endregion Добавление Кваллификационного сетификата

        #region Добавление и удаление файла изображения
        //Страховой полис
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

        //Свидетельство СРО
        public ICommand AddSroCertificateImageCommand { get; }
        public void AddSroCertificateImage()
        {
            var path = GetAndCopyImage.CopyImage();
            if (path != null)
            {
                PathSroCertificateCollection.Add(path);
            }
        }
        public ICommand RemoveSroCertificateImageCommand { get; }
        public void RemoveSroCertificateImage(string s)
        {
            PathSroCertificateCollection.Remove(s);
            File.Delete(s);
        }

        //Диплом
        public ICommand AddDiplomImageCommand { get; }
        public void AddDiplomImage()
        {
            var path = GetAndCopyImage.CopyImage();
            if (path != null)
            {
                PathDiplomCollection.Add(path);
            }
        }
        public ICommand RemoveDiplomImageCommand { get; }
        public void RemoveDiplomImage(string s)
        {
            PathDiplomCollection.Remove(s);
            File.Delete(s);
        }
        #endregion Добавление и удаление файла изображения

        #region Расчет стажа работы от даты начала оценочной деательности
        private int? experience;
        public int? Experience
        {
            get => experience;
            set { SetProperty(ref experience, value); }
        }
        public void ExperienceResult()
        {
            Experience = DateTime.Now.Year - StartedDate?.Year;
            if (DateTime.Now.Month < StartedDate?.Month ||
                (DateTime.Now.Month == StartedDate?.Month && DateTime.Now.Day < StartedDate?.Day)) Experience--;
        }
        #endregion Расчет стажа работы

        #region DataBase (Методы и свойства взаимодействующие с Базой данных)
        public Appraiser ToAppraiser()
        {
            var appraiser = new Appraiser
            {
                Id = Id,
                SecondName = SecondName,
                FirstName = FirstName,
                MiddleName = MiddleName,
                FullName = SecondName + " " + FirstName + " " + MiddleName,
                Email = Email,
                Phone = Phone,
                StartedDate = StartedDate,
                Specialization = Specialization,
                Number = Number,
                DiplomDate = DiplomDate,
                PathDiplomImage = JsonConvert.SerializeObject(PathDiplomCollection),
                Universety = Universety,
                //Sro = Sro,
                SroNumber = SroNumber,
                SroDate = SroDate,
                PathSroCertificateImage = JsonConvert.SerializeObject(PathInsurancePolicieCollection),
                //SelfRegulatingOrganizations = ToSRO(),
                InsurancePolicie = ToInsurancePolicie(),
                QualificationCertificates = new ObservableCollection<QualificationCertificate>(Certificates
                    .Select(cvm => cvm.ToQualificationCertificate()))
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
                DateBefore = InsuranceDateBefore,
                PathInsurancePolicieImage = JsonConvert.SerializeObject(PathInsurancePolicieCollection)
            };
            return insurancePolicie;
        }
        public SelfRegulatingOrganization ToSRO()
        {
            var sro = new SelfRegulatingOrganization
            {
                Id = Id,
                NameFull = Sro
            };
            return sro;
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
        public bool UpdateAppraiser()
        {
            try
            {
                Appraiser appraiser = ToAppraiser();
                context.Update(appraiser);
                context.SaveChanges();
                return true;
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                return false;
            }
        }
        public void AddOrUpdateAppraiser(AppraiserVM appraiserVM)
        {
            if (context.Appraisers.Any(a => a.Id == appraiserVM.Id))
                UpdateAppraiser();
            else
                AddAppraiser();
        }
        public AppraiserVM LoadAppraiser()
        {
            try
            {
                var appraiser = context.Appraisers.Single(a => a.Id == 3);
                context.Entry(appraiser)
                    .Reference(ip => ip.InsurancePolicie)
                    .Load();
                context.Entry(appraiser)
                    .Collection(qc => qc.QualificationCertificates)
                    .Load();
                return GetAppraiserVM(appraiser);
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                return new AppraiserVM();
            }
        }
        public static AppraiserVM GetAppraiserVM(Appraiser appraiser)
        {
            var appraiserVM = new AppraiserVM()
            {
                Id = appraiser.Id,
                SecondName = appraiser.SecondName,
                FirstName = appraiser.FirstName,
                MiddleName = appraiser.MiddleName,
                Email = appraiser.Email,
                Phone = appraiser.Phone,
                StartedDate = appraiser.StartedDate,
                Specialization = appraiser.Specialization,
                Number = appraiser.Number,
                DiplomDate = appraiser.DiplomDate,
                Universety = appraiser.Universety,
                PathDiplomCollection = JsonConvert.DeserializeObject<ObservableCollection<string>>(appraiser.PathDiplomImage),
                //Sro = appraiser.SelfRegulatingOrganizations.Sro,
                SroNumber = appraiser.SroNumber,
                SroDate = appraiser.SroDate,
                PathSroCertificateCollection = JsonConvert.DeserializeObject<ObservableCollection<string>>(appraiser.PathSroCertificateImage),
                InsuranceNumber = appraiser.InsurancePolicie.Number,
                InsuranceCompany = appraiser.InsurancePolicie.InsuranceCompany,
                InsuranceMoney = appraiser.InsurancePolicie.InsuranceMoney,
                InsuranceDateFrom = appraiser.InsurancePolicie.DateFrom,
                InsuranceDateBefore = appraiser.InsurancePolicie.DateBefore,
                PathInsurancePolicieCollection = JsonConvert.DeserializeObject<ObservableCollection<string>>(appraiser.InsurancePolicie.PathInsurancePolicieImage),
                Certificates = new ObservableCollection<QualificationCertificateVM>(appraiser
                    .QualificationCertificates.Select(qc =>
                        QualificationCertificateVM.GetQualificationCertificateVM(qc)))
            };
            return appraiserVM;
        }
        public bool DeleteQualificationCertificate(int i)
        {
            try
            {
                var qual = context.QualificationCertificates.Single(c => c.Id == i);
                context.QualificationCertificates.Remove(qual);
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
        private static CBORObject ToCBOR(AppraiserVM appraiserVM)
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
                .Add(CBORObject.FromObject(appraiserVM.PathInsurancePolicieCollection
                    .Select(pip => CBORObject.FromObject(pip)).ToArray()
                    )
                )
                .Add(CBORObject.FromObject(appraiserVM.PathSroCertificateCollection
                    .Select(pip => CBORObject.FromObject(pip)).ToArray()
                    )
                )
                .Add(CBORObject.FromObject(appraiserVM.PathDiplomCollection
                    .Select(pip => CBORObject.FromObject(pip)).ToArray()
                    )
                )
                .Add(CBORObject.FromObject(appraiserVM.Certificates
                    .Select(cvm => CBORObject.DecodeFromBytes(cvm.GetCBOR()))
                    .ToArray()
                    )
                );
        }
        private void FromCBOR(CBORObject cbor)
        {
            Id = cbor[0].AsInt32();
            SecondName = cbor[1].AsStringSafe();
            FirstName = cbor[2].AsStringSafe();
            MiddleName = cbor[3].AsStringSafe();
            StartedDate = cbor[4][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[4][1].ToObject<long>()))
            : null;
            Specialization = cbor[5].AsStringSafe();
            Number = cbor[6].AsStringSafe();
            DiplomDate = cbor[7][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[7][1].ToObject<long>()))
            : null;
            Universety = cbor[8].AsStringSafe();
            Sro = cbor[9].AsStringSafe();
            SroNumber = cbor[10].AsInt32();
            SroDate = cbor[11][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[11][1].ToObject<long>()))
            : null;
            InsuranceNumber = cbor[12].AsStringSafe();
            InsuranceCompany = cbor[13].AsStringSafe();
            InsuranceMoney = cbor[14].ToObject<decimal>();
            InsuranceDateFrom = cbor[15][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[15][1].ToObject<long>()))
            : null;
            InsuranceDateBefore = cbor[16][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[16][1].ToObject<long>()))
            : null;
            PathInsurancePolicieCollection = new ObservableCollection<string>(
                cbor[17].Values.Select(cbor =>
                {
                    var pipi = cbor.AsStringSafe();
                    return pipi;
                }));
            PathSroCertificateCollection = new ObservableCollection<string>(
                cbor[18].Values.Select(cbor =>
                {
                    var pipi = cbor.AsStringSafe();
                    return pipi;
                }));
            PathDiplomCollection = new ObservableCollection<string>(
                cbor[19].Values.Select(cbor =>
                {
                    var pipi = cbor.AsStringSafe();
                    return pipi;
                }));
            Certificates = new ObservableCollection<QualificationCertificateVM>(
                cbor[20].Values.Select(cbor =>
                {
                    var qcvm = new QualificationCertificateVM();
                    qcvm.SetCBOR(cbor.EncodeToBytes());
                    return qcvm;
                }
                ));
            if (Certificates.Count == 0)
            {
                Certificates.Add(new QualificationCertificateVM());
            }
        }
        public override byte[] GetCBOR() => ToCBOR(this).EncodeToBytes();
        public override void SetCBOR(byte[] b) => FromCBOR(CBORObject.DecodeFromBytes(b));
        #endregion CBOR
    }
}
