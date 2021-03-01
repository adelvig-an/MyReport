using _10Model;
using _20DbLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace _30ViewModel
{
    public abstract class PageViewModel : ValidationBase
    {
        public abstract byte[] GetCBOR();
        public abstract void SetCBOR(byte[] b);

        private readonly ApplicationContext context;
        public PageViewModel()
        {
            context = new ApplicationContext();
        }

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
    }
}
