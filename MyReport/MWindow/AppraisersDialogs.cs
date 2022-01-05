using _30ViewModel.MWindow;
using _30ViewModel.MWindow.ViewModel;
using MyReport.MWindow.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MyReport.MWindow
{
    public class AppraisersDialogs : IDialogService
    {
        public void Show(object context)
        {
            var coord = MWindowDialogLib.ContentDialogService.Instance.Coordinator;

            var appraiserDialog = new MWindowDialogLib.Dialogs.CustomDialog(new AppraisersDialogUi());

            var appraiserDialogViewModal = new AppraiserDialogVM(instance =>
            {
                coord.HideMetroDialogAsync(context, appraiserDialog);

                Debug.WriteLine("");
            })

            {
                Title = "Поиск оценщика"
            };

            appraiserDialog.DataContext = appraiserDialogViewModal;

            coord.ShowMetroDialogAsync(context, appraiserDialog);

        }
    }
}
