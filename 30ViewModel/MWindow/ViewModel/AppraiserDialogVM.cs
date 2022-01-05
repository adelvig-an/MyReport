using _10Model;
using _20DbLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

        private readonly ApplicationContext context;

        public AppraiserDialogVM(Action<AppraiserDialogVM> closeHandler)
        {
            Appraisers = (IObservable<Appraiser>)context.Appraisers.Include(a => a.Id).ToList();
            this.closeHandler = closeHandler;
        }

        private ICommand newApptaiser;
        private ICommand search;
        private int searchText;
        public int SearchText
        { get => searchText; set => searchText = value; }
        public IObservable<Appraiser> Appraisers;

        public void SearchAppraiser(int s)
        {
            Appraisers = (IObservable<Appraiser>)context.Appraisers.Include(a => a.SroNumber == s).ToList();
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
