using _30ViewModel;
using MyReport.MWindow;
using System.Windows;


namespace MyReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MWindowLib.MetroWindow
    {
        public MainWindow()
        {
            var dialogService = new CustomDialogs();
            InitializeComponent();
            DataContext = new MainViewModel(dialogService);
        }
    }
}
