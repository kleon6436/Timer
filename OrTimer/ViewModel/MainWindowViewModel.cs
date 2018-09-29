using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Input;

namespace OrTimer.ViewModel
{
    public class MainWindowViewModel:BindableBase
    {
        // コマンド一覧
        public ICommand MinutePlusCommand { get; set; }
        public ICommand MinuteMinusCommand { get; set; }
        public ICommand SecondPlusCommand { get; set; }
        public ICommand SecondMinusCommand { get; set; }
        public ICommand StartButtonCommand { get; set; }
        public ICommand StopButtonCommand { get; set; }
        public ICommand ResetButtonCommand { get; set; }

        // 値のプロパティ一覧
        private string _minuteText;
        public string MinuteText
        {
            get { return _minuteText; }
            set { SetProperty(ref _minuteText, value); }
        }

        private string _secondText;
        public string SecondText
        {
            get { return _secondText; }
            set { SetProperty(ref _secondText, value); }
        }

        // コンストラクタ
        public MainWindowViewModel()
        {
            // コマンドの設定
            SetCommand();

            // プロパティの初期値をセット
            const string _defaultValue = "00";  // ２桁表示であることに注意
            MinuteText = _defaultValue;
            SecondText = _defaultValue;
        }

        /// <summary>
        /// コマンドの設定
        /// </summary>
        private void SetCommand()
        {
            MinutePlusCommand = new DelegateCommand(MinutePlusCommandClicked);
            MinuteMinusCommand = new DelegateCommand(MinuteMinusCommandClicked);
            SecondPlusCommand = new DelegateCommand(SecondPlusCommandClicked);
            SecondMinusCommand = new DelegateCommand(SecondMinusCommandClicked);
            StartButtonCommand = new DelegateCommand(StartButtonCommandClicked);
            StopButtonCommand = new DelegateCommand(StopButtonCommandClicked);
            ResetButtonCommand = new DelegateCommand(ResetButtonCommandClicked);
        }

        /// <summary>
        /// 分のプラスボタンを押下したとき
        /// </summary>
        private void MinutePlusCommandClicked()
        {

        }

        /// <summary>
        /// 分のマイナスボタンを押下したとき
        /// </summary>
        private void MinuteMinusCommandClicked()
        {

        }

        /// <summary>
        /// 秒のプラスボタンを押下したとき
        /// </summary>
        private void SecondPlusCommandClicked()
        {

        }

        /// <summary>
        /// 秒のマイナスボタンを押下したとき
        /// </summary>
        private void SecondMinusCommandClicked()
        {

        }

        /// <summary>
        /// スタートボタンを押下したとき
        /// </summary>
        private void StartButtonCommandClicked()
        {

        }

        /// <summary>
        /// ストップボタンを押下したとき
        /// </summary>
        private void StopButtonCommandClicked()
        {

        }

        /// <summary>
        /// リセットボタンを押下したとき
        /// </summary>
        private void ResetButtonCommandClicked()
        {

        }
    }
}
