using _30ViewModel.MWindow.ViewModel;
using MyReport.MWindow.Ui;
using System.Diagnostics;

namespace MyReport.MWindow
{
    public class CustomDialog
    {
        internal async void RunCustomFromVm(object context)
        {
            var coord = MWindowDialogLib.ContentDialogService.Instance.Coordinator;

            var customDialog = new MWindowDialogLib.Dialogs.CustomDialog(new CustomDialogUi());

            var customDialogViewModel = new CustomDialogViewModel(instance =>
            {
                coord.HideMetroDialogAsync(context, customDialog);

                Debug.WriteLine("Custom Dialog -" + instance.Title + "- VM Result: ");
                Debug.WriteLine("FirstName: " + instance.FirstName);
                Debug.WriteLine("LastName: " + instance.LastName);
            })
            {
                Title = "Модальное окно",
                FirstName = "Просто текст"
            };

            customDialog.DataContext = customDialogViewModel;

            await coord.ShowMetroDialogAsync(context, customDialog);
        }
    }
}
