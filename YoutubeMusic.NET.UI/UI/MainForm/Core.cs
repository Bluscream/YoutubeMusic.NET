using YoutubeMusic.NET.Services;
using YoutubeMusic.NET.Common.Models;
using YoutubeMusic.NET.Common.Utils;
using YoutubeMusic.NET.Common.Services;
using YoutubeMusic.NET.Common.Source.Interfaces;
using YoutubeMusic.NET.Common.Source;

namespace YoutubeMusic.NET.UI;

public partial class MainForm
{
    private async Task InitializeServicesAsync()
    {
        SimpleLogger.Info("Initializing services...");
        
        try
        {
            // Check and validate authorization before initializing services
            if (!await CheckAndRequestAuthorizationAsync())
            {
                SimpleLogger.Warn("Authorization cancelled or failed. Application will continue with limited functionality.");
                MessageBox.Show(
                    "YouTube Music authorization is required for full functionality.\n\n" +
                    "You can authorize later from the Settings menu.",
                    "Authorization Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }
            
            // Create YouTube Music source provider
            var youtubeMusicProvider = new YouTubeMusicSourceProvider(ConfigurationService.Current);
            
            // Initialize the provider with timeout
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            try
            {
                await youtubeMusicProvider.InitializeAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                SimpleLogger.Warn("Service initialization timed out after 10 seconds. Continuing with limited functionality.");
            }
            
            // Store the source provider for later use
            _sourceProvider = youtubeMusicProvider;
            
            // Use YouTube Music services directly
            _searchService = youtubeMusicProvider.SearchService;
            _metadataService = youtubeMusicProvider.MetadataService;
            _playbackService = new NAudioPlaybackService();
            
            // Create main music player service (streaming only)
            _musicPlayerService = new MusicPlayerService(_searchService, _metadataService, null, _playbackService);
            UpdateServicesConfiguration();
            
            // Initialize Windows Media Service for system media controls
            try
            {
                _windowsMediaService = new WindowsMediaService(_musicPlayerService, _configService ?? throw new InvalidOperationException("Configuration service is not initialized"), this.Handle);
                
                // Wire up media command handling
                _windowsMediaService.MediaCommandReceived += OnMediaCommandReceived;
                
                SimpleLogger.Info("Windows Media Service initialized");
            }
            catch (Exception ex)
            {
                SimpleLogger.Error(ex, "Failed to initialize Windows Media Service - system media controls will not be available");
            }
            
            // Initialize Global Hotkeys as fallback for media keys
            if (ConfigurationService.Current.EnableGlobalHotkeysFallback)
            {
                try
                {
                    _globalHotkeys = new GlobalHotkeys(_musicPlayerService, _configService ?? throw new InvalidOperationException("Configuration service is not initialized"), this.Handle);
                    
                    // Wire up media command handling for global hotkeys
                    _globalHotkeys.MediaCommandReceived += OnMediaCommandReceived;
                    
                    SimpleLogger.Info("Global Hotkeys initialized as media keys fallback");
                }
                catch (Exception ex)
                {
                    SimpleLogger.Error(ex, "Failed to initialize Global Hotkeys - media keys fallback will not be available");
                }
            }
            else
            {
                SimpleLogger.Info("Global Hotkeys fallback disabled in configuration");
            }
            
            // Wire up music player events
            _musicPlayerService.SongChanged += OnSongChanged;
            _musicPlayerService.PlaybackStateChanged += OnPlaybackStateChanged;
            _musicPlayerService.PositionChanged += OnPositionChanged;
            _musicPlayerService.VolumeChanged += OnVolumeChanged;
            _musicPlayerService.PlaybackCompleted += OnPlaybackCompleted;
            _musicPlayerService.PlaybackError += OnPlaybackError;
            
            // Wire up queue events
            _queue.OnRepeatModeChanged += OnQueueRepeatModeChanged;
            _queue.OnShuffleChanged += OnQueueShuffleChanged;
            _queue.OnSongsChanged += OnQueueSongsChanged;
            _queue.OnCurrentIndexChanged += OnQueueCurrentIndexChanged;
            
            // Load playlists after services are initialized
            LoadPlaylists();
            
            // Initialize toast notification service
            InitializeToastNotifications();
            
            // Initialize lyrics service
            _lyricsService = new LyricsService();
            SimpleLogger.Info("Lyrics service initialized");
                
            SimpleLogger.Info("All services initialized successfully");
            SimpleLogger.Info("Ready! You can now search for music on YouTube Music.");
            
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to initialize services");
            MessageBox.Show($"Failed to initialize services: {ex.Message}", "Initialization Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task PerformSearch()
    {
        if (string.IsNullOrWhiteSpace(searchTextBox.Text))
        {
            SimpleLogger.Debug("Search skipped: empty search text");
            return;
        }
        
        var query = searchTextBox.Text.Trim();
        SimpleLogger.Info($"Starting search for: '{query}'");
        
        try
        {
            SimpleLogger.Info("Searching across all enabled sources...");
            
            var songs = await (_musicPlayerService?.SearchAsync(query, 50) ?? Task.FromResult(new List<Song>()));
            
            // Update the search results using data binding
            UpdateSearchResults(songs);
            
            // Restore the search text box placeholder
            searchTextBox.PlaceholderText = "Search for songs, artists, albums...";
            
            // Get unique sources from the results
            var sources = songs.Where(s => !string.IsNullOrEmpty(s.Source)).Select(s => s.Source).Distinct().ToList();
            var sourceText = sources.Count > 0 ? $" from {string.Join(", ", sources)}" : "";
            
            SimpleLogger.Info($"Search completed: Found {songs.Count} songs{sourceText}");
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, $"Search failed for query: '{query}'");
            MessageBox.Show($"Search failed: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }
    }

    private void OnQueueSongsChanged()
    {
        // Refresh highlighting when queue changes
        RefreshAllListViewHighlighting();
    }

    private void OnQueueCurrentIndexChanged(object? sender, int currentIndex)
    {
        // Queue state changed
    }



    private void InitializeToastNotifications()
    {
        try
        {
            _toastNotificationService = new ToastNotificationService();
            SimpleLogger.Info("Toast notification service initialized successfully");
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to initialize toast notification service");
        }
    }


    private void OnMediaCommandReceived(object? sender, MediaCommand command)
    {
        try
        {
            // Handle media commands from Windows Media Session
            switch (command)
            {
                case MediaCommand.Play:
                case MediaCommand.Pause:
                    this.BeginInvoke(OnPlayPauseButtonClick);
                    break;
                    
                case MediaCommand.Stop:
                    this.BeginInvoke(() => _musicPlayerService?.Stop());
                    break;
                    
                case MediaCommand.Next:
                    this.BeginInvoke(async () => await PlayNextSong());
                    break;
                    
                case MediaCommand.Previous:
                    this.BeginInvoke(async () => await PlayPreviousSong());
                    break;
                    
                case MediaCommand.VolumeUp:
                    this.BeginInvoke(() => AdjustVolume(10));
                    break;
                    
                case MediaCommand.VolumeDown:
                    this.BeginInvoke(() => AdjustVolume(-10));
                    break;
                    
                default:
                    SimpleLogger.Warn($"Unhandled media command received: {command}");
                    break;
            }
            
            SimpleLogger.Debug($"Processed media command from Windows Media Session: {command}");
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, $"Failed to handle media command: {command}");
        }
    }
    
    private async Task<bool> CheckAndRequestAuthorizationAsync()
    {
        try
        {
            SimpleLogger.Info("Checking YouTube Music authorization...");
            
            // Load settings
            var settings = new YouTubeMusicSourceSettings();
            await settings.LoadAsync();
            
            // Check if cookies exist
            if (string.IsNullOrWhiteSpace(settings.Cookies))
            {
                SimpleLogger.Info("No cookies found. Requesting authorization...");
                return await RequestAuthorizationAsync(settings);
            }
            
            // Validate existing cookies
            SimpleLogger.Info("Validating existing cookies...");
            var tempProvider = new YouTubeMusicSourceProvider(ConfigurationService.Current);
            await tempProvider.InitializeAsync();
            
            var isValid = await tempProvider.ValidateAuthenticationAsync();
            
            if (!isValid)
            {
                SimpleLogger.Warn("Existing cookies are invalid. Requesting new authorization...");
                return await RequestAuthorizationAsync(settings);
            }
            
            SimpleLogger.Info("Authorization validated successfully.");
            return true;
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to check authorization");
            // If validation fails, request new authorization
            var settings = new YouTubeMusicSourceSettings();
            await settings.LoadAsync();
            return await RequestAuthorizationAsync(settings);
        }
    }
    
    private async Task<bool> RequestAuthorizationAsync(YouTubeMusicSourceSettings settings)
    {
        if (InvokeRequired)
        {
            return await Task.Run(() =>
            {
                bool result = false;
                Invoke(() =>
                {
                    try
                    {
                        using var authForm = new AuthorizationForm(settings);
                        var dialogResult = authForm.ShowDialog(this);
                        result = dialogResult == DialogResult.OK && authForm.IsAuthorized;
                    }
                    catch (Exception ex)
                    {
                        SimpleLogger.Error(ex, "Failed to show authorization form");
                        result = false;
                    }
                });
                return result;
            });
        }
        
        try
        {
            using var authForm = new AuthorizationForm(settings);
            var dialogResult = authForm.ShowDialog(this);
            
            if (dialogResult == DialogResult.OK && authForm.IsAuthorized)
            {
                SimpleLogger.Info("Authorization completed successfully.");
                return true;
            }
            
            SimpleLogger.Info("Authorization cancelled by user.");
            return false;
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to show authorization form");
            return false;
        }
    }

    /// <summary>
    /// Regenerate YouTube Music session tokens
    /// </summary>
    public async Task RegenerateSessionTokensAsync()
    {
        try
        {
            if (_sourceProvider is YouTubeMusicSourceProvider youtubeProvider)
            {
                await youtubeProvider.RegenerateSessionTokensAsync();
                SimpleLogger.Info("Session tokens regenerated successfully");
            }
            else
            {
                SimpleLogger.Warn("YouTube Music source provider not available");
            }
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
    
    /// <summary>
    /// Clear all application data and restart the application
    /// </summary>
    public void ClearApplicationDataAndRestart()
    {
        try
        {
            var result = MessageBox.Show(
                "This will clear all application data including settings, cookies, and cached data.\n\n" +
                "The application will restart after clearing the data.\n\n" +
                "Are you sure you want to continue?",
                "Clear Application Data",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            
            if (result == DialogResult.Yes)
            {
                SimpleLogger.Info("User confirmed clearing application data");
                ApplicationDataManager.ClearAllData();
                
                // Restart the application
                Application.Restart();
            }
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to clear application data");
            MessageBox.Show(
                $"Failed to clear application data: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
}
