using System.IO;
using YoutubeMusic.NET.Common.Utils;
using YoutubeMusic.NET.Common.Services;

namespace YoutubeMusic.NET.Common.Utils;

public static class ApplicationDataManager
{
    /// <summary>
    /// Clear all application data including settings and cached data
    /// </summary>
    public static void ClearAllData()
    {
        try
        {
            var appDataPath = GetAppDataPath();
            
            if (Directory.Exists(appDataPath))
            {
                SimpleLogger.Info($"Clearing application data from: {appDataPath}");
                Directory.Delete(appDataPath, recursive: true);
                SimpleLogger.Info("Application data cleared successfully");
            }
            else
            {
                SimpleLogger.Info("No application data found to clear");
            }
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to clear application data");
            throw;
        }
    }
    
    /// <summary>
    /// Get the application data directory path
    /// </summary>
    public static string GetAppDataPath()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            Main.AppName
        );
    }
}
