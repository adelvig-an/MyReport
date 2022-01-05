using System;
using System.Windows.Input;

namespace _30ViewModel.MWindow.ViewModel
{
    public class AppraiserDialogVM : MsgViewModel
    {
        #region fields
        private ICommand closeCommand;
        private Action<ImageDialogVM> closeHandler = null;

        private string path = null;
        #endregion fields

        public AppraiserDialogVM(Action<AppraiserDialogVM> closeHandler)
        {
            this.closeHandler = closeHandler;
        }


        public override ICommand CloseCommand
        {
            get
            {
                if (closeCommand == null)
                {
                    closeCommand = new RelayCommand(_ => { closeHandler(this); });
                }
                return closeCommand;
            }
        }
    }
}
