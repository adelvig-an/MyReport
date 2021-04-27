using _30ViewModel;
using MyReport.MWindow;


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
