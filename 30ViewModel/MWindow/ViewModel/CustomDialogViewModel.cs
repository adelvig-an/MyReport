using System;
using System.Windows.Input;

namespace _30ViewModel.MWindow.ViewModel
{
    public class CustomDialogViewModel : MsgViewModel
    {
        #region fields
        public ICommand CloseCommand { get; }
        private Action<CustomDialogViewModel> closeHandler = null;

        private string firstName = null;
        private string lastName = null;
        #endregion fields

        public CustomDialogViewModel()
        {
            CloseCommand = new RelayCommand(_ => CloseCommandAction());
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
        public void CloseCommandAction()
        {
            closeHandler(this);
        }
    }
}
