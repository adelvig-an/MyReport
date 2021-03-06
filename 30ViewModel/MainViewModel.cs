﻿using _20DbLayer;
using _30ViewModel.MWindow;
using _30ViewModel.MWindow.ViewModel;
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
        private bool isVisibl;
        public bool IsVisibl { get => isVisibl; 
            set
            {
                SetProperty(ref isVisibl, value);
                if (CurrentPage is ReportVM)
                    isVisibl = false;
            }
        }
        

        public MainViewModel(IDialogService dialogService)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Reports.Load();
            db.Contracts.Load();
            db.People.Load();
            db.PrivatePeople.Load();
            db.Directors.Load();
            db.Organizations.Load();
            db.AppraiserOrganizations.Load();
            db.Appraisers.Load();
            db.Addresses.Load();
            db.InsurancePolicies.Load();
            db.QualificationCertificates.Load();
            db.TempDatas.Load();
            CurrentPage = new AppraiserVM();
            SaveData = new RelayCommand(_ => SaveDataAction());
            NextPage = new RelayCommand(_ => NextPageAction());
            BackPage = new RelayCommand(_ => BackPageAction());
            ShowDialog = new RelayCommand(_ => dialogService.Show(this));
            AppraiserPage = new RelayCommand(_ => AppraiserPageAction());
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
            if (CurrentPage is OrganizationVM organization)
            {
                organization.AddOrganization();
            }
        }
        public void NextPageAction()
        {
            if (CurrentPage is ReportVM)
            { 
                CurrentPage = new AppraiserVM();
                CurrentPage?.ReadCBOR();
            }    
                
            else if (CurrentPage is AppraiserVM)
            {
                CurrentPage = new ReportVM();
                CurrentPage?.ReadCBOR(); //Чтение из CBOR
            }
        }
        public void BackPageAction()
        {
            if (CurrentPage is ContractVM)
            {
                CurrentPage = new ReportVM();
                CurrentPage.ReadCBOR(); //Чтение из CBOR
            }
            else if (CurrentPage is PrivatePersonVM)
            {
                CurrentPage = new ContractVM();
                CurrentPage.ReadCBOR(); //Чтение из CBOR
            }
            else if (CurrentPage is AppraiserVM)
            {
                CurrentPage = new AppraiserOrganizationVM();
                CurrentPage.ReadCBOR(); //Чтение из CBOR
            }
        }
        public void AppraiserPageAction()
        {
            CurrentPage = new AppraiserVM();
            CurrentPage.ReadCBOR();
        }

        //Test MWindow
        public ICommand ShowDialog { get; }
    }
}
