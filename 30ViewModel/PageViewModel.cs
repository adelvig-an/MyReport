using _10Model;
using _20DbLayer;
using System;
using System.Diagnostics;
using System.Linq;

namespace _30ViewModel
{
    public abstract class PageViewModel : ValidationBase
    {
        #region Abstract methods CBOR
        /// <summary>
        /// Абстрактный метод преобразования данных в формат cbor
        /// </summary>
        public abstract byte[] GetCBOR();
        /// <summary>
        /// Абстрактный метод преобразования данных из формата cbor
        /// </summary>
        /// <param name="b"></param>
        public abstract void SetCBOR(byte[] b);
        #endregion Abstract methods CBOR

        private readonly ApplicationContext context;
        public PageViewModel()
        {
            context = new ApplicationContext();
        }

        #region Methods CBOR
        /// <summary>
        /// Сохранение данных в формате cbor
        /// </summary>
        public bool WriteCBOR()
        {
            try
            {
                var Id = 0;
                var cbor = GetCBOR();
                var tempData = new TempData
                {
                    Id = Id,
                    Page = GetType().Name,
                    CBOR = cbor
                };
                context.Add(tempData);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Метод сохранения отредактированных данных в формате cbor
        /// </summary>
        public bool UpdateCBOR()
        {
            try
            {
                var primaryKey = GetType().Name;
                var tempData = context.TempDatas.First(t => t.Page == primaryKey);
                var cbor = GetCBOR();
                tempData.CBOR = cbor;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Метод чтения сохраненных данных из формата cbor
        /// </summary>
        public void ReadCBOR()
        {
            try
            {
                var primaryKey = GetType().Name;
                var tempData = context.TempDatas.Single(t => t.Page == primaryKey);
                SetCBOR(tempData.CBOR);
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
            }
        }
        #endregion Methods CBOR
    }
}
