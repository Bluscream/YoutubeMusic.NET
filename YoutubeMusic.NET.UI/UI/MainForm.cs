using YoutubeMusic.NET.Services;
using System.Diagnostics;
using YoutubeMusic.NET.Common.Models;
using YoutubeMusic.NET.Common.Services;
using YoutubeMusic.NET.Common.Source.Interfaces;
using YoutubeMusic.NET.Common.Source;
using static YoutubeMusic.NET.Common.Models.PlaybackState;

namespace YoutubeMusic.NET.UI;

public partial class MainForm : Form
{
    private MusicPlayerService? _musicPlayerService;
    private ISearchService? _searchService;
    private IMetadataService? _metadataService;
    private IPlaybackService? _playbackService;
    private ConfigurationService? _configService;
    private YouTubeMusicSourceProvider? _sourceProvider;

    private ToastNotificationService? _toastNotificationService;
    private WindowsMediaService? _windowsMediaService;
    private GlobalHotkeys? _globalHotkeys;
    private LyricsService? _lyricsService;
    
    private List<Song> _searchResults = new();
    private List<Playlist> _playlists = new();
    private Queue _queue = new();
    private CancellationTokenSource? _currentLyricsCancellation;
    private System.Windows.Forms.Timer? _progressTimer;
    
    // Data binding properties for the different views
    // Note: Using manual ListView population instead of data binding for better control
    
    public MainForm(ConfigurationService? configService = null)
    {
        SimpleLogger.Info($"=== {Program.AppName} Starting ===");
        SimpleLogger.Debug("Initializing MainForm components");
        
        // Use the provided services or create new ones if none provided
        _configService = configService ?? new ConfigurationService();
        
        InitializeComponent();
        SimpleLogger.Debug("Form components initialized successfully");
        
        // Set the form icon
        try
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("logo.ico");
            if (stream != null)
            {
                Icon = new Icon(stream);
            }
        }
        catch (Exception ex)
        {
            SimpleLogger.Warn(ex, "Failed to load application icon");
        }
        
        // Apply configuration to UI
        ApplyConfiguration();
        
        // Setup UI event handlers
        SetupEventHandlers();
        
        // Setup data binding
        SetupDataBinding();
        
        // Setup progress timer
        SetupProgressTimer();
        
        // Load playlists will be called after services are initialized
        
        SimpleLogger.Info("MainForm initialization completed successfully");
        
        // Initialize services asynchronously after form is shown
        this.Load += async (s, e) => await InitializeServicesAsync();
    }

    protected override void WndProc(ref Message m)
    {
        // Let the Windows Media Service handle media commands
        if (_windowsMediaService?.ProcessMediaCommand(m) == true)
        {
            return; // Message was handled
        }

        base.WndProc(ref m);
    }
    
    private void SettingsMenuItem_Click(object? sender, EventArgs e)
    {
        try
        {
            using var settingsForm = new SettingsForm();
            var result = settingsForm.ShowDialog(this);
            
            if (result == DialogResult.OK)
            {
                // Reload configuration
                ApplyConfiguration();
                SimpleLogger.Info("Settings updated and applied successfully");
            }
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to open settings form");
            MessageBox.Show(
                "Failed to open settings form. Please check the logs for details.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
    
    private async void RegenerateTokensMenuItem_Click(object? sender, EventArgs e)
    {
        try
        {
            await RegenerateSessionTokensAsync();
            MessageBox.Show(
                "Session tokens regenerated successfully!",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to regenerate session tokens");
            MessageBox.Show(
                "Failed to regenerate session tokens. Please check the logs for details.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
    
    private void ClearDataMenuItem_Click(object? sender, EventArgs e)
    {
        ClearApplicationDataAndRestart();
    }
}
