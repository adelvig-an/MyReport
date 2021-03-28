using _10Model;
using System;

namespace _30ViewModel.PagesVM
{
    public class QualificationCertificateVM : PageViewModel
    {
        private string certificateNumber = "010296-1";
        private DateTime? certificateDateFrom = DateTime.Today;
        private DateTime? certificateDateBefore = DateTime.Today;
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
            set { ValidateProperty(value); SetProperty(ref certificateDateFrom, value); }
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

        public override byte[] GetCBOR()
        {
            throw new NotImplementedException();
        }
        public override void SetCBOR(byte[] b)
        {
            throw new NotImplementedException();
        }
    }
}
