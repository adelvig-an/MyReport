using _10Model;
using _20DbLayer;
using PeterO.Cbor;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace _30ViewModel.PagesVM
{
    public class ContractVM : PageViewModel
    {
        #region Properties (Нужны для валидации данных)
        private string number;
        private DateTime? contractDate = DateTime.Today;
        private TargetType target;
        private string intendedUse = "Сообщение";
        public int Id { get; set; }
        [Required(ErrorMessage = "Требуется указать номер договора")]
        [StringLength(20, ErrorMessage = "Длина номера договора превышает максимально возможное количество символов")]
        public string Number { get => number;
            set { ValidateProperty(value); SetProperty(ref number, value); } }
        [Required(ErrorMessage = "Требуется указать дату договора")]
        public DateTime? ContractDate { get => contractDate;
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
        public void AddContract()
        {
            try
            {
                Contract contract = ToContract();
                context.Add(contract);
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

        #region CBOR
        static CBORObject ToCBOR(ContractVM contractVM)
        {
            return CBORObject.NewArray()
                .Add(contractVM.Id)
                .Add(contractVM.Number)
                .Add(contractVM.ContractDate.HasValue
                ? CBORObject.NewArray().Add(true).Add(contractVM.ContractDate.Value.ToBinary())
                : CBORObject.NewArray().Add(false))
                .Add(contractVM.Target)
                .Add(contractVM.IntendedUse);
        }
        void FromCBOR(CBORObject cbor)
        {
            Id = cbor[0].AsInt32();
            Number = cbor[1].AsString();
            ContractDate = cbor[2][0].AsBoolean()
            ? new DateTime?(DateTime.FromBinary(cbor[2][1].ToObject<long>()))
            : null;
            Target = (TargetType) Enum.Parse(typeof(TargetType), cbor[3].AsString());
            IntendedUse = cbor[4].AsString();
        }
        public override byte[] GetCBOR() => ToCBOR(this).EncodeToBytes();
        public override void SetCBOR(byte[] b) => FromCBOR(CBORObject.DecodeFromBytes(b));
        #endregion CBOR
    }
}
