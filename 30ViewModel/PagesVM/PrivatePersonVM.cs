using _10Model.Customer;
using _20DbLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace _30ViewModel.PagesVM
{
    public class PrivatePersonVM : PageViewModel
    {
        #region Properties (Нужны для валидации данных)
        private string secondName;
        private string firstName;
        private string middleName;
        private string serial;
        private string number;
        private string division;
        private DateTime divisionDate = DateTime.Today;
        public int Id { get; set; }
        public string SecondName { get => secondName;
            set { ValidateProperty(value); SetProperty(ref secondName, value); } }
        public string FirstName { get => firstName;
            set { ValidateProperty(value); SetProperty(ref firstName, value); } }
        public string MiddleName { get => middleName;
            set { ValidateProperty(value); SetProperty(ref middleName, value); } }
        public string Serial { get => serial;
            set { ValidateProperty(value); SetProperty(ref serial, value); } }
        public string Number { get => number;
            set { ValidateProperty(value); SetProperty(ref number, value); } }
        public string Division { get => division;
            set { ValidateProperty(value); SetProperty(ref division, value); } }
        public DateTime DivisionDate { get => divisionDate;
            set { ValidateProperty(value); SetProperty(ref divisionDate, value); } }
        #endregion Properties

        private readonly ApplicationContext context;
        public PrivatePersonVM()
        {
            context = new ApplicationContext();
        }
        public PrivatePerson ToPrivatePerson()
        {
            var privatePerson = new PrivatePerson
            {
                Id = Id,
                SecondName = SecondName,
                FirstName = FirstName,
                MiddleName = MiddleName,
                Serial = Serial,
                Number = Number,
                Division = Division,
                DivisionDate = DivisionDate
            };
            return privatePerson;
        }
        public int AddPrivatePerson()
        {
            try
            {
                PrivatePerson privatePerson = ToPrivatePerson();
                context.Add(privatePerson);
                context.SaveChanges();
                int newId = ToPrivatePerson().Id;
                return newId;
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                return -1;
            }
        }
        public bool UpdatePrivatePerson()
        {
            try
            {
                var privatePerson = context.PrivatePeople.First();
                privatePerson = ToPrivatePerson();
                context.SaveChanges();
                return true;
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                return false;
            }
        }
    }
}
