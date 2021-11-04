using _30ViewModel.MWindow;
using _30ViewModel.MWindow.ViewModel;
using MyReport.MWindow.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MyReport.MWindow
{
    public class ImageDialogs : IImageDiaolgService
    {
        public void OpenImage(object context, string path)
        {
            var coord = MWindowDialogLib.ContentDialogService.Instance.Coordinator;

            var customDialog = new MWindowDialogLib.Dialogs.CustomDialog(new ImageDialogUi());

            var imageDialogVM = new ImageDialogVM(instance =>
            {
                coord.HideMetroDialogAsync(path, customDialog);

                Debug.WriteLine("FirstName: " + instance.Path);
            });

            customDialog.DataContext = imageDialogVM;

            coord.ShowMetroDialogAsync(path, customDialog);
        }
    }
}
