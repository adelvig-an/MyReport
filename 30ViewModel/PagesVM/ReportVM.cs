using _10Model;
using _20DbLayer;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace _30ViewModel.PagesVM
{
    public class ReportVM : PageViewModel
    {
        #region Properties (Нужны для валидации данных)
        private string number;
        private DateTime vulationDate = DateTime.Today;
        private DateTime compilationDate = DateTime.Today;
        private DateTime inspectionDate = DateTime.Today;
        private string inspectionFeaures = "Отсутствуют";
        public int Id { get; set; }
        [Required(ErrorMessage = "Требуется указать номер отчета")]
        [StringLength(20,ErrorMessage = "Длина номера отчета превышает максимально возможное количество символов")]
        public string Number { get => number;
            set { ValidateProperty(value); SetProperty(ref number, value); } }
        [Required(ErrorMessage = "Требуется указать дату оценки")]
        public DateTime VulationDate { get=> vulationDate;
            set { ValidateProperty(value); SetProperty(ref vulationDate, value); } }
        [Required(ErrorMessage = "Требуется указать дату составления отчета")]
        public DateTime CompilationDate { get => compilationDate;
            set { ValidateProperty(value); SetProperty(ref compilationDate, value); } }
        [Required(ErrorMessage = "Требуется указать дату осмотра")]
        public DateTime InspectionDate { get => inspectionDate;
            set { ValidateProperty(value); SetProperty(ref inspectionDate, value); } }
        [Required(ErrorMessage = "Требуется указать особенности проведения осмотра")]
        public string InspectionFeaures { get=>inspectionFeaures;
            set { ValidateProperty(value); SetProperty(ref inspectionFeaures, value); } }
        #endregion Properties

        private readonly ApplicationContext context;
        public ReportVM()
        {
            context = new ApplicationContext();
        }

        #region DataBase (Методы и свойства взаимодействующие с Базой данных)
        /// <summary>
        /// Метод преобразования свойст класса ReportVM в свойства Model.Report
        /// </summary>
        public Report ToReport()
        {
            var report = new Report
            {
                Id = Id,
                Number = Number,
                VulationDate = VulationDate,
                CompilationDate = CompilationDate,
                InspectionDate = InspectionDate,
                InspectionFeaures = InspectionFeaures
            };
            return report;
        }
        /// <summary>
        /// Метод сохранения зачений в БД
        /// context.Add(report) - метод сохранения, который принимает данные из метода ToReport
        /// newId - передает Id созданного объекта
        /// </summary>
        public int AddReport()
        {
            try
            {
                Report report = ToReport();
                context.Add(report);
                context.SaveChanges();
                int newId = ToReport().Id;
                return newId;
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                return -1;
            }
        }
        /// <summary>
        /// Метод редактирования/изменения ранее сохраненных данных в БД
        /// </summary>
        public bool UpdateReport()
        {
            try
            {
                var report = context.Reports.First();
                report = ToReport();
                context.SaveChanges();
                return true;
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
                return false;
            }
        }
        #endregion DataBase
    }
}
