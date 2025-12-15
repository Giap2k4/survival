using System;

public static class TimeManager
{
    /// <summary>
    /// lấy thời gian hiện tại ở local
    /// </summary>
    /// <returns></returns>
    public static long GetNow()
    {
        return (DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }

    /// <summary>
    /// Hiển thị thời gian đã được format
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static string FormatTime(long seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);

        if (seconds >= 86400)
            return $"{t.Days}d {t.Hours:D2}h";
        if (seconds >= 3600)
            return $"{t.Hours:D2}:{t.Minutes:D2}:{t.Seconds:D2}";

        return $"{t.Minutes:D2}:{t.Seconds:D2}";
    }
}
