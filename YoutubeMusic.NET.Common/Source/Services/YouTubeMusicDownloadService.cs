using YoutubeMusic.NET.Common.Models;
using YoutubeMusic.NET.Common.Services;
using YoutubeMusic.NET.Common.Utils;
using YoutubeMusic.NET.Common.Source.Interfaces;

namespace YoutubeMusic.NET.Common.Source.Services;

public class YouTubeMusicDownloadService : IDownloadService
{
    private readonly YouTubeMusicSourceSettings _settings;
    private readonly HttpClient _httpClient;
    
    
    public YouTubeMusicDownloadService(YouTubeMusicSourceSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(_settings.RequestTimeoutSeconds);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", 
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        
        SimpleLogger.Info("YouTube Music Download Service initialized");
    }
    
    public async Task<Stream> GetAudioStreamAsync(AudioStreamInfo streamInfo, CancellationToken cancellationToken = default)
    {
        SimpleLogger.Info($"Getting audio stream from: {streamInfo.Url}");
        
        return await ExecuteWithRetryAsync(async () =>
        {
            // URL-decode the stream URL to fix "An invalid request URI was provided" error
            var decodedUrl = UrlUtils.DecodeUrlWithLogging(streamInfo.Url);
            
            var response = await _httpClient.GetAsync(decodedUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            
            SimpleLogger.Info($"Successfully obtained audio stream from: {decodedUrl}");
            return stream;
        }, "get audio stream", cancellationToken);
    }
    
    public async Task<long> GetContentLengthAsync(string url, CancellationToken cancellationToken = default)
    {
        SimpleLogger.Debug($"Getting content length for: {url}");
        
        return await ExecuteWithRetryAsync(async () =>
        {
            // URL-decode the URL to fix "An invalid request URI was provided" error
            var decodedUrl = UrlUtils.DecodeUrlWithLogging(url);
            
            using var request = new HttpRequestMessage(HttpMethod.Head, decodedUrl);
            using var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            var contentLength = response.Content.Headers.ContentLength ?? 0;
            
            SimpleLogger.Debug($"Content length for {decodedUrl}: {contentLength} bytes");
            return contentLength;
        }, "get content length", cancellationToken);
    }
    
    public bool SupportsDirectStreaming(AudioStreamInfo streamInfo)
    {
        // YouTube Music streams are generally streamable directly
        // Check if the URL is valid and not requiring special handling
        var isSupported = !string.IsNullOrEmpty(streamInfo.Url) && 
                         Uri.IsWellFormedUriString(streamInfo.Url, UriKind.Absolute);
        
        SimpleLogger.Debug($"Direct streaming support for {streamInfo.FormatId}: {isSupported}");
        return isSupported;
    }
    
    private async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, string operationName, CancellationToken cancellationToken = default)
    {
        var retryCount = _settings.RetryCount;
        var attempt = 0;
        
        while (true)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex) when (attempt < retryCount && !cancellationToken.IsCancellationRequested)
            {
                attempt++;
                SimpleLogger.Warn(ex, $"Attempt {attempt} failed for {operationName}, retrying... ({retryCount - attempt} attempts remaining)");
                
                // Exponential backoff: wait 1s, 2s, 4s, etc.
                var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt - 1));
                await Task.Delay(delay, cancellationToken);
            }
        }
    }
    
    public void Dispose()
    {
        _httpClient?.Dispose();
        SimpleLogger.Info("YouTube Music Download Service disposed");
    }
}
