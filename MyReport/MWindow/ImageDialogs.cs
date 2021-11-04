using _30ViewModel.MWindow;
using _30ViewModel.MWindow.ViewModel;
using MyReport.MWindow.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                coord.HideMetroDialogAsync(context, customDialog);

                Debug.WriteLine("Path: " + instance.Path);
            });

            var dPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            var cPath = Path.Combine(dPath, path);
            imageDialogVM.Path = cPath;

            customDialog.DataContext = imageDialogVM;

            coord.ShowMetroDialogAsync(context, customDialog);
        }
    }
}
