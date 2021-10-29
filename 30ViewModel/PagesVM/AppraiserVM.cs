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
////Добавление изображений для Appraiser.PathSroCertificateImage
////Создание папки для изображений Appraiser.PathSroCertificateImage
////Отображение наименования добавленного файла изображения для Appraiser.PathSroCertificateImage
////Открытие изображения в диалоговом окне по нажатию на название
////Удаление изображений для Appraiser.PathSroCertificateImage
///////////////////////////////////////////////////////////////////////////////////////////////////
////Добавление изображений для Appraiser.PatnDiplomImage
////Создание папки для изображений Appraiser.PathSroCertificateImage
////Отображение наименования добавленного файла изображения для Appraiser.PatnDiplomImage
////Открытие изображения в диалоговом окне по нажатию на название
////Удаение изображений для Appraiser.PatnDiplomImage
///////////////////////////////////////////////////////////////////////////////////////////////////
////Добавление полей QualificationCertificate (не более 3-х) +
////Удаление полей QualificationCertificate +
////Добавление изображений для QualificationCertificate
////Создание папки для изображений Appraiser.PathSroCertificateImage
////Отображение наименования добавленного файла изображения для QualificationCertificate
////Открытие изображения в диалоговом окне по нажатию на название
////Удаление изображений для QualificationCertificate
///////////////////////////////////////////////////////////////////////////////////////////////////
////Расчет стажа работы от даты начала оценочной деятельности +
///////////////////////////////////////////////////////////////////////////////////////////////////
////Правила создания директории для хранениия создаваемых/загружаемых файлов: data/IdUser/NameClass or IdEntity/NameProperty/...

namespace _30ViewModel.PagesVM
{
    public class AppraiserVM : PageViewModel
    {
        #region Properties (Нужны для валидации данных)
        //Свойства Оценщика
        private string secondName = "Фамилия";
        private string firstName = "Имя";
        private string middleName = "Отчество";
        private string email = "e-mail@mail.ru";
        private string phone = "+7 (999) 654-97-35";
        private DateTime? startedDate = DateTime.Today;
        private string specialization = "Оценка бизнеса";
        private string number = "54 АЕ 000346";
        private DateTime? diplomDate = DateTime.Today;
        private string universety = "ЧОУ ВО «Сибирская академия финансов и банковского дела»";
        private string pathDiplomImage;
        private string sro = "НП СРО «Деловой Союз Оценщиков»";
        private int sroNumber = 1069;
        private DateTime? sroDate = DateTime.Today;
        private string pathSroCertificateImage;
        //Свойства Страхового полиса
        private string insuranceNumber = "009-073-006214/21";
        private string insuranceCompany = "ООО «Абсолют Страхование»";
        private decimal insuranceMoney = 300000;
        private DateTime? insuranceDateFrom = DateTime.Today;
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
        public ObservableCollection<string> PathQualificationCertificateCollection { get; set; }
        private readonly ApplicationContext context;
        public AppraiserVM()
        {
            context = new ApplicationContext();
            Certificates = new ObservableCollection<QualificationCertificateVM>();
            PathInsurancePolicieCollection = new ObservableCollection<string>();
            PathSroCertificateCollection = new ObservableCollection<string>();
            PathDiplomCollection = new ObservableCollection<string>();
            PathQualificationCertificateCollection = new ObservableCollection<string>();
            AddCommand = new RelayCommand(_ => AddCertificate());
            AddInsurancePolicieImageCommand = new RelayCommand(_ => AddInsurancePolicieImage());
            RemoveInsurancePolicieImageCommand = new RelayCommand(p => RemoveInsurancePolicieImage(p.ToString()));
            RemoveCommand = new RelayCommand(certificate => RemoveCertificate(certificate as QualificationCertificateVM));
            if (Certificates.Count == 0)
            {
                Certificates.Add(new QualificationCertificateVM());
            }
        }
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
        }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }

        #region Добавление и удаление файла
        public ICommand AddInsurancePolicieImageCommand { get; }
        public void AddInsurancePolicieImage()
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "Пользовательские файлы (*.jfif; *.pjpeg; *.jpeg; *.pjp; *.tiff; *.bmp; *.jpg; *.png) |*.jfif; *.pjpeg; *.jpeg; *.pjp; *.tiff; *.bmp; *.jpg; *.png";
            if (true == OpenFileDialog.ShowDialog())
            {
                string filePath = OpenFileDialog.FileName;
                string extension = Path.GetExtension(filePath);
                if (extension == ".jfif")
                {
                    PathInsurancePolicieImage = Path.GetRandomFileName() + ".jfif";
                    File.Copy(filePath, PathInsurancePolicieImage);
                    PathInsurancePolicieCollection.Add(PathInsurancePolicieImage);
                }
                else if (extension == ".pjpeg")
                {
                    PathInsurancePolicieImage = Path.GetRandomFileName() + ".pjpeg";
                    File.Copy(filePath, PathInsurancePolicieImage);
                    PathInsurancePolicieCollection.Add(PathInsurancePolicieImage);

                }
                else if (extension == ".jpeg")
                {
                    PathInsurancePolicieImage = Path.GetRandomFileName() + ".jpeg";
                    File.Copy(filePath, PathInsurancePolicieImage);
                    PathInsurancePolicieCollection.Add(PathInsurancePolicieImage);

                }
                else if (extension == ".pjp")
                {
                    PathInsurancePolicieImage = Path.GetRandomFileName() + ".pjp";
                    File.Copy(filePath, PathInsurancePolicieImage);
                    PathInsurancePolicieCollection.Add(PathInsurancePolicieImage);

                }
                else if (extension == ".tiff")
                {
                    PathInsurancePolicieImage = Path.GetRandomFileName() + ".tiff";
                    File.Copy(filePath, PathInsurancePolicieImage);
                    PathInsurancePolicieCollection.Add(PathInsurancePolicieImage);

                }
                else if (extension == ".bmp")
                {
                    PathInsurancePolicieImage = Path.GetRandomFileName() + ".bmp";
                    File.Copy(filePath, PathInsurancePolicieImage);
                    PathInsurancePolicieCollection.Add(PathInsurancePolicieImage);

                }
                else if (extension == ".jpg")
                {
                    PathInsurancePolicieImage = Path.GetRandomFileName() + ".jpg";
                    File.Copy(filePath, PathInsurancePolicieImage);
                    PathInsurancePolicieCollection.Add(PathInsurancePolicieImage);

                }
                else if (extension == ".png")
                {
                    PathInsurancePolicieImage = Path.GetRandomFileName() + ".png";
                    File.Copy(filePath, PathInsurancePolicieImage);
                    PathInsurancePolicieCollection.Add(PathInsurancePolicieImage);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
        public ICommand RemoveInsurancePolicieImageCommand { get; }
        public void RemoveInsurancePolicieImage(string s)
        {
            PathInsurancePolicieCollection.Remove(s);
            File.Delete(s);
        }
        #endregion Добавление и удаление файла

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
                StartedDate = StartedDate,
                Specialization = Specialization,
                Number = Number,
                DiplomDate = DiplomDate,
                Universety = Universety,
                Sro = Sro,
                SroNumber = SroNumber,
                SroDate = SroDate,
                InsurancePolicie = ToInsurancePolicie(),
                QualificationCertificates = (ICollection<QualificationCertificate>)Certificates
                    .Select(cvm => context.Add(cvm.ToQualificationCertificate()))
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
                .Add(CBORObject.FromObject(appraiserVM.Certificates
                    .Select(cvm => CBORObject.DecodeFromBytes(cvm.GetCBOR()))
                    .ToArray()
                    )
                );
        }
        void FromCBOR(CBORObject cbor)
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
            Certificates = new ObservableCollection<QualificationCertificateVM>(
                cbor[17].Values.Select(cbor =>
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
