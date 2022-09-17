using System.ComponentModel;

namespace Kchary.Timer.Models;

/// <summary>
/// タイマー値の管理クラス
/// </summary>
public sealed class TimerValue : INotifyPropertyChanged
{
    // タイマーの最大・最小値の固定値
    private const int MaxMinuteValue = 99;
    private const int MinMinuteValue = 0;
    private const int MaxSecondValue = 59;
    private const int MinSecondValue = 0;

    public event PropertyChangedEventHandler PropertyChanged;

    private int minute = 0;
    /// <summary>
    /// 分値
    /// </summary>
    public int Minute
    {
        get => minute;
        set
        {
            minute = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Minute)));
        }
    }

    private int second = 0;
    /// <summary>
    /// 秒値
    /// </summary>
    public int Second
    {
        get => second;
        set
        {
            second = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Second)));
        }
    }

    /// <summary>
    /// タイマー終了フラグ
    /// </summary>
    /// <returns>True: 終了、False: 継続中</returns>
    public bool IsTimerEnd => Minute == MinMinuteValue && Second == MinSecondValue;

    /// <summary>
    /// タイマー値設定有無フラグ
    /// </summary>
    /// <returns>True: 設定済み、False: 未設定(デフォルト値のまま)</returns>
    public bool IsTimerValueSet => Minute <= MinMinuteValue && Second <= MinSecondValue;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public TimerValue()
    {
    }

    /// <summary>
    /// タイマー値をアップデートする
    /// </summary>
    public void Update()
    {
        if (Minute > MinMinuteValue && Second == MinSecondValue)
        {
            // 1分減らして59秒にする
            Minute--;
            Second = MaxSecondValue;
        }
        else
        {
            // 1秒減らす
            Second--;
        }
    }

    /// <summary>
    /// タイマー値をリセットする
    /// </summary>
    public void Reset()
    {
        Minute = MinMinuteValue;
        Second = MinSecondValue;
    }

    /// <summary>
    /// 1分タイマー値にプラス
    /// </summary>
    public void PlusMinute()
    {
        // タイマー値の最大値より小さい場合は＋１
        if (Minute < MaxMinuteValue)
        {
            Minute++;
        }
    }

    /// <summary>
    /// 1分タイマー値からマイナス
    /// </summary>
    public void MinusMinute()
    {
        // タイマー値の最小値より大きい場合は-１
        if (Minute > MinMinuteValue)
        {
            Minute--;
        }
    }

    /// <summary>
    /// タイマーの値に1秒を足す
    /// </summary>
    public void PlusSecond()
    {
        if (Second >= MaxSecondValue)
        {
            if (Minute < MaxMinuteValue)
            {
                // 分が最大値出ない場合は、秒 => 0、分に1を追加する
                Second = 0;
                Minute++;
            }
        }
        else
        {
            // 秒の値に余裕がある場合は、秒に1を足す
            Second++;
        }
    }

    /// <summary>
    /// 1秒タイマー値からマイナス
    /// </summary>
    public void MinusSecond()
    {
        if (Second <= MinSecondValue)
        {
            if (Minute > MinMinuteValue)
            {
                // 分が最小値出ない場合は、秒 => 59、分 => -1を追加する
                Second = 59;
                Minute--;
            }
        }
        else
        {
            // 秒の値に余裕がある場合は、秒から1を引く
            Second--;
        }
    }
}