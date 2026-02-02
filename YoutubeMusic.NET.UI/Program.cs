using YoutubeMusic.NET.Services;
using YoutubeMusic.NET.Common.Source.Interfaces;
using YoutubeMusic.NET.Common.Models;
using YoutubeMusic.NET.UI;
using YoutubeMusic.NET.Common.Services;
using System.Runtime.InteropServices;

namespace YoutubeMusic.NET;

static class Program
{
    internal const string AppName = "YoutubeMusic.NET";
    private static ConfigurationService? _configService;
    
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool AllocConsole();
    
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            // Allocate console window
#if DEBUG
            AllocConsole();
#else
            if (args.Any(a => a.Equals("/console", StringComparison.OrdinalIgnoreCase) || a.Equals("--console", StringComparison.OrdinalIgnoreCase)))
            {
                AllocConsole();
            }
#endif
            SimpleLogger.Info("=== YoutubeMusic.NET Console ===");
            
            // Initialize configuration service first
            _configService = new ConfigurationService();
            
            SimpleLogger.Info("=== YoutubeMusic.NET Application Starting ===");
            SimpleLogger.Info($"Operating System: {Environment.OSVersion}");
            SimpleLogger.Info($".NET Version: {Environment.Version}");
            SimpleLogger.Info($"Working Directory: {Environment.CurrentDirectory}");
            
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            SimpleLogger.Debug("Application configuration initialized");
            
            SimpleLogger.Info("Starting main application form");
            Application.Run(new MainForm(_configService));
            
            SimpleLogger.Info("Application form closed, main thread ending");
        }
        catch (Exception ex)
        {
            SimpleLogger.Fatal(ex, "Fatal error during application startup");
            MessageBox.Show($"Fatal error during startup: {ex.Message}", "Application Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SimpleLogger.Info("=== YoutubeMusic.NET Application Ended ===");
        }
    }    
}
