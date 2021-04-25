using _30ViewModel.MWindow.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyReport.MWindow.Ui
{
    /// <summary>
    /// Interaction logic for CustomDialogUi.xaml
    /// </summary>
    public partial class CustomDialogUi : UserControl
    {
        public CustomDialogUi()
        {
            InitializeComponent();
            DataContext = new CustomDialogViewModel();
        }
    }
}
