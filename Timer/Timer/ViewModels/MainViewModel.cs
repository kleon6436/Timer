using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kchary.Timer.Models;
using Microsoft.UI.Xaml;
using Timer;
using Timer.Core.Models;

namespace Kchary.Timer.ViewModels;

/// <summary>
/// タイマーの状態遷移
/// </summary>
public enum TimerStatus
{
    Standby,
    Processing,
}

public sealed class MainViewModel : ObservableRecipient
{
    #region コマンド

    private IRelayCommand? minutePlusCommand;
    /// <summary>
    ///  分のプラスボタンを押下したときのコマンド
    /// </summary>
    public IRelayCommand MinutePlusCommand => minutePlusCommand ??= new RelayCommand(MinutePlusClicked);

    private IRelayCommand? minuteMinusCommand;
    /// <summary>
    /// 分のマイナスボタンを押下したときのコマンド
    /// </summary>
    public IRelayCommand MinuteMinusCommand => minuteMinusCommand ??= new RelayCommand(MinuteMinusClicked);

    private IRelayCommand? secondPlusCommand;
    /// <summary>
    /// 秒のプラスボタンを押下したときのコマンド
    /// </summary>
    public IRelayCommand SecondPlusCommand => secondPlusCommand ??= new RelayCommand(SecondPlusClicked);

    private IRelayCommand? secondMinusCommand;
    /// <summary>
    /// 秒のマイナスボタンを押下したときのコマンド
    /// </summary>
    public IRelayCommand SecondMinusCommand => secondMinusCommand ??= new RelayCommand(SecondMinusClicked);

    private IRelayCommand? startButtonCommand;
    /// <summary>
    /// スタートボタンを押下したときのコマンド
    /// </summary>
    public IRelayCommand StartButtonCommand => startButtonCommand ??= new RelayCommand(StartButtonClicked);

    private IRelayCommand? stopButtonCommand;
    /// <summary>
    /// ストップボタンを押下したときのコマンド
    /// </summary>
    public IRelayCommand StopButtonCommand => stopButtonCommand ??= new RelayCommand(StopButtonClicked);

    private IRelayCommand? resetButtonCommand;
    /// <summary>
    ///  リセットボタンを押下したときのコマンド
    /// </summary>
    public IRelayCommand ResetButtonCommand => resetButtonCommand ??= new RelayCommand(ResetButtonClicked);

    #endregion

    /// <summary>
    /// タイマー
    /// </summary>
    private DispatcherTimer Timer { get; }

    /// <summary>
    /// タイマー値
    /// </summary>
    public TimerValue TimerValue { get; }

    /// <summary>
    /// タイマーの状態
    /// </summary>
    public TimerStatus TimerStatus { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MainViewModel()
    {
        // デフォルト値設定
        TimerValue = App.GetService<TimerValue>();
        TimerStatus = TimerStatus.Standby;

        // タイマーの初期化とイベントハンドラー登録
        Timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(1000),
        };
        Timer.Tick += Timer_Tick;
    }

    /// <summary>
    /// 分のプラスボタンを押下したとき
    /// </summary>
    private void MinutePlusClicked()
    {
        TimerValue.PlusMinute();
    }

    /// <summary>
    /// 分のマイナスボタンを押下したとき
    /// </summary>
    private void MinuteMinusClicked()
    {
        TimerValue.MinusMinute();
    }

    /// <summary>
    /// 秒のプラスボタンを押下したとき
    /// </summary>
    private void SecondPlusClicked()
    {
        TimerValue.PlusSecond();
    }

    /// <summary>
    /// 秒のマイナスボタンを押下したとき
    /// </summary>
    private void SecondMinusClicked()
    {
        TimerValue.MinusSecond();
    }

    /// <summary>
    /// スタートボタンを押下したとき
    /// </summary>
    private void StartButtonClicked()
    {
        // 設定されたタイマーの値が分、秒どちらも0以下の場合は何もしない
        if (TimerValue.IsTimerValueSet)
        {
            return;
        }

        // タイマーを開始
        TimerStatus = TimerStatus.Processing;
        Timer.Start();
    }

    /// <summary>
    /// ストップボタンを押下したとき
    /// </summary>
    private void StopButtonClicked()
    {
        if (TimerStatus != TimerStatus.Processing)
        {
            return;
        }

        Timer.Stop();
        TimerStatus = TimerStatus.Standby;
    }

    /// <summary>
    /// リセットボタンを押下したとき
    /// </summary>
    private void ResetButtonClicked()
    {
        // 動作中のタイマーをリセット
        if (Timer.IsEnabled)
        {
            Timer.Stop();
        }

        // タイマーを最小値に戻す
        TimerValue.Reset();
    }

    /// <summary>
    /// タイマー動作中の動作
    /// </summary>
    private async void Timer_Tick(object? sender, object e)
    {
        if (TimerStatus != TimerStatus.Processing)
        {
            // 処理中でないときは、以降の処理はスキップ
            return;
        }

        // タイマー値をアップデートする
        TimerValue.Update();

        // どちらも0になった場合はタイマー終了
        if (TimerValue.IsTimerEnd)
        {
            // タイマーを終了
            Timer.Stop();

            // ドミソミドと音を鳴らす
            await Task.Delay(500);
            BeepSound.Beep();
        }
    }
}