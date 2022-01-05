using _10Model;
using _20DbLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
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
            Appraisers = new ObservableCollection<Appraiser>(context.Appraisers.ToList());
            this.closeHandler = closeHandler;
            Search = new RelayCommand(_ => SearchAppraiser());
        }

        public ICommand NewApptaiser { get; }
        public ICommand Search { get; }
        private int searchText;
        public int SearchText
        { get => searchText; set => searchText = value; }
        public ObservableCollection<Appraiser> Appraisers { get; set; };

        public void SearchAppraiser()
        {
            Appraisers = new ObservableCollection<Appraiser>(context.Appraisers.Include(a => a.SroNumber == searchText).ToList());
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
