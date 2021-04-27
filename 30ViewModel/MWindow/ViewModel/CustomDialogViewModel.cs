using System;
using System.Windows.Input;

namespace _30ViewModel.MWindow.ViewModel
{
    public class CustomDialogViewModel : MsgViewModel
    {
        #region fields
        private ICommand closeCommand;
        private Action<CustomDialogViewModel> closeHandler = null;

        private string firstName = null;
        private string lastName = null;
        #endregion fields

        public CustomDialogViewModel(Action<CustomDialogViewModel> closeHandler)
        {
            this.closeHandler = closeHandler;
        }
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                RaisePropertyChanged(() => this.FirstName);
            }
        }
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                RaisePropertyChanged(() => this.LastName);
            }
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
