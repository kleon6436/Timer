using Kchary.Timer.ViewModels;

namespace Kchary.Timer.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var vm = new MainWindowViewModel();
            DataContext = vm;
        }

        /// <summary>
        /// ウィンドウクローズ処理
        /// </summary>
        /// <param name="sender">Window</param>
        /// <param name="e">引数情報</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            vm.CheckAndStopTimer();
            vm?.Dispose();
        }
    }
}