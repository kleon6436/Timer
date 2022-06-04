using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Reactive.Disposables;
using Kchary.Timer.Models;
using System;

namespace Kchary.Timer.ViewModels
{
    public sealed class MainWindowViewModel : BindableBase, IDisposable
    {
        private readonly CompositeDisposable disposables = new();

        /// <summary>
        /// タイマーに表示する数字のデフォルト値
        /// </summary>
        /// <remarks>
        /// 2桁表示であることに注意
        /// </remarks>
        private const string DefaultValue = "00";

        #region Command
        public ReactiveCommand MinutePlusCommand { get; }
        public ReactiveCommand MinuteMinusCommand { get; }
        public ReactiveCommand SecondPlusCommand { get; }
        public ReactiveCommand SecondMinusCommand { get; }
        public ReactiveCommand StartButtonCommand { get; }
        public ReactiveCommand StopButtonCommand { get; }
        public ReactiveCommand ResetButtonCommand { get; }
        #endregion

        /// <summary>
        /// 分テキスト
        /// </summary>
        public ReactivePropertySlim<string> MinuteText { get; } = new();

        /// <summary>
        /// 秒テキスト
        /// </summary>
        public ReactivePropertySlim<string> SecondText { get; } = new();

        /// <summary>
        /// タイマーコントローラー
        /// </summary>
        public TimerController TimerController { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            // コマンドの設定
            MinutePlusCommand = new ReactiveCommand().WithSubscribe(MinutePlusCommandClicked).AddTo(disposables);
            MinuteMinusCommand = new ReactiveCommand().WithSubscribe(MinuteMinusCommandClicked).AddTo(disposables);
            SecondPlusCommand = new ReactiveCommand().WithSubscribe(SecondPlusCommandClicked).AddTo(disposables);
            SecondMinusCommand = new ReactiveCommand().WithSubscribe(SecondMinusCommandClicked).AddTo(disposables);
            StartButtonCommand = new ReactiveCommand().WithSubscribe(StartButtonCommandClicked).AddTo(disposables);
            StopButtonCommand = new ReactiveCommand().WithSubscribe(StopButtonCommandClicked).AddTo(disposables);
            ResetButtonCommand = new ReactiveCommand().WithSubscribe(ResetButtonCommandClicked).AddTo(disposables);

            // コントローラーの設定
            TimerController = new TimerController();
            TimerController.TimerEvent += UpdateTimerValue;

            // 初期値設定
            MinuteText.Value = DefaultValue;
            SecondText.Value = DefaultValue;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() => disposables.Dispose();

        /// <summary>
        /// タイマーの状態を確認し、停止処理を行う
        /// </summary>
        public void CheckAndStopTimer()
        {
            if (TimerController.TimerStatus != TimerStatus.Processing)
            {
                return;
            }

            TimerController.StopTimer();
        }

        /// <summary>
        /// 分のプラスボタンを押下したとき
        /// </summary>
        private void MinutePlusCommandClicked()
        {
            var timerValue = TimerController.PlusMinute();
            UpdateTimerValue(timerValue);
        }

        /// <summary>
        /// 分のマイナスボタンを押下したとき
        /// </summary>
        private void MinuteMinusCommandClicked()
        {
            var timerValue = TimerController.MinusMinute();
            UpdateTimerValue(timerValue);
        }

        /// <summary>
        /// 秒のプラスボタンを押下したとき
        /// </summary>
        private void SecondPlusCommandClicked()
        {
            var timerValue = TimerController.PlusSecond();
            UpdateTimerValue(timerValue);
        }

        /// <summary>
        /// 秒のマイナスボタンを押下したとき
        /// </summary>
        private void SecondMinusCommandClicked()
        {
            var timerValue = TimerController.MinusSecond();
            UpdateTimerValue(timerValue);
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
            CheckAndStopTimer();
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
            MinuteText.Value = TimerController.ConvertTimerValueToStr(timerValue.Minute);
            SecondText.Value = TimerController.ConvertTimerValueToStr(timerValue.Second);
        }
    }
}