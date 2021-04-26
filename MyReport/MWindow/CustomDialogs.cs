using _30ViewModel.MWindow;
using _30ViewModel.MWindow.ViewModel;
using MyReport.MWindow.Ui;
using System.Diagnostics;

namespace MyReport.MWindow
{
    public class CustomDialogs : IDialogService
    {
        public void Show(MsgViewModel msgViewModel)
        {
            var coord = MWindowDialogLib.ContentDialogService.Instance.Coordinator;

            var customDialog = new MWindowDialogLib.Dialogs.CustomDialog(new CustomDialogUi());

            msgViewModel = new CustomDialogViewModel()
            {
                Title = "Модальное окно",
                FirstName = "Тест открытия и закрытия Модального окна"
            };

            customDialog.DataContext = msgViewModel;

            coord.ShowMetroDialogAsync(msgViewModel, customDialog);
        }
    }
}
