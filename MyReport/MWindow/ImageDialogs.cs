using _30ViewModel.MWindow;
using MyReport.MWindow.Ui;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyReport.MWindow
{
    public class ImageDialogs : IImageDiaolgService
    {
        public void OpenImage(string path)
        {
            var coord = MWindowDialogLib.ContentDialogService.Instance.Coordinator;

            var customDialog = new MWindowDialogLib.Dialogs.CustomDialog(new ImageDialogUi());

            coord.ShowMetroDialogAsync(path, customDialog);
        }
    }
}
