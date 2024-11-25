using UnityEngine;

public static class Log
{
    public static void Debug(string str)
    {
        UnityEngine.Debug.Log(PreFrame(str));
    }

    public static void Info(string str)
    {
        UnityEngine.Debug.Log(PreFrame(str));
    }

    public static void Warn(string str)
    {
        UnityEngine.Debug.LogWarning(PreFrame(str));
    }

    public static void Error(string str)
    {
        UnityEngine.Debug.LogError(PreFrame(str));
    }

    public static void Fatal(string str)
    {
        UnityEngine.Debug.LogError(PreFrame(str));
    }

    private static string PreFrame(string str)
    {
        return $"Frame:[{Time.frameCount}]{str}";
    }
}