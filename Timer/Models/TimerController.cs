using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;

namespace Kchary.Timer.Models
{
    public class TimerController
    {
        /// <summary>
        /// Timerイベント
        /// </summary>
        /// <param name="timerValue">Timer value</param>
        public delegate void TimerEventHandler(TimerValue timerValue);

        /// <summary>
        /// 音程を表す周波数と長さ、タイマーの最大・最小値の固定値
        /// </summary>
        private const int Cfreq = 262; // ド
        private const int Efreq = 330; // ミ
        private const int Gfreq = 392; // ソ
        private const int Duration = 500;
        private const int MaxTimerValue = 59;
        private const int MinTimerValue = 0;

        private System.Timers.Timer _timerCount;


        public TimerValue TimerValue;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TimerController()
        {
            // デフォルトの値をセット
            const int defaultMinuteTimer = 0;
            const int defaultSecondTimer = 0;
            TimerValue = new TimerValue(defaultMinuteTimer, defaultSecondTimer);
        }

        public event TimerEventHandler TimerEvent;

        /// <summary>
        /// 1分タイマー値にプラス
        /// </summary>
        public string PlusMinute()
        {
            // タイマー値の最大値より小さい場合は＋１
            if (TimerValue.Minute < MaxTimerValue) TimerValue.Minute += 1;

            return GetStringValueFromTimerValue(TimerValue.Minute);
        }

        /// <summary>
        /// 1分タイマー値からマイナス
        /// </summary>
        public string MinusMinute()
        {
            // タイマー値の最小値より大きい場合は-１
            if (TimerValue.Minute > MinTimerValue) TimerValue.Minute -= 1;

            return GetStringValueFromTimerValue(TimerValue.Minute);
        }

        /// <summary>
        /// 1秒タイマー値にプラス
        /// </summary>
        public string PlusSecond()
        {
            // タイマー値の最大値より小さい場合は＋１
            if (TimerValue.Second < MaxTimerValue) TimerValue.Second += 1;

            return GetStringValueFromTimerValue(TimerValue.Second);
        }

        /// <summary>
        /// 1秒タイマー値からマイナス
        /// </summary>
        public string MinusSecond()
        {
            // タイマー値の最小値より大きい場合は-１
            if (TimerValue.Second > MinTimerValue) TimerValue.Second -= 1;

            return GetStringValueFromTimerValue(TimerValue.Second);
        }

        /// <summary>
        /// タイマー値を文字列に変換する
        /// </summary>
        /// <param name="timerValue">タイマー値</param>
        /// <returns>タイマー値を文字列に変換した結果</returns>
        public static string GetStringValueFromTimerValue(int timerValue)
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
            if (TimerValue.Minute <= MinTimerValue && TimerValue.Second <= MinTimerValue) return;

            if (_timerCount != null) return;

            _timerCount = new System.Timers.Timer(1000); // 1秒ごとの処理にセット

            // タイマーでの処理をイベントハンドラーとして登録
            _timerCount.Elapsed += OnElapsed_TimersTimer;

            // タイマーを開始
            _timerCount.Start();
        }

        /// <summary>
        /// タイマー動作中の動作
        /// </summary>
        private void OnElapsed_TimersTimer(object sender, ElapsedEventArgs e)
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

            // UIに反映(別スレッドなので、Dispatcherを利用)
            Application.Current.Dispatcher.Invoke(() => { TimerEvent?.Invoke(TimerValue); });

            // どちらも0になった場合はタイマー終了
            if (TimerValue.Minute != MinTimerValue || TimerValue.Second != MinTimerValue) return;

            // タイマーを終了
            _timerCount.Stop();
            _timerCount.Dispose();
            _timerCount = null;

            // ドミソミドと音を鳴らす
            BeepSound();
        }

        /// <summary>
        /// タイマーをストップ
        /// </summary>
        public void StopTimer()
        {
            if (_timerCount == null)
            {
                return;
            }

            _timerCount.Stop();
            _timerCount.Dispose();
            _timerCount = null;
        }

        /// <summary>
        /// タイマーをリセット
        /// </summary>
        public void ResetTimer()
        {
            // タイマーが動作中の場合
            if (_timerCount != null)
            {
                // タイマーを停止
                _timerCount.Stop();
                _timerCount.Dispose();
                _timerCount = null;
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
            NativeMethods.Beep(Cfreq, Duration); // ド
            NativeMethods.Beep(Efreq, Duration); // ミ
            NativeMethods.Beep(Gfreq, Duration); // ソ
            NativeMethods.Beep(Efreq, Duration); // ミ
            NativeMethods.Beep(Cfreq, Duration); // ド
        }

        internal class NativeMethods
        {
            [DllImport("Kernel32.dll")]
            internal static extern bool Beep(int dwFreq, int dwDuration);
        }
    }
}