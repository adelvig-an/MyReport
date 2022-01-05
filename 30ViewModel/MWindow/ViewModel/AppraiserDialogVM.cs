using _10Model;
using System;
using System.Windows.Input;

namespace _30ViewModel.MWindow.ViewModel
{
    public class AppraiserDialogVM : MsgViewModel
    {
        #region fields
        private ICommand closeCommand;
        private Action<AppraiserDialogVM> closeHandler = null;

        private string path = null;
        #endregion fields

        public AppraiserDialogVM(Action<AppraiserDialogVM> closeHandler)
        {
            this.closeHandler = closeHandler;
        }

        private ICommand newApptaiser;
        private ICommand search;
        private string searchText;
        public string SearchText
        { get => searchText;}
        public IObservable<Appraiser> Appraisers;
        public ICommand Search
        {
            
        };


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
