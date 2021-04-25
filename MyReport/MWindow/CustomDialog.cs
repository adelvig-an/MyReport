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

            var customDialogViewModel = new CustomDialogViewModel()
            {
                Title = "Модальное окно",
                FirstName = "Просто текст"
            };

            customDialog.DataContext = customDialogViewModel;

            await coord.ShowMetroDialogAsync(context, customDialog);
        }
    }
}
