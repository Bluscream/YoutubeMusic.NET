using YouTubeMusicAPI;
using YouTubeMusicAPI.Client;
using YouTubeSessionGenerator;
using YouTubeSessionGenerator.Js.Environments;
using YoutubeMusic.NET.Common.Models;
using YoutubeMusic.NET.Common.Services;
using YoutubeMusic.NET.Common.Source.Interfaces;
using YoutubeMusic.NET.Common.Source.Services;
using YoutubeMusic.NET.Common.Utils;
using System.Net;

namespace YoutubeMusic.NET.Common.Source;

public class YouTubeMusicSourceProvider : BaseSourceProvider
{
    private readonly Configuration _config;
    private readonly YouTubeMusicSourceSettings _settings;
    private readonly YouTubeMusicSearchService _searchService;
    private readonly YouTubeMusicDownloadService _downloadService;
    private readonly YouTubeMusicMetadataService _metadataService;
    private readonly YouTubeMusicPlaylistService _playlistService;
    private YouTubeMusicClient? _client;
    
    public override string Name => "YouTube Music";
    public override string ShortName => "YTM";
    public override string Description => "YouTube Music streaming source using YouTubeMusicAPI";
    public override string Version => "1.0.0";
    
    public override ISearchService SearchService => _searchService;
    public override IDownloadService DownloadService => _downloadService;
    public override IMetadataService MetadataService => _metadataService;
    public override IPlaylistService PlaylistService => _playlistService;
    public override ISourceSettings? Settings => _settings;
    
    public YouTubeMusicSourceProvider(Configuration config)
    {
        _config = config;
        _settings = new YouTubeMusicSourceSettings();
        
        // Initialize services with temporary client (will be recreated in OnInitializeAsync)
        _client = CreateYouTubeMusicClient();
        _searchService = new YouTubeMusicSearchService(_client, _settings);
        _downloadService = new YouTubeMusicDownloadService(_settings);
        _metadataService = new YouTubeMusicMetadataService(_client, _settings);
        _playlistService = new YouTubeMusicPlaylistService(_client, _settings);
        
        SimpleLogger.Info("YouTube Music Source Provider created");
    }
    
    protected override async Task OnInitializeAsync(CancellationToken cancellationToken = default)
    {
        SimpleLogger.Info("Initializing YouTube Music Source Provider...");
        
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _settings.LoadAsync();
            
            // Generate session tokens if needed and enabled BEFORE creating the client
            if (_settings.AutoGenerateSession && 
                (string.IsNullOrEmpty(_settings.VisitorData) || string.IsNullOrEmpty(_settings.PoToken)))
            {
                await GenerateSessionTokensAsync(cancellationToken);
            }
            
            // Recreate client with loaded settings (now including any newly generated tokens)
            _client = CreateYouTubeMusicClient();
            
            // Update services with new client
            var searchField = typeof(YouTubeMusicSearchService).GetField("_client", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            searchField?.SetValue(_searchService, _client);
            
            var metadataField = typeof(YouTubeMusicMetadataService).GetField("_client", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            metadataField?.SetValue(_metadataService, _client);
            
            var playlistField = typeof(YouTubeMusicPlaylistService).GetField("_client", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            playlistField?.SetValue(_playlistService, _client);
            
            cancellationToken.ThrowIfCancellationRequested();
            await _metadataService.InitializeAsync(cancellationToken);
            await _playlistService.InitializeAsync(cancellationToken);
            
            SimpleLogger.Info("YouTube Music Source Provider initialization completed");
        }
        catch (OperationCanceledException)
        {
            SimpleLogger.Warn("YouTube Music Source Provider initialization was cancelled");
            throw;
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to initialize YouTube Music Source Provider - continuing with defaults");
            // Don't throw - just log the error and continue
        }
    }
    
    protected override Task OnDisposeAsync()
    {
        _downloadService?.Dispose();
        SimpleLogger.Info("YouTube Music Source Provider disposed");
        return Task.CompletedTask;
    }
    
    private YouTubeMusicClient CreateYouTubeMusicClient()
    {
        try
        {
            var geographicalLocation = !string.IsNullOrEmpty(_settings.GeographicalLocation) 
                ? _settings.GeographicalLocation 
                : "US";
            
            var visitorData = !string.IsNullOrEmpty(_settings.VisitorData) 
                ? _settings.VisitorData 
                : null;
            
            var poToken = !string.IsNullOrEmpty(_settings.PoToken) 
                ? _settings.PoToken 
                : null;
            
            // Parse cookies if provided
            IEnumerable<Cookie>? cookies = null;
            if (!string.IsNullOrEmpty(_settings.Cookies))
            {
                cookies = ParseCookies(_settings.Cookies);
            }
            
            var client = new YouTubeMusicClient(geographicalLocation, visitorData, poToken, cookies);
            LogClientConfiguration();
            return client;
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to create YouTube Music client, using default configuration");
            var client = new YouTubeMusicClient("US", null, null, null);
            LogClientConfiguration();
            return client;
        }
    }
    
    private async Task GenerateSessionTokensAsync(CancellationToken cancellationToken)
    {
        try
        {
            SimpleLogger.Info("Generating session tokens...");
            
            // Create JavaScript environment configuration for PoToken generation
            var nodeEnvironment = !string.IsNullOrEmpty(_settings.NodeJsPath) 
                ? new YouTubeSessionGenerator.Js.Environments.NodeEnvironment(_settings.NodeJsPath)
                : new YouTubeSessionGenerator.Js.Environments.NodeEnvironment();
            
            var config = new YouTubeSessionConfig
            {
                JsEnvironment = nodeEnvironment
            };
            
            var generator = new YouTubeSessionCreator(config);
            
            // Generate visitor data first
            var visitorData = await generator.VisitorDataAsync(cancellationToken);
            _settings.VisitorData = visitorData;
            
            // Generate PoToken using the visitor data
            var poToken = await generator.ProofOfOriginTokenAsync(visitorData, null, cancellationToken);
            _settings.PoToken = poToken;
            
            await _settings.SaveAsync();
            
            SimpleLogger.Info("Session tokens generated and saved successfully");
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to generate session tokens - continuing without them");
        }
    }
    
    private static IEnumerable<Cookie> ParseCookies(string cookieString)
    {
        if (string.IsNullOrWhiteSpace(cookieString))
            return Enumerable.Empty<Cookie>();
        
        // Check if it's Netscape format (contains tabs and has # Netscape HTTP Cookie File header)
        if (cookieString.Contains('\t') || cookieString.Contains("# Netscape HTTP Cookie File"))
        {
            return NetscapeCookieParser.ParseNetscapeCookieFile(cookieString);
        }
        
        // Otherwise, parse as semicolon-separated format
        var cookies = new List<Cookie>();
        
        try
        {
            var cookiePairs = cookieString.Split(';', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var pair in cookiePairs)
            {
                var trimmedPair = pair.Trim();
                var equalIndex = trimmedPair.IndexOf('=');
                
                if (equalIndex > 0)
                {
                    var name = trimmedPair.Substring(0, equalIndex).Trim();
                    var value = trimmedPair.Substring(equalIndex + 1).Trim();
                    
                    cookies.Add(new Cookie
                    {
                        Name = name,
                        Value = value,
                        Domain = ".youtube.com"
                    });
                }
            }
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to parse cookies");
        }
        
        return cookies;
    }
    
    /// <summary>
    /// Validates the current authentication by attempting a simple API call
    /// </summary>
    public async Task<bool> ValidateAuthenticationAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_client == null)
                return false;
            
            // Validate by checking if cookies contain required authentication cookies
            if (string.IsNullOrEmpty(_settings.Cookies))
                return false;
            
            var cookies = ParseCookies(_settings.Cookies);
            if (!NetscapeCookieParser.ValidateCookies(cookies))
                return false;
            
            // Try a simple search to validate authentication works
            try
            {
                var searchResults = _client.SearchAsync("test", YouTubeMusicAPI.Models.Search.SearchCategory.Songs);
                await searchResults.FetchItemsAsync(0, 1);
                return true;
            }
            catch (Exception ex)
            {
                SimpleLogger.Warn(ex, "Authentication test failed");
                return false;
            }
        }
        catch (Exception ex)
        {
            SimpleLogger.Warn(ex, "Authentication validation failed");
            return false;
        }
    }
    
    /// <summary>
    /// Manually regenerate session tokens
    /// </summary>
    public async Task RegenerateSessionTokensAsync()
    {
        try
        {
            SimpleLogger.Info("Manually regenerating session tokens...");
            await GenerateSessionTokensAsync(CancellationToken.None);
            
            // Recreate client with new tokens
            _client = CreateYouTubeMusicClient();
            
            // Update services with new client
            var searchField = typeof(YouTubeMusicSearchService).GetField("_client", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            searchField?.SetValue(_searchService, _client);
            
            var metadataField = typeof(YouTubeMusicMetadataService).GetField("_client", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            metadataField?.SetValue(_metadataService, _client);
            
            var playlistField = typeof(YouTubeMusicPlaylistService).GetField("_client", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            playlistField?.SetValue(_playlistService, _client);
            
            SimpleLogger.Info("Session tokens regenerated successfully");
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to regenerate session tokens");
            throw;
        }
    }
    
    /// <summary>
    /// Log client configuration for debugging
    /// </summary>
    private void LogClientConfiguration()
    {
        try
        {
            var settings = _settings;
            SimpleLogger.Debug("=== YouTube Music Client Configuration ===");
            SimpleLogger.Debug($"Geographical Location: {settings.GeographicalLocation}");
            SimpleLogger.Debug($"Has Visitor Data: {!string.IsNullOrEmpty(settings.VisitorData)}");
            SimpleLogger.Debug($"Has PoToken: {!string.IsNullOrEmpty(settings.PoToken)}");
            SimpleLogger.Debug($"Has Cookies: {!string.IsNullOrEmpty(settings.Cookies)}");
            SimpleLogger.Debug("========================================");
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to log client configuration");
        }
    }
}
