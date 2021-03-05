using _20DbLayer;
using _30ViewModel.PagesVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace _30ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private PageViewModel currentPage;
        public PageViewModel CurrentPage
        {
            get => currentPage;
            set 
            {
                if (CurrentPage?.UpdateCBOR() == true)
                    CurrentPage?.UpdateCBOR(); //Редактирование в CBOR уже созданной записи
                else
                    CurrentPage?.WriteCBOR(); //Сохранение в CBOR
                SetProperty(ref currentPage, value); 
            }
        }
        private readonly ApplicationContext db = new ApplicationContext();

        public MainViewModel()
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Reports.Load();
            db.Contracts.Load();
            db.People.Load();
            db.PrivatePeople.Load();
            db.Directors.Load();
            db.Organizations.Load();
            db.Addresses.Load();
            db.TempDatas.Load();
            CurrentPage = new ReportVM();
            SaveData = new RelayCommand(_ => SaveDataAction());
            NextBackPage = new RelayCommand(_ => NextBackPageAction());
        }
        /// <summary>
        /// Команда сохранения в БД
        /// </summary>
        public ICommand SaveData { get; }
        public ICommand NextBackPage { get; }
        /// <summary>
        /// Метод реализации сохранения данных в БД
        /// </summary>
        public void SaveDataAction()
        {
            if (CurrentPage is OrganizationVM organization)
            {
                organization.AddOrganization();
            }
        }
        public void NextBackPageAction()
        {
            if (CurrentPage is ReportVM)
            { 
                CurrentPage = new ContractVM();
                CurrentPage?.ReadCBOR();
            }    
                
            else if (CurrentPage is ContractVM)
            {
                CurrentPage = new ReportVM();
                CurrentPage.ReadCBOR(); //Чтение из CBOR
            }
        }
    }
}
