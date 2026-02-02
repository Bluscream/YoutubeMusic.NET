using System.Web;
using YoutubeMusic.NET.Common.Services;

namespace YoutubeMusic.NET.Common.Utils;

public static class UrlUtils
{
    /// <summary>
    /// Decodes a URL-encoded string to fix "An invalid request URI was provided" errors.
    /// This is needed when URLs come from external APIs that may be double-encoded.
    /// </summary>
    /// <param name="url">The URL-encoded string to decode</param>
    /// <returns>The decoded URL string</returns>
    public static string DecodeUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return url;
            
        return HttpUtility.UrlDecode(url);
    }
    
    /// <summary>
    /// Decodes a URL-encoded string and logs the transformation for debugging.
    /// </summary>
    /// <param name="url">The URL-encoded string to decode</param>
    /// <returns>The decoded URL string</returns>
    public static string DecodeUrlWithLogging(string url)
    {
        if (string.IsNullOrEmpty(url))
            return url;
            
        var decodedUrl = HttpUtility.UrlDecode(url);
        
        SimpleLogger.Debug($"URL decoding: '{url}' -> '{decodedUrl}'");
        
        return decodedUrl;
    }
}
