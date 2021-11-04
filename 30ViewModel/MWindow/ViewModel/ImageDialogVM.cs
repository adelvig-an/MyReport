using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace _30ViewModel.MWindow.ViewModel
{
    public class ImageDialogVM : MsgViewModel
    {
        #region fields
        private ICommand closeCommand;
        private Action<ImageDialogVM> closeHandler = null;

        #endregion fields

        public ImageDialogVM(Action<ImageDialogVM> closeHandler)
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
