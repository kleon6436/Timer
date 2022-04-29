namespace Kchary.Timer.Models
{
    /// <summary>
    /// タイマー値の管理クラス
    /// </summary>
    public sealed record TimerValue
    {
        public TimerValue(int minute, int second)
        {
            Minute = minute;
            Second = second;
        }

        public int Minute { get; set; }
        public int Second { get; set; }
    }
}