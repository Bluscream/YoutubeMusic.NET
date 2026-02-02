using SimpMusic.Lyrics.Client.Api;
using SimpMusic.Lyrics.Client.Model;
using YoutubeMusic.NET.Common.Utils;

namespace YoutubeMusic.NET.Common.Services;

public class LyricsService
{
    private readonly LyricsApi _lyricsApi;
    
    public LyricsService()
    {
        _lyricsApi = new LyricsApi();
    }
    
    /// <summary>
    /// Fetches lyrics for a YouTube video ID
    /// </summary>
    /// <param name="videoId">YouTube video ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Lyrics response or null if not found</returns>
    public async Task<LyricsResponse?> GetLyricsAsync(string videoId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(videoId))
        {
            SimpleLogger.Warn("Cannot fetch lyrics: video ID is empty");
            return null;
        }
        
        try
        {
            SimpleLogger.Info($"Fetching lyrics for video ID: {videoId}");
            
            var lyricsResponse = await _lyricsApi.GetLyricsByVideoIdAsync(videoId, null, null);
            
            if (lyricsResponse != null)
            {
                SimpleLogger.Info($"Successfully fetched lyrics for video ID: {videoId} - Title: {lyricsResponse.SongTitle ?? "(null)"}, Artist: {lyricsResponse.ArtistName ?? "(null)"}");
            }
            else
            {
                SimpleLogger.Info($"Lyrics not found for video ID: {videoId} (response is null)");
            }
            
            return lyricsResponse;
        }
        catch (SimpMusic.Lyrics.Client.ApiException ex) when (ex.ErrorCode == 404)
        {
            SimpleLogger.Info($"Lyrics not found for video ID: {videoId}");
            return null;
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, $"Error while fetching lyrics for video ID: {videoId}");
            return null;
        }
    }
    
    /// <summary>
    /// Fetches translated lyrics for a YouTube video ID and language
    /// </summary>
    /// <param name="videoId">YouTube video ID</param>
    /// <param name="language">Language code (e.g., "en", "es", "fr")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Translated lyrics response or null if not found</returns>
    public async Task<TranslatedLyricsListResponse?> GetTranslatedLyricsAsync(string videoId, string language, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(videoId))
        {
            SimpleLogger.Warn("Cannot fetch translated lyrics: video ID is empty");
            return null;
        }
        
        if (string.IsNullOrWhiteSpace(language))
        {
            SimpleLogger.Warn("Cannot fetch translated lyrics: language is empty");
            return null;
        }
        
        try
        {
            SimpleLogger.Info($"Fetching translated lyrics for video ID: {videoId}, language: {language}");
            var translationsApi = new TranslationsApi();
            var translatedResponse = await translationsApi.GetTranslatedLyricsByVideoIdAndLanguageAsync(videoId, language);
            
            if (translatedResponse != null && translatedResponse.Data != null && translatedResponse.Data.Count > 0)
            {
                SimpleLogger.Info($"Successfully fetched {translatedResponse.Data.Count} translated lyric(s) for video ID: {videoId}, language: {language}");
                // Log the first translated lyric text for debugging
                var firstLyric = translatedResponse.Data.FirstOrDefault();
                if (firstLyric != null && !string.IsNullOrWhiteSpace(firstLyric._TranslatedLyric))
                {
                    SimpleLogger.Debug($"First translated lyric preview: {firstLyric._TranslatedLyric.Substring(0, Math.Min(100, firstLyric._TranslatedLyric.Length))}...");
                }
            }
            else
            {
                SimpleLogger.Info($"Translated lyrics not found for video ID: {videoId}, language: {language}");
            }
            
            return translatedResponse;
        }
        catch (SimpMusic.Lyrics.Client.ApiException ex) when (ex.ErrorCode == 404)
        {
            SimpleLogger.Info($"Translated lyrics not found for video ID: {videoId}, language: {language}");
            return null;
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, $"Error while fetching translated lyrics for video ID: {videoId}, language: {language}");
            return null;
        }
    }
    
    public void Dispose()
    {
        // Client library manages its own resources
    }
}
