using _20DbLayer;
using _30ViewModel.MWindow;
using _30ViewModel.MWindow.ViewModel;
using _30ViewModel.PagesVM;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PeterO.Cbor;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace _30ViewModel
{
    public static class CBORHelper
    {
        public static string AsStringSafe(this CBORObject cbor)
        {
            return cbor.IsNull ? "" : cbor.AsString();
        }
    }

    public class MainViewModel : ViewModelBase
    {
        private PageViewModel currentPage;
        public PageViewModel CurrentPage
        {
            get => currentPage;
            set
            {
                //Редактирование в CBOR уже созданной записи
                if (CurrentPage?.UpdateCBOR() == false)
                    CurrentPage?.WriteCBOR(); //Сохранение в CBOR
                SetProperty(ref currentPage, value);
            }
        }
        private readonly ApplicationContext db = new ApplicationContext();
        private bool isVisibl;
        public bool IsVisibl
        {
            get => isVisibl;
            set
            {
                SetProperty(ref isVisibl, value);
                if (CurrentPage is ReportVM)
                    isVisibl = false;
            }
        }

        public MainViewModel(IDialogService dialogService, IImageDiaolgService imageDiaolgService, IAppraiserDialogService appraiserDialogService)
        {
            //db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Reports.Load();
            db.Contracts.Load();
            db.People.Load();
            db.PrivatePeople.Load();
            db.Directors.Load();
            db.Organizations.Load();
            db.AppraiserOrganizations.Load();
            db.Appraisers.Load();
            db.SRO.Load();
            db.Addresses.Load();
            db.InsurancePolicies.Load();
            db.QualificationCertificates.Load();
            db.TempDatas.Load();
            
            CurrentPage = new TestPageVM(); //Стартовая страница
            
            SaveData = new RelayCommand(_ => SaveDataAction());
            NextPage = new RelayCommand(_ => NextPageAction());
            BackPage = new RelayCommand(_ => BackPageAction());
            ShowDialog = new RelayCommand(_ => dialogService.Show(this));
            ShowImageDialog = new RelayCommand(p => imageDiaolgService.OpenImage(this, p.ToString()));
            ShowAppraiserDialog = new AsyncRelayCommand(_ => appraiserDialogService.ShowAsync(this));
            AppraiserPage = new RelayCommand(_ => AppraiserPageAction());

            NewAOVM = new RelayCommand(_ => NewAOVMAction());
            CborAOVM = new RelayCommand(_ => CborAOVMAction());
            LoadAOVM = new RelayCommand(_ => LoadAOVMAction());
        }

        /// <summary>
        /// Команда сохранения в БД
        /// </summary>
        public ICommand SaveData { get; }
        public ICommand NextPage { get; }
        public ICommand BackPage { get; }
        public ICommand AppraiserPage { get; }

        /// <summary>
        /// Метод реализации сохранения данных в БД
        /// </summary>
        public void SaveDataAction()
        {
            if (CurrentPage is AppraiserVM appraiserVM)
            {
                appraiserVM.AddOrUpdateAppraiser(appraiserVM);
                //appraiserVM.ReturnIdAppraiser();
                CurrentPage = new AppraiserOrganizationVM();
                CurrentPage?.ReadCBOR();
            }
        }
        public void NextPageAction()
        {
            if (CurrentPage is AppraiserOrganizationVM appraiserOrg)
            {
                appraiserOrg.AddOrUpdateAppraiserOrganization(appraiserOrg);
                CurrentPage = new TestPageVM();
            }
        }
        public void BackPageAction()
        {
            if (CurrentPage is AppraiserVM)
            {
                CurrentPage = new AppraiserOrganizationVM();
                CurrentPage?.ReadCBOR(); //Чтение из CBOR
            }
        }
        public void AppraiserPageAction()
        {
            CurrentPage = new AppraiserVM();
        }

        //MWindow
        public ICommand ShowDialog { get; }
        public ICommand ShowImageDialog { get; }
        public ICommand ShowAppraiserDialog { get; }

        //
        public ICommand NewAOVM { get; }
        public void NewAOVMAction()
        {
            CurrentPage = new AppraiserOrganizationVM();
        }
        public ICommand CborAOVM { get; }
        public void CborAOVMAction()
        {
            CurrentPage = new AppraiserOrganizationVM();
            CurrentPage?.ReadCBOR();
        }
        public ICommand LoadAOVM { get; }
        public void LoadAOVMAction()
        {
            CurrentPage = new AppraiserOrganizationVM().LoadAppraiserOrganization();
        }
    }
}
