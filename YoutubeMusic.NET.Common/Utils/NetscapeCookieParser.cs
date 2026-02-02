using System.Net;
using YoutubeMusic.NET.Common.Services;

namespace YoutubeMusic.NET.Common.Utils;

/// <summary>
/// Utility class for parsing Netscape HTTP Cookie File format
/// Format: domain, flag, path, secure, expiration, name, value (tab-separated)
/// </summary>
public static class NetscapeCookieParser
{
    /// <summary>
    /// Parses a Netscape HTTP Cookie File format string into a collection of Cookie objects
    /// </summary>
    /// <param name="cookieFileContent">The content of the Netscape cookie file</param>
    /// <returns>A collection of parsed Cookie objects</returns>
    public static IEnumerable<Cookie> ParseNetscapeCookieFile(string cookieFileContent)
    {
        var cookies = new List<Cookie>();
        
        if (string.IsNullOrWhiteSpace(cookieFileContent))
            return cookies;
        
        var lines = cookieFileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            
            // Skip comments and empty lines
            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
                continue;
            
            // Parse tab-separated values
            // Format: domain, flag, path, secure, expiration, name, value
            var parts = trimmedLine.Split('\t');
            
            if (parts.Length < 7)
            {
                // Try space-separated as fallback
                parts = trimmedLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 7)
                    continue;
            }
            
            try
            {
                var domain = parts[0].Trim();
                var flag = parts[1].Trim(); // Usually TRUE/FALSE for includeSubdomains
                var path = parts[2].Trim();
                var secure = parts[3].Trim().Equals("TRUE", StringComparison.OrdinalIgnoreCase);
                var expiration = parts[4].Trim();
                var name = parts[5].Trim();
                var value = parts[6].Trim();
                
                // Skip expired cookies (if expiration is a timestamp)
                if (double.TryParse(expiration, out var expirationTimestamp))
                {
                    var expirationDate = DateTimeOffset.FromUnixTimeSeconds((long)expirationTimestamp).DateTime;
                    if (expirationDate < DateTime.UtcNow)
                        continue;
                }
                
                var cookie = new Cookie
                {
                    Name = name,
                    Value = value,
                    Domain = domain.StartsWith(".") ? domain : $".{domain}",
                    Path = path,
                    Secure = secure,
                    HttpOnly = false // Netscape format doesn't specify HttpOnly
                };
                
                cookies.Add(cookie);
            }
            catch (Exception ex)
            {
                SimpleLogger.Warn(ex, $"Failed to parse cookie line: {trimmedLine}");
            }
        }
        
        return cookies;
    }
    
    /// <summary>
    /// Converts a collection of Cookie objects to Netscape HTTP Cookie File format
    /// </summary>
    /// <param name="cookies">The cookies to convert</param>
    /// <returns>A string in Netscape cookie file format</returns>
    public static string ToNetscapeCookieFile(IEnumerable<Cookie> cookies)
    {
        var lines = new List<string>
        {
            "# Netscape HTTP Cookie File",
            "# This is a generated file by YoutubeMusic.NET! Do not edit.",
            ""
        };
        
        foreach (var cookie in cookies)
        {
            // Calculate expiration timestamp (default to 1 year from now)
            var expiration = DateTimeOffset.UtcNow.AddYears(1).ToUnixTimeSeconds();
            
            // Format: domain, flag, path, secure, expiration, name, value
            var domain = cookie.Domain?.StartsWith(".") == true ? cookie.Domain.Substring(1) : cookie.Domain ?? "";
            var flag = "TRUE"; // Include subdomains
            var path = cookie.Path ?? "/";
            var secure = cookie.Secure ? "TRUE" : "FALSE";
            var expirationStr = expiration.ToString("F6");
            var name = cookie.Name;
            var value = cookie.Value;
            
            lines.Add($"{domain}\t{flag}\t{path}\t{secure}\t{expirationStr}\t{name}\t{value}");
        }
        
        return string.Join("\n", lines);
    }
    
    /// <summary>
    /// Validates that the parsed cookies contain required authentication cookies
    /// </summary>
    /// <param name="cookies">The cookies to validate</param>
    /// <returns>True if cookies appear valid (contain SAPISID or __Secure-3PAPISID)</returns>
    public static bool ValidateCookies(IEnumerable<Cookie> cookies)
    {
        var cookieList = cookies.ToList();
        
        // Check for required session cookies
        var hasSapIsId = cookieList.Any(c => 
            c.Name.Equals("SAPISID", StringComparison.OrdinalIgnoreCase) ||
            c.Name.Equals("__Secure-3PAPISID", StringComparison.OrdinalIgnoreCase) ||
            c.Name.Equals("__Secure-1PAPISID", StringComparison.OrdinalIgnoreCase));
        
        // Check for other important cookies
        var hasLoginInfo = cookieList.Any(c => 
            c.Name.Equals("LOGIN_INFO", StringComparison.OrdinalIgnoreCase));
        
        return hasSapIsId && !string.IsNullOrEmpty(cookieList.FirstOrDefault(c => 
            c.Name.Equals("SAPISID", StringComparison.OrdinalIgnoreCase) ||
            c.Name.Equals("__Secure-3PAPISID", StringComparison.OrdinalIgnoreCase) ||
            c.Name.Equals("__Secure-1PAPISID", StringComparison.OrdinalIgnoreCase))?.Value);
    }
}
