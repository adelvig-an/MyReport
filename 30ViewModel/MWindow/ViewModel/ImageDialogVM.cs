using System;
using System.Windows.Input;

namespace _30ViewModel.MWindow.ViewModel
{
    public class ImageDialogVM : MsgViewModel
    {
        #region fields
        private ICommand closeCommand;
        private Action<ImageDialogVM> closeHandler = null;

        private string path = null;
        #endregion fields

        public ImageDialogVM(Action<ImageDialogVM> closeHandler)
        {
            this.closeHandler = closeHandler;
        }
        public string Path
        {
            get => path;
            set { path = value; RaisePropertyChanged(() => path); }
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
