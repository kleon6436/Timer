using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;

namespace Kchary.Timer.Models
{
    /// <summary>
    /// タイマーの状態遷移
    /// </summary>
    public enum TimerStatus
    {
        Standby,
        Processing,
    }

    public sealed class TimerController
    {
        /// <summary>
        /// 音程を表す周波数と長さ、タイマーの最大・最小値の固定値
        /// </summary>
        private const int CFreq = 262; // ド
        private const int EFreq = 330; // ミ
        private const int GFreq = 392; // ソ
        private const int Duration = 500;
        private const int MaxMinuteValue = 99;
        private const int MinMinuteValue = 0;
        private const int MaxSecondValue = 59;
        private const int MinSecondValue = 0;

        /// <summary>
        /// タイマー値
        /// </summary>
        private TimerValue TimerValue { get; }

        /// <summary>
        /// タイマー
        /// </summary>
        private System.Timers.Timer Timer { get; }

        /// <summary>
        /// タイマーの状態
        /// </summary>
        public TimerStatus TimerStatus { get; set; }

        /// <summary>
        /// Timerイベント
        /// </summary>
        public delegate void TimerEventHandler(TimerValue timerValue);
        public event TimerEventHandler TimerEvent;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TimerController()
        {
            // デフォルト値設定
            const int DefaultMinuteTimer = 0;
            const int DefaultSecondTimer = 0;
            TimerValue = new TimerValue(DefaultMinuteTimer, DefaultSecondTimer);
            TimerStatus = TimerStatus.Standby;

            // タイマーの初期化とイベントハンドラー登録
            Timer = new System.Timers.Timer(1000)
            {
                Enabled = false
            };
            Timer.Elapsed += OnElapsedTimer;
        }

        /// <summary>
        /// 1分タイマー値にプラス
        /// </summary>
        public TimerValue PlusMinute()
        {
            // タイマー値の最大値より小さい場合は＋１
            if (TimerValue.Minute < MaxMinuteValue)
            {
                TimerValue.Minute += 1;
            }

            return TimerValue;
        }

        /// <summary>
        /// 1分タイマー値からマイナス
        /// </summary>
        public TimerValue MinusMinute()
        {
            // タイマー値の最小値より大きい場合は-１
            if (TimerValue.Minute > MinMinuteValue)
            {
                TimerValue.Minute -= 1;
            }

            return TimerValue;
        }

        /// <summary>
        /// タイマーの値に1秒を足す
        /// </summary>
        public TimerValue PlusSecond()
        {
            if (TimerValue.Second >= MaxSecondValue)
            {
                if (TimerValue.Minute >= MaxMinuteValue)
                {
                    // 分も最大値だったら更新せず、元の値を返す
                    return TimerValue;
                }
                else
                {
                    // 分が最大値出ない場合は、秒 => 0、分に1を追加する
                    TimerValue.Second = 0;
                    TimerValue.Minute += 1;
                    return TimerValue;
                }
            }
            else
            {
                // 秒の値に余裕がある場合は、秒に1を足す
                TimerValue.Second += 1;
                return TimerValue;
            }
        }

        /// <summary>
        /// 1秒タイマー値からマイナス
        /// </summary>
        public TimerValue MinusSecond()
        {
            if (TimerValue.Second <= MinSecondValue)
            {
                if (TimerValue.Minute <= MinMinuteValue)
                {
                    // 分も最小値だったら更新せず、元の値を返す
                    return TimerValue;
                }
                else
                {
                    // 分が最小値出ない場合は、秒 => 59、分 => -1を追加する
                    TimerValue.Second = 59;
                    TimerValue.Minute -= 1;
                    return TimerValue;
                }
            }
            else
            {
                // 秒の値に余裕がある場合は、秒から1を引く
                TimerValue.Second -= 1;
                return TimerValue;
            }
        }

        /// <summary>
        /// タイマー値を文字列に直した値を取得する
        /// </summary>
        /// <param name="timerValue">タイマー値</param>
        /// <returns>タイマー値を文字列に変換した結果</returns>
        public static string ConvertTimerValueToStr(int timerValue)
        {
            // タイマーの値が10以下の場合は2桁目に0をつける
            return timerValue < 10 ? $"0{timerValue}" : timerValue.ToString();
        }

        /// <summary>
        /// タイマーをスタート
        /// </summary>
        public void StartTimer()
        {
            // 設定されたタイマーの値が分、秒どちらも0以下の場合は何もしない
            if (TimerValue.Minute <= MinMinuteValue && TimerValue.Second <= MinSecondValue)
            {
                return;
            }

            // タイマーを開始
            TimerStatus = TimerStatus.Processing;
            Timer.Start();
        }

        /// <summary>
        /// タイマーをストップ
        /// </summary>
        public void StopTimer()
        {
            Timer.Stop();
            TimerStatus = TimerStatus.Standby;
        }

        /// <summary>
        /// タイマーをリセット
        /// </summary>
        public void ResetTimer()
        {
            // 動作中のタイマーをリセット
            if (Timer.Enabled)
            {
                Timer.Stop();
            }

            // タイマーを最小値に戻す
            TimerValue.Minute = MinMinuteValue;
            TimerValue.Second = MinSecondValue;

            TimerEvent?.Invoke(TimerValue);
        }

        /// <summary>
        /// ドミソミドと音を鳴らす
        /// </summary>
        private static void BeepSound()
        {
            NativeMethods.Beep(CFreq, Duration); // ド
            NativeMethods.Beep(EFreq, Duration); // ミ
            NativeMethods.Beep(GFreq, Duration); // ソ
            NativeMethods.Beep(EFreq, Duration); // ミ
            NativeMethods.Beep(CFreq, Duration); // ド
        }

        /// <summary>
        /// タイマー動作中の動作
        /// </summary>
        private void OnElapsedTimer(object sender, ElapsedEventArgs e)
        {
            if (TimerStatus != TimerStatus.Processing)
            {
                // 処理中でないときは、以降の処理はスキップ
                return;
            }

            if (TimerValue.Minute > MinMinuteValue && TimerValue.Second == MinSecondValue)
            {
                // 1分減らして59秒にする
                TimerValue.Minute -= 1;
                TimerValue.Second = MaxSecondValue;
            }
            else
            {
                // 1秒減らす
                TimerValue.Second -= 1;
            }

            Application.Current.Dispatcher.Invoke(() => 
            {
                if (TimerStatus != TimerStatus.Processing)
                {
                    // 処理中でないときは、以降の処理はスキップ
                    return;
                }
                TimerEvent?.Invoke(TimerValue);
            });

            // どちらも0になった場合はタイマー終了
            if (TimerValue.Minute == MinMinuteValue && TimerValue.Second == MinSecondValue)
            {
                // タイマーを終了
                Timer.Stop();

                // ドミソミドと音を鳴らす
                BeepSound();
            }
        }

        /// <summary>
        /// ネイティブのdllのメソッドをロードするクラス
        /// </summary>
        private static class NativeMethods
        {
            [DllImport("Kernel32.dll")]
            internal static extern bool Beep(int dwFreq, int dwDuration);
        }
    }
}