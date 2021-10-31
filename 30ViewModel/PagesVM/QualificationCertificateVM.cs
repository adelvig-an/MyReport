using _10Model;
using PeterO.Cbor;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace _30ViewModel.PagesVM
{
    public class QualificationCertificateVM : PageViewModel
    {
        private string certificateNumber;
        private DateTime? certificateDateFrom;
        private DateTime? certificateDateBefore;
        private SpecialityType speciality;
        private string nameInstitution = @"""ФБУ"" ""ФРЦ по организации подготовки управленческих кадров""";
        private string pathQualificationCertificateImage;
        public int Id { get; set; }
        public string CertificateNumber
        {
            get => certificateNumber;
            set { ValidateProperty(value); SetProperty(ref certificateNumber, value); }
        }
        public DateTime? CertificateDateFrom
        {
            get => certificateDateFrom;
            set { ValidateProperty(value); SetProperty(ref certificateDateFrom, value); ToCertificateDateBefore(); }
        }
        public DateTime? CertificateDateBefore
        {
            get => certificateDateBefore;
            set { ValidateProperty(value); SetProperty(ref certificateDateBefore, value); }
        }
        public SpecialityType Speciality { get => speciality;
            set => SetProperty(ref speciality, value); }
        public string NameInstitution { get => nameInstitution; 
            set { ValidateProperty(value); SetProperty(ref nameInstitution, value); } }
        public string PathQualificationCertificateImage
        { get => pathQualificationCertificateImage;
            set { ValidateProperty(value); SetProperty(ref pathQualificationCertificateImage, value); } }

        public ObservableCollection<string> PathImageCollection { get; set; }

        public QualificationCertificateVM()
        {
            PathImageCollection = new ObservableCollection<string>();

            AddImageCommand = new RelayCommand(_ => AddImage());
            RemoveImageCommand = new RelayCommand(p => RemoveImage(p.ToString()));
        }

        public void ToCertificateDateBefore()
        {
            CertificateDateBefore = CertificateDateFrom?.AddDays(-1).AddYears(+3);
        }
        public QualificationCertificate ToQualificationCertificate()
        {
            var certificate = new QualificationCertificate
            {
                Id = Id,
                Number = CertificateNumber,
                DateFrom = CertificateDateFrom,
                DateBefore = CertificateDateBefore,
                Speciality = Speciality,
                NameInstitution = NameInstitution
            };
            return certificate;
        }

        public ICommand AddImageCommand { get; }
        public void AddImage()
        {
            var path = GetAndCopyImage.CopyImage();
            if (path != null)
            {
                PathImageCollection.Add(path);
            }
        }
        public ICommand RemoveImageCommand { get; }
        public void RemoveImage(string s)
        {
            PathImageCollection.Remove(s);
            File.Delete(s);
        }

        #region CBOR
        private static CBORObject ToCBOR(QualificationCertificateVM certificateVM)
        {
            return CBORObject.NewArray()
                .Add(certificateVM.Id)
                .Add(certificateVM.CertificateNumber)
                .Add(certificateVM.CertificateDateFrom.HasValue
                ? CBORObject.NewArray().Add(true).Add(certificateVM.CertificateDateFrom.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(certificateVM.CertificateDateBefore.HasValue
                ? CBORObject.NewArray().Add(true).Add(certificateVM.CertificateDateBefore.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(certificateVM.Speciality)
                .Add(CBORObject.FromObject(certificateVM.PathImageCollection
                    .Select(pip => CBORObject.FromObject(pip)).ToArray()
                    )
                );
        }
        private void FromCBOR(CBORObject cbor)
        {
            Id = cbor[0].AsInt32();
            CertificateNumber = cbor[1].IsNull ? "" : cbor[1].AsString();
            CertificateDateFrom = cbor[2][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[2][1].ToObject<long>()))
            : null;
            CertificateDateBefore = cbor[3][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[3][1].ToObject<long>()))
            : null;
            Speciality = (SpecialityType)Enum.Parse(typeof(SpecialityType), cbor[4].ToString(), true);
            PathImageCollection = new ObservableCollection<string>(
                cbor[5].Values.Select(cbor =>
                {
                    var pipi = cbor.AsStringSafe();
                    return pipi;
                }));
        }
        public override byte[] GetCBOR() => ToCBOR(this).EncodeToBytes();
        public override void SetCBOR(byte[] b) => FromCBOR(CBORObject.DecodeFromBytes(b));
        #endregion CBOR
    }
}
