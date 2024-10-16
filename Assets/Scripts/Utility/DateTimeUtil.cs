using System;

public static class DateTimeUtil
{
    public static long GetCurTs()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
