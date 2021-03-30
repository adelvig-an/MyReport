using _10Model;
using PeterO.Cbor;
using System;

namespace _30ViewModel.PagesVM
{
    public class QualificationCertificateVM : PageViewModel
    {
        private string certificateNumber;
        private DateTime? certificateDateFrom;
        private DateTime? certificateDateBefore;
        private SpecialityType speciality;
        private string pathImg;
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
        public string PathImg { get => pathImg;
            set { ValidateProperty(value); SetProperty(ref pathImg, value); } }

        public void ToCertificateDateBefore()
        {
            CertificateDateBefore = CertificateDateFrom?.AddDays(-1).AddYears(+3);
        }


        static CBORObject ToCBOR(QualificationCertificateVM certificateVM)
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
                .Add(certificateVM.Speciality);
        }
        void FromCBOR(CBORObject cbor)
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
        }
        public override byte[] GetCBOR() => ToCBOR(this).EncodeToBytes();
        public override void SetCBOR(byte[] b) => FromCBOR(CBORObject.DecodeFromBytes(b));
    }
}
