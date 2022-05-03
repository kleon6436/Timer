namespace Kchary.Timer.Models
{
    /// <summary>
    /// タイマー値の管理クラス
    /// </summary>
    public sealed record TimerValue
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="minute">分値</param>
        /// <param name="second">秒値</param>
        public TimerValue(int minute, int second)
        {
            Minute = minute;
            Second = second;
        }

        /// <summary>
        /// 分値
        /// </summary>
        public int Minute { get; set; }

        /// <summary>
        /// 秒値
        /// </summary>
        public int Second { get; set; }
    }
}