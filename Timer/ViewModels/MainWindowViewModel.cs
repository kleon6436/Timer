using System.Windows.Input;
using Kchary.Timer.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace Kchary.Timer.ViewModels
{
    public sealed class MainWindowViewModel : BindableBase
    {
        #region コマンド一覧

        public ICommand MinutePlusCommand { get; }
        public ICommand MinuteMinusCommand { get; }
        public ICommand SecondPlusCommand { get; }
        public ICommand SecondMinusCommand { get; }
        public ICommand StartButtonCommand { get; }
        public ICommand StopButtonCommand { get; }
        public ICommand ResetButtonCommand { get; }

        #endregion

        /// <summary>
        /// タイマーに表示する数字のデフォルト値
        /// </summary>
        /// <remarks>
        /// ２桁表示であることに注意
        /// </remarks>
        private const string DefaultValue = "00";

        /// <summary>
        /// タイマーコントローラー
        /// </summary>
        private TimerController TimerController { get; init; }

        private string minuteText;
        private string secondText;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            // コマンドの設定
            MinutePlusCommand = new DelegateCommand(MinutePlusCommandClicked);
            MinuteMinusCommand = new DelegateCommand(MinuteMinusCommandClicked);
            SecondPlusCommand = new DelegateCommand(SecondPlusCommandClicked);
            SecondMinusCommand = new DelegateCommand(SecondMinusCommandClicked);
            StartButtonCommand = new DelegateCommand(StartButtonCommandClicked);
            StopButtonCommand = new DelegateCommand(StopButtonCommandClicked);
            ResetButtonCommand = new DelegateCommand(ResetButtonCommandClicked);

            TimerController = new TimerController();
            TimerController.TimerEvent += UpdateTimerValue;

            // プロパティの初期値をセット
            MinuteText = DefaultValue;
            SecondText = DefaultValue;
        }

        /// <summary>
        /// 分
        /// </summary>
        public string MinuteText
        {
            get => minuteText;
            private set => SetProperty(ref minuteText, value);
        }

        /// <summary>
        /// 秒
        /// </summary>
        public string SecondText
        {
            get => secondText;
            private set => SetProperty(ref secondText, value);
        }

        /// <summary>
        /// 分のプラスボタンを押下したとき
        /// </summary>
        private void MinutePlusCommandClicked()
        {
            MinuteText = TimerController.PlusMinute();
        }

        /// <summary>
        /// 分のマイナスボタンを押下したとき
        /// </summary>
        private void MinuteMinusCommandClicked()
        {
            MinuteText = TimerController.MinusMinute();
        }

        /// <summary>
        /// 秒のプラスボタンを押下したとき
        /// </summary>
        private void SecondPlusCommandClicked()
        {
            SecondText = TimerController.PlusSecond();
        }

        /// <summary>
        /// 秒のマイナスボタンを押下したとき
        /// </summary>
        private void SecondMinusCommandClicked()
        {
            SecondText = TimerController.MinusSecond();
        }

        /// <summary>
        /// スタートボタンを押下したとき
        /// </summary>
        private void StartButtonCommandClicked()
        {
            TimerController.StartTimer();
        }

        /// <summary>
        /// ストップボタンを押下したとき
        /// </summary>
        private void StopButtonCommandClicked()
        {
            TimerController.StopTimer();
        }

        /// <summary>
        /// リセットボタンを押下したとき
        /// </summary>
        private void ResetButtonCommandClicked()
        {
            TimerController.ResetTimer();
        }

        /// <summary>
        /// タイマー値を更新する
        /// </summary>
        /// <param name="timerValue">タイマー値</param>
        private void UpdateTimerValue(TimerValue timerValue)
        {
            MinuteText = TimerController.GetTimerValue(timerValue.Minute);
            SecondText = TimerController.GetTimerValue(timerValue.Second);
        }
    }
}