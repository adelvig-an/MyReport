using _10Model;
using _20DbLayer;
using _30ViewModel.PagesVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _30ViewModel.MWindow.ViewModel
{
    public class AppraiserDialogVM : MsgViewModel
    {
        #region fields
        private ICommand closeCommand;
        private Action<AppraiserDialogVM> closeHandler = null;
        #endregion fields

        private readonly ApplicationContext context;

        public AppraiserDialogVM(Action<AppraiserDialogVM> closeHandler)
        {
            context = new ApplicationContext();
            Appraisers = new ObservableCollection<Appraiser>();
            //Appraisers = new ObservableCollection<Appraiser>(context.Appraisers.ToList());
            this.closeHandler = closeHandler;
            Search = new RelayCommand(_ => SearchAppraiser());
            SelectAppraiser = new RelayCommand(_ => SelectedAppraiser());
        }

        public ICommand NewApptaiser { get; }
        public ICommand Search { get; }
        public ICommand SelectAppraiser { get; }
        public int selectedId = -1;
        private int searchText;
        public int SearchText
        {
            get => searchText;
            set => searchText = value; 
        }
        private Appraiser appraiser;
        public Appraiser Appraiser { get => appraiser; set => appraiser = value; }
        public ObservableCollection<Appraiser> Appraisers { get; set; }

        public void SearchAppraiser()
        {
            Appraisers.Clear();
            foreach (var item in context.Appraisers.Where(a => a.SroNumber == searchText))
            {
                Appraisers.Add(item);
            }
        }

        public void SelectedAppraiser()
        {
            selectedId = Appraiser.Id;
            closeHandler(this);
            AppraiserOrganizationVM appraiserOrganizationVM = new AppraiserOrganizationVM();
            appraiserOrganizationVM.AppraiserAdd(selectedId);
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
