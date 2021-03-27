using _10Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace _30ViewModel
{
    public class QualificationCertificateVM : ValidationBase
    {
        private int certificateNumber;
        private DateTime? certificateDateFrom;
        private DateTime? certificateDateBefore;
        private SpecialityType speciality;
        public int Id { get; set; }
        public int CertificateNumber
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
        public SpecialityType Speciality
        {
            get => speciality;
            set { ValidateProperty(value); SetProperty(ref speciality, value); }
        }
    }
}
