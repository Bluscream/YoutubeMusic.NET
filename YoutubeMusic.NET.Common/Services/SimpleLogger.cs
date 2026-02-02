namespace YoutubeMusic.NET.Common.Services;

public static class SimpleLogger
{
    private static void Log(string level, string message, Exception? ex = null)
    {
        var logMessage = $"{level}|{DateTime.Now:dd.MM HH:mm:ss.f}|{message}";
        if (ex != null) logMessage += $"|{ex}";
        Console.WriteLine(logMessage);
    }

    public static void Info(string message) => Log("INFO", message);
    public static void Debug(string message) => Log("DEBUG", message);
    public static void Debug(Exception ex, string message) => Log("DEBUG", message, ex);
    public static void Warn(string message) => Log("WARN", message);
    public static void Warn(Exception ex, string message) => Log("WARN", message, ex);
    public static void Error(string message) => Log("ERROR", message);
    public static void Error(Exception ex, string message) => Log("ERROR", message, ex);
    public static void Fatal(Exception ex, string message) => Log("FATAL", message, ex);
}
