using _10Model;
using _20DbLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace _30ViewModel.PagesVM
{
    public class ContractVM : PageViewModel
    {
        #region Properties (Нужны для валидации данных)
        private string number;
        private DateTime contractDate = DateTime.Today;
        private TargetType target;
        private string intendedUse;
        public int Id { get; set; }
        [Required(ErrorMessage = "Требуется указать номер договора")]
        [StringLength(20, ErrorMessage = "Длина номера договора превышает максимально возможное количество символов")]
        public string Number { get => number;
            set { ValidateProperty(value); SetProperty(ref number, value); } }
        [Required(ErrorMessage = "Требуется указать дату договора")]
        public DateTime ContractDate { get => contractDate;
            set { ValidateProperty(value); SetProperty(ref contractDate, value); } }
        public TargetType Target { get => target; 
            set { ValidateProperty(value); SetProperty(ref target, value); } }
        [Required(ErrorMessage = "Требуется указать предполагаемое использование результатов оценки")]
        public string IntendedUse { get => intendedUse;
            set { ValidateProperty(value); SetProperty(ref intendedUse, value); } }
        #endregion Properties

        private readonly ApplicationContext context;
        public ContractVM()
        {
            context = new ApplicationContext();
        }

        #region DataBase (Методы и свойства взаимодействующие с Базой данных)
        public Contract ToContract()
        {
            var contract = new Contract
            {
                Id = Id,
                Number = Number,
                ContractDate = ContractDate,
                Target = Target,
                IntendedUse = IntendedUse
            };
            return contract;
        }
        public int AddContract()
        {
            try
            {
                Contract contract = ToContract();
                context.Add(contract);
                context.SaveChanges();
                int newId = ToContract().Id;
                return newId;
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                return -1;
            }
        }
        public bool UpdateContract()
        {
            try
            {
                var contract = context.Contracts.First();
                contract = ToContract();
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
    }
}
