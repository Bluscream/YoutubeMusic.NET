using YoutubeMusic.NET.Services;
using YoutubeMusic.NET.Common.Source.Interfaces;
using YoutubeMusic.NET.Common.Models;
using YoutubeMusic.NET.Common.Services;

namespace YoutubeMusic.NET.UI;

public partial class MainForm
{


    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        SimpleLogger.Info($"=== {Program.AppName} Shutting Down ===");
        SimpleLogger.Debug("Cleaning up resources before application exit");
        

        

        
        // Dispose Windows Media Service
        _windowsMediaService?.Dispose();
        
        // Dispose Global Hotkeys service
        _globalHotkeys?.Dispose();
        
        // Dispose toast notification service
        _toastNotificationService?.Dispose();
        
        // Queue caching removed - no longer saving queue
        
        _musicPlayerService?.Stop();
        _progressTimer?.Stop();
        _progressTimer?.Dispose();
        
        SimpleLogger.Info("Application shutdown completed successfully");
        base.OnFormClosing(e);
    }
}
