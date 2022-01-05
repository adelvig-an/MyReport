using _30ViewModel.MWindow;
using _30ViewModel.MWindow.ViewModel;
using MyReport.MWindow.Ui;
using System.Diagnostics;

namespace MyReport.MWindow
{
    public class CustomDialogs : IDialogService
    {
        public void Show(object context)
        {
            var coord = MWindowDialogLib.ContentDialogService.Instance.Coordinator;

            var customDialog = new MWindowDialogLib.Dialogs.CustomDialog(new CustomDialogUi());

            var customDialogViewModal = new CustomDialogViewModel(instance =>
            {
                coord.HideMetroDialogAsync(context, customDialog);

                Debug.WriteLine("Custom Dialog -" + instance.Title + "- VM Result: ");
                Debug.WriteLine("FirstName: " + instance.FirstName);
                Debug.WriteLine("LastName: " + instance.LastName);
            })
            {
                Title = "Модальное окно",
                FirstName = "Тест открытия и закрытия Модального окна"
            };

            customDialog.DataContext = customDialogViewModal;

            coord.ShowMetroDialogAsync(context, customDialog);
        }
    }
}
