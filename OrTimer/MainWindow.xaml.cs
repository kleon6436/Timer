using OrTimer.ViewModel;
using System.Windows;

namespace OrTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel _mainWindowViewModel = new MainWindowViewModel();
            this.DataContext = _mainWindowViewModel;
        }
    }
}
