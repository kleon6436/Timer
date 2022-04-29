using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;

namespace Kchary.Timer.Models
{
    public sealed class TimerController
    {
        /// <summary>
        /// Timerイベント
        /// </summary>
        /// <param name="timerValue">Timer value</param>
        public delegate void TimerEventHandler(TimerValue timerValue);
        public event TimerEventHandler TimerEvent;

        /// <summary>
        /// 音程を表す周波数と長さ、タイマーの最大・最小値の固定値
        /// </summary>
        private const int CFreq = 262; // ド
        private const int EFreq = 330; // ミ
        private const int GFreq = 392; // ソ
        private const int Duration = 500;
        private const int MaxTimerValue = 59;
        private const int MinTimerValue = 0;

        /// <summary>
        /// タイマー値
        /// </summary>
        private TimerValue TimerValue { get; }

        /// <summary>
        /// タイマー
        /// </summary>
        private System.Timers.Timer Timer { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TimerController()
        {
            // デフォルト値設定
            const int DefaultMinuteTimer = 0;
            const int DefaultSecondTimer = 0;
            TimerValue = new TimerValue(DefaultMinuteTimer, DefaultSecondTimer);

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
        public string PlusMinute()
        {
            // タイマー値の最大値より小さい場合は＋１
            if (TimerValue.Minute > MaxTimerValue)
            {
                TimerValue.Minute += 1;
            }

            return GetTimerValue(TimerValue.Minute);
        }

        /// <summary>
        /// 1分タイマー値からマイナス
        /// </summary>
        public string MinusMinute()
        {
            // タイマー値の最小値より大きい場合は-１
            if (TimerValue.Minute > MinTimerValue)
            {
                TimerValue.Minute -= 1;
            }

            return GetTimerValue(TimerValue.Minute);
        }

        /// <summary>
        /// 1秒タイマー値にプラス
        /// </summary>
        public string PlusSecond()
        {
            // タイマー値の最大値より小さい場合は＋１
            if (TimerValue.Second < MaxTimerValue)
            {
                TimerValue.Second += 1;
            }

            return GetTimerValue(TimerValue.Second);
        }

        /// <summary>
        /// 1秒タイマー値からマイナス
        /// </summary>
        public string MinusSecond()
        {
            // タイマー値の最小値より大きい場合は-１
            if (TimerValue.Second > MinTimerValue)
            {
                TimerValue.Second -= 1;
            }

            return GetTimerValue(TimerValue.Second);
        }

        /// <summary>
        /// タイマー値を取得する
        /// </summary>
        /// <param name="timerValue">タイマー値</param>
        /// <returns>タイマー値を文字列に変換した結果</returns>
        public static string GetTimerValue(int timerValue)
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
            if (TimerValue.Minute <= MinTimerValue && TimerValue.Second <= MinTimerValue)
            {
                return;
            }

            // タイマーを開始
            Timer.Start();
        }

        /// <summary>
        /// タイマーをストップ
        /// </summary>
        public void StopTimer()
        {
            Timer.Stop();
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
            TimerValue.Minute = MinTimerValue;
            TimerValue.Second = MinTimerValue;

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
            if (TimerValue.Minute > MinTimerValue && TimerValue.Second == MinTimerValue)
            {
                // 1分減らして59秒にする
                TimerValue.Minute -= 1;
                TimerValue.Second = MaxTimerValue;
            }
            else
            {
                // 1秒減らす
                TimerValue.Second -= 1;
            }

            Application.Current.Dispatcher.Invoke(() => { TimerEvent?.Invoke(TimerValue); });

            // どちらも0になった場合はタイマー終了
            if (TimerValue.Minute == MinTimerValue && TimerValue.Second == MinTimerValue)
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