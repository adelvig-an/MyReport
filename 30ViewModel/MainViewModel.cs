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
            set => SetProperty(ref currentPage, value);
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
            CurrentPage = new PrivatePersonVM();
            SaveData = new RelayCommand(_ => SaveDataAction());
        }
        /// <summary>
        /// Команда сохранения в БД
        /// </summary>
        public ICommand SaveData { get; }
        /// <summary>
        /// Метод реализации сохранения данных в БД
        /// </summary>
        public void SaveDataAction()
        {
            if (CurrentPage is ContractVM contract)
            {
                contract.AddContract();
            }
        }
    }
}
