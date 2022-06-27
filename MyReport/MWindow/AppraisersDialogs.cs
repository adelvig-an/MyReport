using _30ViewModel.MWindow;
using _30ViewModel.MWindow.ViewModel;
using MyReport.MWindow.Ui;
using System.Threading.Tasks;

namespace MyReport.MWindow
{
    //1) изменить интерфейс чтобы он возвращал Task<int> а не void
    //2) команду сделать асинхронной
    //3) возвращать значение Id из диалоговой VM

    public class AppraisersDialogs : IAppraiserDialogService
    {
        public async Task<int> Show(object context)
        {
            var coord = MWindowDialogLib.ContentDialogService.Instance.Coordinator;

            var appraiserDialog = new MWindowDialogLib.Dialogs.CustomDialog(new AppraisersDialogUi());

            var appraiserDialogViewModal = new AppraiserDialogVM(instance =>
            {
                coord.HideMetroDialogAsync(context, appraiserDialog);
            })

            {
                Title = "Поиск оценщика"
            };

            appraiserDialog.DataContext = appraiserDialogViewModal;

            await coord.ShowMetroDialogAsync(context, appraiserDialog);

            return appraiserDialogViewModal.selectedId;
        }
    }
}


