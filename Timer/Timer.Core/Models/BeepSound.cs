using System.Runtime.InteropServices;

namespace Timer.Core.Models;

public static class BeepSound
{
    // 音程を表す周波数と長さ
    private const int CFreq = 262; // ド
    private const int EFreq = 330; // ミ
    private const int GFreq = 392; // ソ
    private const int Duration = 500;

    /// <summary>
    /// ネイティブのdllのメソッドをロードするクラス
    /// </summary>
    private static class NativeMethods
    {
        [DllImport("Kernel32.dll")]
        internal static extern bool Beep(int dwFreq, int dwDuration);
    }

    /// <summary>
    /// ドミソミドと音を鳴らす
    /// </summary>
    public static void Beep()
    {
        NativeMethods.Beep(CFreq, Duration); // ド
        NativeMethods.Beep(EFreq, Duration); // ミ
        NativeMethods.Beep(GFreq, Duration); // ソ
        NativeMethods.Beep(EFreq, Duration); // ミ
        NativeMethods.Beep(CFreq, Duration); // ド
    }
}
