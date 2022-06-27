using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _30ViewModel.MWindow
{
    public interface IAppraiserDialogService
    {
        public Task<int> Show(object context);
    }
}
