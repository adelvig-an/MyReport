using MWindowDialogLib.ViewModels;
using MWindowInterfacesLib.Events;
using MWindowInterfacesLib.Interfaces;
using System;
using System.Windows.Input;

namespace _30ViewModel.MWindow.ViewModel
{
    public class MsgViewModel : DialogResultViewModel<int>, IBaseMetroDialogFrameViewModel<int>
    {
        #region fields
        private string messange;
        private string title;
        private bool dialogCanCloseViaChrome;
        private bool? dialogCloseResult;
        private ICommand closeCommand;
        private bool closeWindowButtonVisibility;
        #endregion fields

        public event EventHandler<DialogStateChangedEventArgs> DialogClosed;

        #region properties
        #region IBaseMetroDialogFrameViewModel properties
        public string Title
        {
            get => title;
            set
            {
                if (title != value)
                {
                    title = value;
                }
            }
        }
        public bool DialogCanCloseViaChrome
        {
            get => dialogCanCloseViaChrome;
            set
            {
                if (dialogCanCloseViaChrome != value)
                {
                    dialogCanCloseViaChrome = value;
                }
            }
        }
        public bool? DialogCloseResult
        {
            get => dialogCloseResult;
            set
            {
                if (dialogCloseResult != value)
                {
                    dialogCloseResult = value;
                }
            }
        }
        virtual public ICommand CloseCommand
        {
            get => closeCommand;
            set
            {
                if (closeCommand != value)
                {
                    closeCommand = value;
                }
            }
        }
        public bool CloseWindowButtonVisibility
        {
            get => closeWindowButtonVisibility;
            set
            {
                if (closeWindowButtonVisibility != value)
                {
                    closeWindowButtonVisibility = value;
                }
            }
        }
        #endregion IBaseMetroDialogFrameViewModel properties
        public string Messange
        {
            get => messange;
            set
            {
                if (messange != value)
                {
                    messange = value;
                }
            }
        }
        #endregion properties
        #region methods
        protected void SendDialogStateChangedEvent()
        {
            DialogClosed?.Invoke(this, new DialogStateChangedEventArgs());
        }
        #endregion methods
    }
}
